namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class ClauserUnitStrategy : IStrategy
    {
        private static bool NoClauserFound(Sentence sentence)
        {
            return sentence.Texts.All(
                            text => text.pe_tag != UnitTypes.CS_ClauserUnit);
        }

        private static bool ClauserProceededByNBKP(Sentence sentence)
        {
            return 
                sentence.Texts.Skip(GetClauserIndexPosition(sentence)).Any(
                    text => text.pe_tag == UnitTypes.NBKP_NonBreakerPunctuation); 
        }

        private static int GetClauserIndexPosition(Sentence sentence)
        {
            return sentence.Texts.FindIndex(
                text => text.pe_tag == UnitTypes.CS_ClauserUnit);
        }

        private static int SentenceSize(Sentence sentence)
        {
            return sentence.Texts.Count;
        }

        private static int GetIndexPositionOfFirstNBKPAfterClauser(Sentence sentence, int clauserPosition)
        {
            //TODO: cater for pe_tag_revised
            return sentence
                    .Texts
                    .GetRange(
                        clauserPosition, SentenceSize(sentence) - clauserPosition)
                    .FindIndex(
                        text => text.pe_tag == UnitTypes.NBKP_NonBreakerPunctuation
                            || text.pe_tag_revised == UnitTypes.NBKP_NonBreakerPunctuation) + clauserPosition;
        }

        private static int GetNulThatPosition(Sentence sentence)
        {
            return sentence.Texts.FindIndex(
                IsNulThat);
        }

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (NoClauserFound(sentence))
                return sentence;
            
            if (ClauserProceededByNBKP(sentence))
            {
                ShuffleClauserUnitAndNBKP(
                    sentence, GetClauserIndexPosition(sentence));
                return sentence;
            }
            
            ShuffleClauserUnitAndRestOfSentence(
                sentence, GetClauserIndexPosition(sentence));
            return sentence;
        }

        private void ShuffleClauserUnitAndRestOfSentence(
            Sentence sentence, int clauserPosition)
        {
            List<Text> newSentence;

            int endOfSentencePosition = SentenceSize(sentence)- 2; // zero index and full stop

            List<Text> clauserTexts =
                GetClauserUnit(sentence, clauserPosition, endOfSentencePosition);

            if (sentence.Texts.Take(clauserPosition).Any(
                IsNulThat)) // TODO: Before CS only
            {
                //move to after nulThat
                newSentence = 
                    MoveClauserUnitAndRestOfSentenceToAfterNulThat(
                        sentence, GetNulThatPosition(sentence), clauserTexts);
            }
            else
            {
                //move to beginning of sentence
                newSentence = 
                    MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
                        sentence, clauserTexts, clauserPosition);

            }

            sentence.Texts = newSentence;
        }

        private static void ShuffleClauserUnitAndNBKP(Sentence sentence, int clauserPosition)
        {
            List<Text> newSentence;

            int nbkpPosition =
                GetIndexPositionOfFirstNBKPAfterClauser(
                    sentence, clauserPosition);

            List<Text> clauserTexts = 
                GetClauserUnit(sentence, clauserPosition, nbkpPosition);

            if (sentence.Texts.Take(clauserPosition).Any(
                IsNulThat))
            {
                newSentence = MoveClauserAndNBKPToAfterNulThat(
                    sentence, clauserTexts, nbkpPosition);
            }
            else
            {
                newSentence = MoveClauserAndNBKPToBeginningOfSentence(
                    sentence, clauserPosition, clauserTexts, nbkpPosition);
            }
            sentence.Texts = newSentence;
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

        private static List<Text> GetRemaingTextsCount(Sentence sentence, int position)
        {
            return sentence.Texts.GetRange(
                            position + 1,
                            SentenceSize(sentence) - position - 1);
        }

        //TODO: Test coverage needed
        private static List<Text> MoveClauserAndNBKPToAfterNulThat(
            Sentence sentence,
            List<Text> clauserTexts,
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
            List<Text> clauserTexts)
        {
            List<Text> newSentence = new List<Text>();

            newSentence.AddRange(
                sentence.Texts.GetRange(0, nulThatPosition + 1));
            newSentence.AddRange(
                clauserTexts);
            newSentence.AddRange(
                sentence.Texts.GetRange(
                    nulThatPosition + 1,
                    SentenceSize(sentence) - newSentence.Count - 1)); // another minus one?
            newSentence.AddRange(
                sentence.Texts.GetRange(
                    SentenceSize(sentence) - 1, 1)); // full stop position

            return newSentence;
        }

        private List<Text> MoveClauserUnitAndRestOfSentenceToBeginningOfSentence(
            Sentence sentence, List<Text> clauserTexts, int clauserPosition)
        {
            List<Text> newSentence = new List<Text>();
            newSentence.AddRange(
                clauserTexts);
            newSentence.AddRange(
                sentence.Texts.GetRange(
                    clauserPosition, SentenceSize(sentence) - 1 - clauserPosition));

            return newSentence;
        }

        // TODO: property of Sentence?
        private static List<Text> GetClauserUnit(
            Sentence sentence, int clauserPosition, int lastIndexPosition)
        {
            List<Text> clauserTexts = new List<Text>();
            for (int i = clauserPosition; i <= lastIndexPosition; i++)
            {
                clauserTexts.Add(sentence.Texts[i]);
            }

            return clauserTexts;
        }

        // TODO: property of Text class?
        private static bool IsNulThat(Text text)
        {
            return text.pe_tag_revised == "NUL" &&
                            (text.pe_text == " that " || text.pe_tag_revised == " that ");
        }

        private static void MoveClauserUnitAndNKBPToAfterNulThat(Sentence sentence)
        {
            
        }

        public class UnitTypes
        {
            public const string CS_ClauserUnit = "CS";
            public const string NBKP_NonBreakerPunctuation = "NBKP";
        }
    }
}
