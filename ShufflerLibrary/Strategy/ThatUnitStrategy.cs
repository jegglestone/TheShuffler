namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Model;

    public class ThatUnitStrategy : IStrategy
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
                sentence
                .Texts
                .Skip(nulThatPosition).ToList().FindIndex(text => text.IsSentenceEnd); //TODO: logic here for commas etc. Extract nethod

            var nulThatUnit =
                sentence.Texts.Skip(nulThatPosition).Take(nulThatEndPosition);

            sentence.Texts.RemoveRange(
                nulThatPosition, nulThatEndPosition);

                sentence.Texts.InsertRange(
                    nnPosition, nulThatUnit);
        }
    }
}
