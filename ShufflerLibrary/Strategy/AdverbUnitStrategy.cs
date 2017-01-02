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

            int lastAdverPosition =
                adverbSentenceDecorator.LastAdverbIndexPosition;

            if (adverbSentenceDecorator.Texts.Take(adverbPosition).Any(
                text => text.IsVbPastPres))
            {
                var newSentence = new Sentence {pe_para_no = adverbSentenceDecorator.Pe_para_no};

                int vbPastPresPosition = 
                    adverbSentenceDecorator.Texts.Take(adverbPosition).ToList().FindLastIndex(
                    text => text.IsVbPastPres);
                   
                newSentence.Texts.AddRange(
                    adverbSentenceDecorator.Texts.GetRange(0, vbPastPresPosition));
                newSentence.Texts.AddRange(
                    adverbSentenceDecorator.Texts.GetRange(adverbPosition, lastAdverPosition - 1));
                newSentence.Texts.Add(
                    adverbSentenceDecorator.Texts[vbPastPresPosition]);
                newSentence.Texts.Add(
                    adverbSentenceDecorator.SentenceBreaker);

                return newSentence;
            }

            return sentence;
        }
    }
}
