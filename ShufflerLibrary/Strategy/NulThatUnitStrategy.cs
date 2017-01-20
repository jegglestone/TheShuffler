namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Model;

    public class NulThatUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsNulThat))
                return sentence;

            int nulThatPosition = 
                sentence.Texts.FindIndex(text => text.IsNulThat);

            if (sentence.Texts.Take(nulThatPosition).Any(text => text.IsNN))
            {
                int nnPosition = 
                    sentence.Texts.Take(nulThatPosition).ToList().FindIndex(text => text.IsNN);
                ShuffleNulThatBeforeNN(sentence, nulThatPosition, nnPosition);
            }

            return sentence;
        }

        private static void ShuffleNulThatBeforeNN(
            Sentence sentence, int nulThatPosition, int nnPosition)
        {
            int nulThatEndPosition = 
                GetNulThatEndPosition(sentence, nulThatPosition);

            var nulThatUnit =
                sentence.Texts.Skip(nulThatPosition).Take(nulThatEndPosition).ToList();

            sentence.Texts.RemoveRange(
                nulThatPosition, nulThatEndPosition);

            sentence.Texts.InsertRange(
                nnPosition, nulThatUnit);
        }

        private static int GetNulThatEndPosition(Sentence sentence, int nulThatPosition)
        {
            if (sentence.Texts.Count(text => text.IsComma) == 0
                || sentence.Texts.Count(text => text.IsComma) > 1)
            {
                return sentence
                .Texts
                .Skip(nulThatPosition).ToList().FindIndex(text => text.IsSentenceEnd);
            }

            return sentence
                .Texts
                .Skip(nulThatPosition).ToList().FindIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation));
        }
    }
}
