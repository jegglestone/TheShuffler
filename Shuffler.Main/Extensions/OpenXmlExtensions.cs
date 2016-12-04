namespace Main.Extensions
{
    using System;
    using Constants;
    using DocumentFormat.OpenXml;

    public static class OpenXmlExtensions
    {
        public static bool IsAdverb(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.RemoveWhiteSpaces() == TagMarks.AdverbTag;
        }

        public static bool IsAdverb(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.AdverbTag;
        }

        public static bool IsVBA(this string textElement)
        {
            return textElement.RemoveWhiteSpaces() == TagMarks.VBA;
        }

        public static bool IsTimer(this string textElement)
        {
            var value = textElement.RemoveWhiteSpaces();
            if (value == string.Empty)
                return false;
            if (textElement.Length < 2)
                return false;
            var tagPrefix = value.Substring(0, 2);
            return tagPrefix == TagMarks.TimerTag;
        }

        public static bool ReachedSentenceBreaker(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "VB"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PAST"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PRES"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "BKP";
        }

        public static bool IsVbPastPres(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "VB"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PAST"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PRES";
        }

        public static bool IsTimer(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.IsTimer();
        }
    }
}
