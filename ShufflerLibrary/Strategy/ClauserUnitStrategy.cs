namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Model;

    public class ClauserUnitStrategy : IStrategy
    {
        private ClauserSentenceDecorator _clauserSentence;

        private static int GetIndexPositionOfFirstBKPAfterClauser(Sentence sentence, int clauserPosition)
        {
            return sentence
                    .Texts
                    .GetRange(
                        clauserPosition, sentence.TextCount - clauserPosition)
                    .FindIndex(
                        text => (text.pe_tag == UnitTypes.BKP_BreakerPunctuation
                            || text.pe_tag_revised == UnitTypes.BKP_BreakerPunctuation) 
                            && text.pe_text == " , ") + clauserPosition;
        }

        private static int GetNulThatPosition(Sentence sentence)
        {
            return sentence.Texts.FindIndex(
                text => text.IsNulThat);
        }
        
        private static IEnumerable<Text> GetRemaingTextsCount(Sentence sentence, int position)
        {
            return sentence.Texts.GetRange(
                            position + 1,
                            sentence.TextCount - position - 1);
        }

        private static bool ClauserIsAlreadyAtBeginningOf(Sentence sentence)
        {
            return sentence.Texts.First().pe_tag_revised == UnitTypes.CS_ClauserUnit ||
                            (sentence.Texts.First().pe_tag_revised == "NULL"
                                && sentence.Texts.First().pe_tag == UnitTypes.CS_ClauserUnit);
        }

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasClauser())
                return sentence;

            if (ClauserIsAlreadyAtBeginningOf(sentence))
                return sentence;

            _clauserSentence = new ClauserSentenceDecorator(sentence);

            if (_clauserSentence.ClauserProceededByComma)
            {
                ShuffleClauserUnitAndBKP(
                    _clauserSentence, _clauserSentence.ClauserIndexPosition);
                return _clauserSentence.Sentence;
            }

            ShuffleClauserUnitAndRestOfSentence(
                _clauserSentence, _clauserSentence.ClauserIndexPosition);

            return _clauserSentence.Sentence;
        }

        private void ShuffleClauserUnitAndRestOfSentence(
            ClauserSentenceDecorator clauserSentence, int clauserPosition)
        {
            int endOfSentencePosition = clauserSentence.EndOfSentencePosition;

            List<Text> clauserTexts =
                clauserSentence.GetClauserUnit(
                    clauserPosition, endOfSentencePosition);

            if (clauserSentence.Sentence.Texts.Take(clauserPosition).Any(
                text => text.IsNulThat))
            {
                MoveClauserUnitAndRestOfSentenceToAfterNulThat(
                    GetNulThatPosition(clauserSentence.Sentence),
                    clauserTexts,
                    clauserSentence);
            }
            else
            {
                MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
                    clauserSentence.Sentence, clauserTexts, clauserPosition);

            }
        }

        private static void ShuffleClauserUnitAndBKP(
            ClauserSentenceDecorator clauserSentence, int clauserPosition)
        {
            int nbkpPosition =
                GetIndexPositionOfFirstBKPAfterClauser(
                    clauserSentence.Sentence, clauserPosition);

            List<Text> clauserTexts =
                clauserSentence.GetClauserUnit(clauserPosition, nbkpPosition);

            if (clauserSentence.Sentence.Texts.Take(clauserPosition).Any(
                text => text.IsNulThat))
            {
                MoveClauserAndNBKPToAfterNulThat(
                    clauserSentence.Sentence, clauserPosition, clauserTexts);
            }
            else
            {
                MoveClauserAndNBKPToBeginningOfSentence(
                    clauserSentence.Sentence, clauserPosition, clauserTexts);
            }
        }

        private static void MoveClauserAndNBKPToBeginningOfSentence(
            Sentence sentence, 
            int clauserPosition, 
            List<Text> clauserTexts)
        {
            sentence.Texts.RemoveRange(
                clauserPosition, clauserTexts.Count);

            sentence.Texts.InsertRange(
                0, clauserTexts);
        }

        private static void MoveClauserAndNBKPToAfterNulThat(
            Sentence sentence,
            int clauserPosition,
            IList<Text> clauserTexts)
        {
            sentence.Texts.RemoveRange(
                clauserPosition, clauserTexts.Count);

            int nulThatPosition =
                GetNulThatPosition(sentence);

            sentence.Texts.InsertRange(nulThatPosition+1, clauserTexts);
        }

        private void MoveClauserUnitAndRestOfSentenceToAfterNulThat(
            int nulThatPosition,
            IList<Text> clauserTexts,
            ClauserSentenceDecorator clauserSentence)
        {
            clauserSentence.Sentence.Texts.RemoveRange(
                clauserSentence.ClauserIndexPosition, clauserTexts.Count);

            clauserTexts.Add(
                CreateUnitProceedingCommaText(
                    clauserSentence.Sentence.pe_para_no, clauserTexts));

            clauserSentence.Sentence.Texts.InsertRange(
                nulThatPosition+1, clauserTexts);
        }

        private static void MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
            Sentence sentence, IList<Text> clauserTexts, int clauserPosition)
        {
            sentence.Texts.RemoveRange(
                clauserPosition,
                clauserTexts.Count);

            sentence.Texts.Insert(
                0,
                CreateUnitProceedingCommaText(
                    sentence.pe_para_no, clauserTexts));

            sentence.Texts.InsertRange(
                0, clauserTexts);
        }

        private static Text CreateUnitProceedingCommaText(
            int pe_para_no, IList<Text> unitTexts)
        {
            return new Text
            {
                pe_para_no = pe_para_no,
                pe_order = unitTexts[unitTexts.Count - 1].pe_order + 5,
                pe_tag = UnitTypes.BKP_BreakerPunctuation,
                pe_tag_revised = UnitTypes.BKP_BreakerPunctuation,
                pe_text = " , ",
                pe_text_revised = " , ",
                pe_user_id = unitTexts[0].pe_user_id
            };
        }        
    }
}