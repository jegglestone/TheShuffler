namespace Main
{
    using System.Collections.Generic;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class OpenXmlHelper
    {
        public static List<OpenXmlElement> BuildWordsIntoOpenXmlElement(Text[] textUnits)
        {
            var wordElements =
                new List<OpenXmlElement>();

            foreach (var text in textUnits)
            {
                if (text.Parent != null)
                    wordElements.Add(
                        text.Parent.CloneNode(true));
                else
                {
                    var wordRun = new Run();
                    wordRun.AppendChild(text);
                    wordElements.Add(wordRun);
                }
            }

            return wordElements;
        }
    }
}