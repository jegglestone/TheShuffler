namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Model;

    public class ClauserUnitStrategy : IStrategy
    {
        private ClauserSentenceDecorator _clauserSentence;

        private static bool ClauserIsAlreadyAtBeginningOf(Sentence sentence)
        {
            return 
                sentence.Texts.First().pe_tag_revised == 
                    UnitTypes.CS_ClauserUnit ||
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
                    _clauserSentence, _clauserSentence.ClauserIndexPosition, sentence);
                return sentence;
            }

            ShuffleClauserUnitAndRestOfSentence(
                _clauserSentence, sentence, _clauserSentence.ClauserIndexPosition);

            return sentence;
        }

        private void ShuffleClauserUnitAndRestOfSentence(
            ClauserSentenceDecorator clauserSentenceDecorator, Sentence sentence, int clauserPosition)
        {
            int endOfSentencePosition = clauserSentenceDecorator.EndOfSentencePosition;

            List<Text> clauserTexts =
                clauserSentenceDecorator.GetClauserUnit(
                    clauserPosition, endOfSentencePosition);

            if (sentence.Texts.Take(clauserPosition).Any(
                text => text.IsMdNulThat))
            {
                MoveClauserUnitAndRestOfSentenceToAfterNulThat(
                    clauserSentenceDecorator.NulThatPosition,
                    clauserTexts,
                    sentence,
                    clauserSentenceDecorator);
            }
            else
            {
                MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
                    sentence, clauserTexts, clauserPosition);
            }
        }

        private static void ShuffleClauserUnitAndBKP(
            ClauserSentenceDecorator clauserSentenceDecorator, int clauserPosition, Sentence sentence)
        {
            int nbkpPosition =
                clauserSentenceDecorator.GetIndexPositionOfFirstBKPAfterClauser(
                    clauserPosition);

            List<Text> clauserTexts =
                clauserSentenceDecorator.GetClauserUnit(clauserPosition, nbkpPosition);

            if (sentence.Texts.Take(clauserPosition).Any(
                text => text.IsMdNulThat))
            {
                MoveClauserAndNBKPToAfterNulThat(
                    sentence, clauserPosition, clauserTexts, clauserSentenceDecorator);
            }
            else
            {
                MoveClauserAndNBKPToBeginningOfSentence(
                    sentence, clauserPosition, clauserTexts);
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
            IList<Text> clauserTexts,
            ClauserSentenceDecorator clauserSentenceDecorator)
        {
            sentence.Texts.RemoveRange(
                clauserPosition, clauserTexts.Count);

            int nulThatPosition =
                clauserSentenceDecorator.NulThatPosition;

            sentence.Texts.InsertRange(nulThatPosition+1, clauserTexts);
        }

        private void MoveClauserUnitAndRestOfSentenceToAfterNulThat(
            int nulThatPosition,
            IList<Text> clauserTexts,
            Sentence sentence,
            ClauserSentenceDecorator clauserSentenceDecorator)
        {
            sentence.Texts.RemoveRange(
                clauserSentenceDecorator.ClauserIndexPosition, clauserTexts.Count);

            clauserTexts.Add(
                CreateUnitProceedingCommaText(
                    sentence.pe_para_no, clauserTexts));

            sentence.Texts.InsertRange(
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
            int peParaNo, IList<Text> unitTexts)
        {
            return new Text
            {
                pe_para_no = peParaNo,
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