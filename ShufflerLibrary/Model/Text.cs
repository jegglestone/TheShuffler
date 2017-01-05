﻿namespace ShufflerLibrary.Model
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
       
        private string _peTextRevised;
        public string pe_text_revised
        {
            get
            {
                if (_peTextRevised == "")
                    _peTextRevised = null;
                return _peTextRevised;
            }
            set { _peTextRevised = value; }
        }

        public string pe_rule_applied { get; set; }
        public int pe_order { get; set; }
        public int pe_C_num { get; set; }

        public bool IsNulThat => 
            pe_tag_revised == "NUL" 
            && (pe_text == " that " || pe_tag_revised == " that ");

        public bool IsClauser =>
            IsType(UnitTypes.CS_ClauserUnit);

        public bool IsAdverb =>
            IsType(UnitTypes.ADV_Adverb);

        public bool IsType(string unitType)
        {
            return (pe_tag_revised.IsNull() && pe_tag == unitType)
            || (pe_tag_revised == unitType);
        }

        public bool IsVbVbaPast =>
            IsType(UnitTypes.VB_Verb) 
            || IsType(UnitTypes.VBA_AuxilliaryVerb) 
            || IsType(UnitTypes.PAST_Participle);

        public bool IsVbPastPres =>
            IsType(UnitTypes.VB_Verb)
            || IsType(UnitTypes.PAST_Participle)
            || IsType("PRES");

        public bool IsTimer =>
            (pe_tag_revised.IsNull()  && 
            pe_tag.StartsWith(UnitTypes.TM_TimerPrefix) && 
            (pe_tag.Length <= 2 || pe_tag.Substring(2, 1).IsNumeric()))
            || 
            (!pe_tag_revised.IsNull() && pe_tag_revised.StartsWith(UnitTypes.TM_TimerPrefix) &&
            (pe_tag_revised.Length <= 2 || pe_tag_revised.Substring(2, 1).IsNumeric()));
    }

    public static class FieldValidationStringExtensions
    {
        public static bool IsNull(this string textField)
        {
            if (textField == null)
                return true;
            return textField.ToLower() == "null";
        }

        public static bool IsNumeric(this string textValue)
        {
            return textValue.All(char.IsDigit);
        }
    }
}

