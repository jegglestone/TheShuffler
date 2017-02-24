namespace ShufflerLibrary.Decorator
{
    using System.Linq;
    using Model;

    public class AdverbSentenceDecorator : SentenceDecorator
    {
        public int AdverbIndexPosition
        {
            get
            {
                if (!Sentence.HasAdverb()) return -1;

                return Sentence.Texts.FindIndex(
                    text => text.IsAdverb);
            }
        }

        public int LastAdverbIndexPosition
        {
            get
            {
                return 
                    Sentence.Texts.FindLastIndex(
                        text => text.IsAdverb); 
            }
        }

        public bool AdjectiveAfterAdv(AdverbSentenceDecorator adverbSentenceDecorator)
        {
            int breakerPositionAfterAdverb =
                    adverbSentenceDecorator
                                .Texts
                                .Skip(adverbSentenceDecorator.AdverbIndexPosition)
                                .ToList()
                                .FindIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation) || text.IsVba || text.IsVbPastPres);

            return adverbSentenceDecorator
                .Texts
                .Skip(adverbSentenceDecorator.AdverbIndexPosition)
                .Take(breakerPositionAfterAdverb)
                .Any(text => text.IsType(UnitTypes.ADJ_Adjective));
        }

        public bool VbPastPresBeforeAdv(AdverbSentenceDecorator adverbSentenceDecorator)
        {
            int breakerPosition =
                    adverbSentenceDecorator
                    .Texts
                    .Take(adverbSentenceDecorator.AdverbIndexPosition)
                    .ToList()
                    .FindLastIndex(text => text.IsBkp || text.IsType(UnitTypes.NbkpNonBreakerPunctuation));
            if (breakerPosition == -1) breakerPosition = 0;

            return adverbSentenceDecorator
                .Texts
                .Skip(breakerPosition)
                .Take(adverbSentenceDecorator.AdverbIndexPosition - breakerPosition)
                .Any(text => text.IsVbPastPres);
        }

        public AdverbSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }
    }
}
