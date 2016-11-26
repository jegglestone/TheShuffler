namespace Main.Extensions
{
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

        public static bool ReachedSentenceBreaker(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "VB"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PAST"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "PRES"
                   || openXmlLeafElement.InnerText.RemoveWhiteSpaces() == "BKP";
        }
    }
}
