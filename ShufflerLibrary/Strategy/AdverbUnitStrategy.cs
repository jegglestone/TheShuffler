namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Decorator;
    using Model;

    public class AdverbUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasAdverb())
                return sentence;

            var adverbSentenceDecorator = 
                new AdverbSentenceDecorator(sentence);

            int adverbPosition = 
                adverbSentenceDecorator.AdverbIndexPosition;

            int lastAdverbPosition =
                adverbSentenceDecorator.LastAdverbIndexPosition;

            if (adverbSentenceDecorator.Texts.Take(adverbPosition).Any(
                text => text.IsVbPastPres))
            {
                MoveAdverbBeforeVbPastPres(
                    adverbSentenceDecorator, adverbPosition, lastAdverbPosition);
            }

            return sentence;
        }

        private static void MoveAdverbBeforeVbPastPres(AdverbSentenceDecorator adverbSentenceDecorator, int adverbPosition,
            int lastAdverbPosition)
        {
            int vbPastPresPosition =
                adverbSentenceDecorator.Texts.Take(adverbPosition).ToList().FindLastIndex(
                    text => text.IsVbPastPres);

            var adverbUnit = adverbSentenceDecorator.Texts.GetRange(
                adverbPosition, lastAdverbPosition - (adverbPosition - 1)).ToList();

            adverbSentenceDecorator.Texts.RemoveRange(
                adverbPosition, lastAdverbPosition - (adverbPosition - 1));

            adverbSentenceDecorator.Texts.InsertRange(
                vbPastPresPosition,
                adverbUnit);
        }
    }
}