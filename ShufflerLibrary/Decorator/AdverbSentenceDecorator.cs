namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class AdverbSentenceDecorator
    {
        public List<Text> Texts
        {
            get
            {
                return _sentence.Texts;
            }
        }

        public int AdverbIndexPosition
        {
            get
            {
                if (!_sentence.HasAdverb()) return -1;

                if (_sentence.Texts.FindIndex(
                    text => text.pe_tag_revised == UnitTypes.ADV_Adverb) > -1)
                {
                    return _sentence.Texts.FindIndex(
                        text => text.pe_tag_revised == UnitTypes.ADV_Adverb);
                }

                return _sentence.Texts.FindIndex(
                    text => text.pe_tag == UnitTypes.ADV_Adverb &&
                            text.pe_tag_revised == "NULL");
            }
        }

        public int LastAdverbIndexPosition
        {
            get
            {
                return 
                    _sentence.Texts.FindLastIndex(
                        text => text.IsAdverb); 
            }
        }

        public Text SentenceBreaker
        {
            get { return _sentence.SentenceBreaker; }
        }

        public int Pe_para_no
        {
            get { return _sentence.pe_para_no; }
        }

        private readonly Sentence _sentence;

        public AdverbSentenceDecorator(Sentence sentence)
        {
            _sentence = sentence;
        }


    }
}
