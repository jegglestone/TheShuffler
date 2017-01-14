namespace ShufflerLibrary.Model
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class Sentence : PhraseElement
    {
        public int Sentence_No { get; set; } = 1;

        private const int sentenceOptionSelected = 1;

        public int Sentence_Option_Selected => sentenceOptionSelected;

        public List<Text> Texts { get; set; } = new List<Text>();

        public int TextCount => Texts.Count;

        public Text SentenceBreaker
        {
            get
            {
                return Texts.Last(text => text.pe_tag == "BKP");
            }
        }

        public string SentenceLine
        {
            get
            {
                var sentenceLine = new StringBuilder(TextCount);
                foreach (var text in Texts)
                {
                    sentenceLine.Append(
                        text.pe_text_revised != "NULL" 
                        && text.pe_text_revised != ""
                            ? text.pe_text_revised 
                            : text.pe_text);
                }
                return sentenceLine.ToString();
            }
        }

        public bool HasClauser()
        {
            return Texts.Any(
                            text =>
                                text.IsClauser);
        }

        public bool HasAdverb()
        {
            return Texts.Any(
                text => text.IsAdverb);
        }

        public bool HasTimer()
        {
            return Texts.Any(
                text => text.IsTimer);
        }

        public bool HasVbVerb()
        {
            return Texts.Any(
                text => text.IsType(UnitTypes.VB_Verb));
        }

        public bool HasVBVBAPAST
        {
            get
            {
                return
                    HasVbVerb() || HasVbaVerb() || HasPastParticiple();
            }
        }
    
        public bool HasDG
        {
            get
            {
                 return Texts.Any(
                   text => text.IsType(UnitTypes.DG_Digit));  
            }
            
        }

        public bool HasVbaVerb()
        {
            return Texts.Any(
                text => text.IsType(UnitTypes.VBA_AuxilliaryVerb));
        }

        public bool HasPastParticiple()
        {
            return Texts.Any(
                text => text.IsType(UnitTypes.PAST_Participle));
        }

        public bool HasBKBy
        {
            get
            {
                return Texts.Any(
                    text => text.IsBKBy);
            }
        }
    }
}
