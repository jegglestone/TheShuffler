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

            if (adverbSentenceDecorator
                .Texts
                .Take(adverbSentenceDecorator.AdverbIndexPosition)
                .Any(text => text.IsVbPastPres))
            {
                MoveAdverbBeforeVbPastPres(
                    adverbSentenceDecorator);
            }

            return sentence;
        }

        private static void MoveAdverbBeforeVbPastPres(
            AdverbSentenceDecorator adverbSentenceDecorator)
        {
            int adverbPosition = adverbSentenceDecorator.AdverbIndexPosition;
            int lastAdverbPosition = adverbSentenceDecorator.LastAdverbIndexPosition;

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