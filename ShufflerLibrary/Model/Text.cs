namespace ShufflerLibrary.Model
{
    using System.Linq;

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

        public bool IsClauser =>
            (pe_tag_revised.IsNull() && pe_tag == UnitTypes.CS_ClauserUnit)
            || (pe_tag_revised == UnitTypes.CS_ClauserUnit);

        public bool IsAdverb =>
            (pe_tag_revised.IsNull() && pe_tag == UnitTypes.ADV_Adverb)
            || (pe_tag_revised == UnitTypes.ADV_Adverb);

        public bool IsVbPastPres => 
            pe_tag_revised == "VB" || pe_tag_revised == "PAST" || pe_tag_revised == "PRES" 
            || (pe_tag_revised.IsNull() && (pe_tag == "VB" || pe_tag == "PAST" || pe_tag == "PRES"));

        public bool IsTimer =>
            (pe_tag_revised.IsNull() && 
            pe_tag.StartsWith(UnitTypes.TM_TimerPrefix) && 
            (pe_tag.Length <= 2 || pe_tag.Substring(2, 1).IsNumeric()))
            || 
            (pe_tag_revised.StartsWith(UnitTypes.TM_TimerPrefix) &&
            ( pe_tag_revised.Length <= 2 || pe_tag_revised.Substring(2, 1).IsNumeric()));
    }

    public static class FieldValidationStringExtensions
    {
        public static bool IsNull(this string textField)
        {
            return textField.ToLower() == "null";
        }

        public static bool IsNumeric(this string textValue)
        {
            return textValue.All(char.IsDigit);
        }
    }
}

