namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Model;

    public class ClauserUnitStrategy : IStrategy
    {
        private ClauserSentenceDecorator _clauserSentence;

        private static int GetIndexPositionOfFirstNBKPAfterClauser(Sentence sentence, int clauserPosition)
        {
            return sentence
                    .Texts
                    .GetRange(
                        clauserPosition, sentence.TextCount - clauserPosition)
                    .FindIndex(
                        text => text.pe_tag == UnitTypes.NBKP_NonBreakerPunctuation
                            || text.pe_tag_revised == UnitTypes.NBKP_NonBreakerPunctuation) + clauserPosition;
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

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasClauser())
                return sentence;

            _clauserSentence = new ClauserSentenceDecorator(sentence);
            
            if (_clauserSentence.ClauserProceededByNBKP)
            {
                ShuffleClauserUnitAndNBKP(
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
            List<Text> newSentence;

            int endOfSentencePosition = clauserSentence.Sentence.TextCount - 2; // zero index and full stop

            List<Text> clauserTexts =
                clauserSentence.GetClauserUnit(clauserPosition, endOfSentencePosition);

            if (clauserSentence.Sentence.Texts.Take(clauserPosition).Any(
                text => text.IsNulThat)) // TODO: Before CS only
            {
                //move to after nulThat
                newSentence = 
                    MoveClauserUnitAndRestOfSentenceToAfterNulThat(
                        clauserSentence.Sentence, GetNulThatPosition(clauserSentence.Sentence), clauserTexts);
            }
            else
            {
                //move to beginning of sentence
                newSentence = 
                    MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
                        clauserSentence.Sentence  , clauserTexts, clauserPosition);

            }

            clauserSentence.Sentence.Texts = newSentence;
        }

        private static void ShuffleClauserUnitAndNBKP(ClauserSentenceDecorator clauserSentence, int clauserPosition)
        {
            List<Text> newSentence;

            int nbkpPosition =
                GetIndexPositionOfFirstNBKPAfterClauser(
                    clauserSentence.Sentence, clauserPosition);

            List<Text> clauserTexts =
                clauserSentence.GetClauserUnit(clauserPosition, nbkpPosition);

            if (clauserSentence.Sentence.Texts.Take(clauserPosition).Any(
                text => text.IsNulThat))
            {
                newSentence = MoveClauserAndNBKPToAfterNulThat(
                    clauserSentence.Sentence, clauserTexts, nbkpPosition);
            }
            else
            {
                newSentence = MoveClauserAndNBKPToBeginningOfSentence(
                    clauserSentence.Sentence, clauserPosition, clauserTexts, nbkpPosition);
            }
            clauserSentence.Sentence.Texts = newSentence;
        }

        private static List<Text> MoveClauserAndNBKPToBeginningOfSentence(
            Sentence sentence, 
            int clauserPosition, 
            List<Text> clauserTexts, 
            int nbkpPosition)
        {
            List<Text> newSentence = new List<Text>();
            newSentence.AddRange(clauserTexts);
            newSentence.AddRange(sentence.Texts.GetRange(
                0, clauserPosition));
            newSentence.AddRange(GetRemaingTextsCount(sentence, nbkpPosition));
            return newSentence;
        }

        //TODO: Test coverage needed
        private static List<Text> MoveClauserAndNBKPToAfterNulThat(
            Sentence sentence,
            IEnumerable<Text> clauserTexts,
            int nbkpPosition)
        {
            int nulThatPosition =
                GetNulThatPosition(sentence);
            List<Text> newSentence = new List<Text>();

            newSentence.AddRange(
                sentence.Texts.GetRange(0, nulThatPosition + 1));
            newSentence.AddRange(
                clauserTexts);
            newSentence.AddRange(
                GetRemaingTextsCount(sentence, nbkpPosition));
            return newSentence;
        }

        private static List<Text> MoveClauserUnitAndRestOfSentenceToAfterNulThat(
            Sentence sentence, 
            int nulThatPosition,
            IReadOnlyList<Text> clauserTexts)
        {
            var newSentence = new List<Text>();

            newSentence.AddRange(
                sentence.Texts.GetRange(0, nulThatPosition + 1));
            newSentence.AddRange(
                clauserTexts);
            newSentence.Add(
                CreateUnitProceedingCommaText(sentence.pe_para_no, clauserTexts));
            newSentence.AddRange(
                sentence.Texts.GetRange(
                    nulThatPosition + 1,
                    sentence.TextCount - newSentence.Count));
            newSentence.Add(
                GetSentenceBreaker(sentence));

            return newSentence;
        }

        private List<Text> MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
            Sentence sentence, IReadOnlyList<Text> clauserTexts, int clauserPosition)
        {
            List<Text> newSentence = new List<Text>();
            newSentence.AddRange(
                clauserTexts);
            newSentence.Add(CreateUnitProceedingCommaText(
                sentence.pe_para_no, clauserTexts));
            newSentence.AddRange(
                sentence.Texts.GetRange(
                    0, clauserPosition));
            newSentence.Add(GetSentenceBreaker(sentence));

            return newSentence;
        }

        private static Text CreateUnitProceedingCommaText(
            int pe_para_no, IReadOnlyList<Text> unitTexts)
        {
            return new Text
            {
                pe_para_no = pe_para_no,
                pe_order = unitTexts[unitTexts.Count - 1].pe_order + 5,
                pe_tag = UnitTypes.NBKP_NonBreakerPunctuation,
                pe_text = " , ",
                pe_user_id = unitTexts[0].pe_user_id
            };
        }

        private static Text GetSentenceBreaker(Sentence sentence)
        {
            // check tag is bkp
            //return sentence.Texts.GetRange(
            //    sentence.TextCount - 1, 1);

            return sentence.Texts.Last(text => text.pe_tag == "BKP");
        }
    }
}