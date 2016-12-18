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
    }
}
