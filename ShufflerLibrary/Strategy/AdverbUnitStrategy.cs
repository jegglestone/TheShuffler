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
                var newSentence = new Sentence {pe_para_no = adverbSentenceDecorator.Pe_para_no};

                int vbPastPresPosition = 
                    adverbSentenceDecorator.Texts.Take(adverbPosition).ToList().FindLastIndex(
                    text => text.IsVbPastPres);

                var beforeVbPastPresUnit = adverbSentenceDecorator.Texts.GetRange(
                    0, vbPastPresPosition);

                var adverbUnit = adverbSentenceDecorator.Texts.GetRange(adverbPosition,
                    lastAdverbPosition - (adverbPosition - 1));

                var pastPresUnit = adverbSentenceDecorator.Texts.GetRange(
                    vbPastPresPosition,
                    adverbSentenceDecorator.TextCount - adverbUnit.Count - beforeVbPastPresUnit.Count - 1);

                newSentence.Texts.AddRange(beforeVbPastPresUnit);
                newSentence.Texts.AddRange(adverbUnit);
                newSentence.Texts.AddRange(pastPresUnit);
                newSentence.Texts.Add(
                    adverbSentenceDecorator.SentenceBreaker);

                return newSentence;
            }

            return sentence;
        }
    }
}
