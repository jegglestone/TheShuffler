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

                if (Sentence.Texts.FindIndex(
                    text => text.pe_tag_revised == UnitTypes.ADV_Adverb) > -1)
                {
                    return Sentence.Texts.FindIndex(
                        text => text.pe_tag_revised == UnitTypes.ADV_Adverb);
                }

                return Sentence.Texts.FindIndex(
                    text => text.pe_tag == UnitTypes.ADV_Adverb &&
                            text.pe_tag_revised.IsNull());
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
