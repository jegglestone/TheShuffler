namespace ShufflerLibrary.Model
{
    using System.Linq;

    public class Text : PhraseElement
    {
        public string actual_tag_used
        {
            get
            {
                if (pe_tag_revised.IsNull())
                    return pe_tag;
                return pe_tag_revised;
            }
        }

        public string actual_text_used
        {
            get
            {
                if( pe_text_revised.IsNull())
                    return pe_text;
                return pe_text_revised;
            }
        }

        public int pe_user_id { get; set; }
        public int pe_phrase_id { get; set; }
        public int? pe_word_id { get; set; }
        public string pe_tag { get; set; }
        public string pe_text { get; set; }
        public string pe_tag_revised { get; set; }
        public string pe_tag_revised_by_Shuffler { get; set; }
        public int pe_merge_ahead { get; set; }
        public int Sentence_Option { get; set; } = 1;
        public string pe_text_revised { get; set; }
        public string pe_rule_applied { get; set; }
        public int pe_order { get; set; }
        public int pe_C_num { get; set; }

        public bool IsComma =>
            IsType(UnitTypes.BKP_BreakerPunctuation)
            && actual_text_used.Replace(" ", "") == ",";

        public bool IsMdNulThat => 
            IsType(UnitTypes.MDNUL)
            && actual_text_used.Replace(" ", "").ToLower() == "that";

        public bool IsClauser =>
            IsType(UnitTypes.CS_ClauserUnit);

        public bool IsAdverb =>
            IsType(UnitTypes.ADV_Adverb)
            || IsNumberedType(UnitTypes.ADV_Adverb);

        public bool IsVbVbaPast =>
            IsType(UnitTypes.VB_Verb) 
            || IsType(UnitTypes.VBA_AuxilliaryVerb) 
            || IsType(UnitTypes.PAST_Participle);

        public bool IsVbPastPres =>
            IsType(UnitTypes.VB_Verb)
            || IsType(UnitTypes.PAST_Participle)
            || IsType(UnitTypes.PRES_Participle);

        public bool IsTimer =>
            IsType(UnitTypes.TM_TimerPrefix)
            || IsNumberedType(UnitTypes.TM_TimerPrefix);

        public bool IsModifier =>
            IsType(UnitTypes.MD_Modifier)
            || IsNumberedType(UnitTypes.MD_Modifier);

        public bool IsNN =>
            IsType(UnitTypes.NN) 
            || IsNumberedType(UnitTypes.NN);

        public bool IsPren =>
            IsType(UnitTypes.PREN_Pronoun)
            || IsNumberedType(UnitTypes.PREN_Pronoun);

        public bool IsPres =>
            IsType(UnitTypes.PRES_Participle);

        public bool IsPast =>
            IsType(UnitTypes.PAST_Participle);

        public bool IsBKBy =>
            IsType(UnitTypes.BK_Breaker)
            && actual_text_used == " by ";

        public bool IsSentenceEnd => 
            actual_text_used.Replace(" ", "") == "."
            && actual_tag_used == UnitTypes.BKP_BreakerPunctuation;

        public bool IsMDBK()
        {
            return pe_tag_revised_by_Shuffler == UnitTypes.MDBK
              || IsType(UnitTypes.MDBK);
        }

        public bool IsDe()
        {
            return IsType(UnitTypes.PY_ChineseWord)
                && actual_text_used.ToLower().Replace(" ", "") == "de";
        }

        public bool IsNNP => 
            IsType(UnitTypes.NNP);

        public bool IsPyXuyao =>
            actual_text_used.ToLower().Replace(" ", "") == "xuyao"
            && actual_tag_used == UnitTypes.PY_ChineseWord;

        public bool IsPyYo =>
            actual_text_used.ToLower().Replace(" ", "") == "yo"
            && actual_tag_used == UnitTypes.PY_ChineseWord;

        public bool IsPyJinxingde =>
            actual_text_used.ToLower().Replace(" ", "") == "jinxingde"
            && actual_tag_used == UnitTypes.PY_ChineseWord;

        public bool IsPercent =>
              actual_text_used.ToLower().Replace(" ", "") == "percent"
              || actual_text_used.ToLower().Replace(" ", "") == "%";
      

        public bool IsType(string unitType)
        {
            return (pe_tag_revised.IsNull() && pe_tag == unitType)
            || (pe_tag_revised == unitType);
        }

        public bool IsNumberedType(string unitType)
        {
            if (pe_tag_revised.IsNull() && pe_tag.IsNull())
                return false;

            return (pe_tag_revised.IsNull() &&
                    pe_tag.StartsWith(unitType)
                    && (pe_tag.Length <= 2
                        || pe_tag.Substring(pe_tag.Length - 1, 1).IsNumeric()))
                   ||
                   (!pe_tag_revised.IsNull()
                    && pe_tag_revised.StartsWith(unitType)
                    && (pe_tag_revised.Length <= 2
                        || pe_tag_revised.Substring(pe_tag_revised.Length - 1, 1).IsNumeric()));
        }
    }

    public static class FieldValidationStringExtensions
    {
        public static bool IsNull(this string textField)
        {
            if (textField == null)
                return true;
            if (textField == string.Empty)
                return false;
            return textField.ToLower() == "null";
        }

        public static bool IsNumeric(this string textValue)
        {
            return textValue.All(char.IsDigit);
        }
    }
}

