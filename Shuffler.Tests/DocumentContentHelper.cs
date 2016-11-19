namespace Shuffler.Tests
{
    using System.Linq;
    using System.Xml;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using NUnit.Framework;

    public class DocumentContentHelper
    {
        public static Paragraph GetParagraphFromWordDocument(string unShuffledSentence)
        {
            using (
                var document =
                    WordprocessingDocument.Open(
                        TestContext.CurrentContext.TestDirectory +
                        string.Format("\\TestFiles\\{0}.docx", unShuffledSentence)
                        , false))
            {
                var docPart = document.MainDocumentPart;
                if (docPart?.Document != null)
                {
                    OpenXmlElement documentBodyXml = docPart.Document.Body;
                    OpenXmlElement p =
                        documentBodyXml.FirstOrDefault(
                            x =>
                                x.LocalName == "p" &&
                                x.InnerText.Contains(unShuffledSentence));
                    return p as Paragraph;
                }
            }

            throw new XmlException();
        }
    }
}
