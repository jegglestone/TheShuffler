namespace Main.Extensions
{
    using Constants;
    using DocumentFormat.OpenXml;

    public static class OpenXmlExtensions
    {
        public static bool IsAdverb(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.RemoveWhiteSpaces() == TagMarks.Adverb;
        }

        public static bool IsAdverb(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.Adverb;
        }

        public static bool IsVBA(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.VBA;
        }

        public static bool IsVB(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.VB;
        }
        
        public static bool IsDG(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.DG;
        }
        
        public static bool IsPast(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.PastParticiple;
        }

        public static bool IsPren(this string textElement)
        {
            return textElement.RemoveWhiteSpaces().StartsWith(TagMarks.Pren);
        }

        public static bool IsDG(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.IsDG();
        }

        public static bool IsModifier(this string textElement)
        {
            return EvaluateNumberedUnit(
                textElement, TagMarks.Modifier);
        }

        public static bool IsTimer(this string textElement)
        {
            return EvaluateNumberedUnit(
                textElement, TagMarks.Timer);
        }

        private static bool EvaluateNumberedUnit(
            string textElement, string tagMark)
        {
            var value = textElement.RemoveWhiteSpaces();

            if (value == string.Empty)
                return false;

            if (textElement.Length < 2)
                return false;
            
            var tagPrefix = value.Substring(0, 2);

            return tagPrefix == tagMark;
        }

        public static bool ReachedSentenceBreaker(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.IsVB()
                   || openXmlLeafElement.InnerText.IsPast()
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PRES"
                   || openXmlLeafElement.InnerText.IsBreakerPunctuation();
        }

        public static bool IsVbPastPres(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.IsVB()
                   || openXmlLeafElement.InnerText.IsPast()
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PRES";
        }

        //IsVbVbaPast
        public static bool IsVbVbaPast(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.IsVB()
                    || openXmlLeafElement.InnerText.IsVBA()
                    || openXmlLeafElement.InnerText.IsPast();
        }

        public static bool IsTimer(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.IsTimer();
        }
    }
}
