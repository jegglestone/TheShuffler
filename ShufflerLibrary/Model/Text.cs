namespace ShufflerLibrary.Model
{
    public class Text : PhraseElement
    {
        public int pe_user_id { get; set; }
        public int pe_phrase_id { get; set; }
        public int? pe_word_id { get; set; }
        public string pe_tag { get; set; }
        public string pe_text { get; set; }
        public string pe_tag_revised { get; set; }
        public int pe_merge_ahead { get; set; }
        public string pe_text_revised { get; set; }
        public string pe_rule_applied { get; set; }
        public int pe_order { get; set; }
        public int pe_C_num { get; set; }

        public bool IsNulThat => 
            pe_tag_revised == "NUL" 
            && (pe_text == " that " || pe_tag_revised == " that ");

        public bool IsAdverb =>
            (pe_tag_revised == "NULL" && pe_tag == UnitTypes.ADV_Adverb)
            || (pe_tag_revised == UnitTypes.ADV_Adverb);

        public bool IsVbPastPres => 
            pe_tag_revised == "VB" || pe_tag_revised == "PAST" || pe_tag_revised == "PRES" 
            || (pe_tag_revised == "NULL" && (pe_tag == "VB" || pe_tag == "PAST" || pe_tag == "PRES"));
    }
}
