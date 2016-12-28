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
                MoveClauserUnitAndNBKPToBeginningOfSentence(sentence);
                return sentence;
            }

            // if (nulThat)
            //    move after NulThaT
            
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

        private static void MoveClauserUnitAndNBKPToBeginningOfSentence(Sentence sentence)
        {
            List<Text> newSentence = new List<Text>();
            
            int clauserPosition = sentence.Texts.FindIndex(
                text => text.pe_tag == UnitTypes.CS_ClauserUnit);
            int nbkpPosition =
                sentence
                    .Texts
                    .GetRange(
                        clauserPosition, sentence.Texts.Count - clauserPosition)
                    .FindIndex(
                        text => text.pe_tag == UnitTypes.NBKP_NonBreakerPunctuation) + clauserPosition;

            List<Text> clauserTexts = new List<Text>();
            for (int i = clauserPosition; i <= nbkpPosition; i++)
            {
                clauserTexts.Add(sentence.Texts[i]);
            }

            newSentence.AddRange(clauserTexts);
            newSentence.AddRange(sentence.Texts.GetRange(
                0, clauserPosition));
            newSentence.AddRange(sentence.Texts.GetRange(
                nbkpPosition + 1, 
                sentence.Texts.Count - nbkpPosition - 1));
            sentence.Texts = newSentence;
        }

        public class UnitTypes
        {
            public const string CS_ClauserUnit = "CS";
            public const string NBKP_NonBreakerPunctuation = "NBKP";
        }
    }
}
