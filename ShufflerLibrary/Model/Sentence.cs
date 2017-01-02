﻿namespace ShufflerLibrary.Model
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class Sentence : PhraseElement
    {
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
                                (text.pe_tag_revised == "NULL" && text.pe_tag == UnitTypes.CS_ClauserUnit)
                                || (text.pe_tag_revised == UnitTypes.CS_ClauserUnit));
        }
    }
}
