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
                    GetDocument(unShuffledSentence, "TestFiles", false))
            {
                var docPart = document.MainDocumentPart;
                if (docPart?.Document != null)
                {
                    OpenXmlElement documentBodyXml = docPart.Document.Body;
                    OpenXmlElement p =
                        documentBodyXml.FirstOrDefault(
                            x =>
                                x.LocalName == "p" &&
                                x.InnerText.Contains(unShuffledSentence.Replace("_Different", "")));
                    return p as Paragraph;
                }
            }

            throw new XmlException();
        }

        public static WordprocessingDocument GetMainDocumentPart(string fileName)
        {
            return GetDocument(fileName, "TestFiles\\MultiLineFiles", true);

        }

        private static WordprocessingDocument GetDocument(string fileName, string subDirectory, bool isEditable)
        {
            var path = string.Format("\\{0}\\{1}.docx", subDirectory, fileName);
            return WordprocessingDocument.Open(
                                    TestContext.CurrentContext.TestDirectory +
                                    path
                                    , isEditable);
        }
    }
}
