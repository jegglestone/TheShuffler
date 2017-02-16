namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Model;

    public class DoublePrenStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (sentence.Texts.Count(
                text => text.IsPren) == 2)
            {
                int firstPrenPosition =
                    sentence.Texts.FindIndex(
                        text => text.IsPren);

                if (!PrenUnitIsFollowedByAnotherPrenUnit(
                        sentence, firstPrenPosition))
                    return sentence;

                MoveSecondPrenBeforeFirstPren(
                    sentence, firstPrenPosition);
            }

            return sentence;
        }

        private static bool PrenUnitIsFollowedByAnotherPrenUnit(Sentence sentence, int firstPrenPosition)
        {
            return sentence.Texts[firstPrenPosition + 2]
                .IsPren
                   || sentence.Texts[firstPrenPosition + 2]
                       .IsPren;
        }

        private static void MoveSecondPrenBeforeFirstPren(Sentence sentence, int firstPrenPosition)
        {
            int secondPrenPosition = firstPrenPosition + 2;

            var secondPrenUnit = sentence.Texts.GetRange(
                secondPrenPosition,
                2).ToList();

            sentence.Texts.RemoveRange(
                secondPrenPosition,
                2);

            sentence.Texts.InsertRange(
                firstPrenPosition,
                secondPrenUnit);
        }
    }
}
