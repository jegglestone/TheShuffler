namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class ClauserUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (NoClauserFound(sentence))
                return sentence;
            
            if (ClauserProceededByNBKP(sentence))
            {
                MoveClauserUnitAndNBKP(sentence);
                return sentence;
            }
            
            return sentence;
        }

        private static bool ClauserProceededByNBKP(Sentence sentence)
        {
            return sentence.Texts.Any(
                            text => text.pe_tag == UnitTypes.NBKP_NonBreakerPunctuation);
        }

        private static bool NoClauserFound(Sentence sentence)
        {
            return sentence.Texts.All(
                            text => text.pe_tag != UnitTypes.CS_ClauserUnit);
        }

        private static void MoveClauserUnitAndNBKP(Sentence sentence)
        {
            List<Text> newSentence = new List<Text>();

            int clauserPosition = sentence.Texts.FindIndex(
                text => text.pe_tag == UnitTypes.CS_ClauserUnit);

            //TODO: cater for pe_tag_revised
            int nbkpPosition =
                sentence
                    .Texts
                    .GetRange(
                        clauserPosition, sentence.Texts.Count - clauserPosition)
                    .FindIndex(
                        text => text.pe_tag == UnitTypes.NBKP_NonBreakerPunctuation
                            || text.pe_tag_revised == UnitTypes.NBKP_NonBreakerPunctuation) + clauserPosition;

            List<Text> clauserTexts = new List<Text>();
            for (int i = clauserPosition; i <= nbkpPosition; i++)
            {
                clauserTexts.Add(sentence.Texts[i]);
            }

            if (clauserTexts.Any(
                IsNulThat))
            {
                //move to after nulThat 
                int nulThatPosition =
                    sentence.Texts.FindIndex(
                        IsNulThat);

                // get everything up to NulThat
                newSentence.AddRange(sentence.Texts.GetRange(0, nulThatPosition));
                // add clauserUnit
                newSentence.AddRange(clauserTexts);
                // add rest of sentence
                newSentence.AddRange(sentence.Texts.GetRange(nulThatPosition + 1, 
                    sentence.Texts.Count - nbkpPosition - 1)); //TODO: duplication - GetRemainingTextsCount
            }
            else
            {
                //move to beginning of sentence
                newSentence.AddRange(clauserTexts);
                newSentence.AddRange(sentence.Texts.GetRange(
                    0, clauserPosition));
                newSentence.AddRange(sentence.Texts.GetRange(
                    nbkpPosition + 1,
                    sentence.Texts.Count - nbkpPosition - 1));

            }
            sentence.Texts = newSentence;
        }

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
