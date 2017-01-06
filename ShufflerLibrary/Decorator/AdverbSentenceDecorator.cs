namespace ShufflerLibrary.Decorator
{
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

        public AdverbSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }
    }
}
