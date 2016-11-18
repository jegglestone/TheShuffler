namespace Shuffler.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using System.Xml;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Helper;

    [TestFixture]
    public class DocumentFormatterTests
    {    
        [Test]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentenceAndAddComma()
        {
            const string unShuffledSentence = 
                "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP.";

            Paragraph paragraph = 
                GetParagraphFromWordDocument(unShuffledSentence);

            var documentFormatter = new DocumentFormatter(new ClauserUnitChecker());

            // act
            var shufflerParagraph = 
                documentFormatter.ShuffleClauserUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP."));
        }


        [Test]
        public void ShuffleClauserUnits_when_NoComma_MoveEntireClauserUnitToStartOfSentence_AndAddComma()
        {
            const string unShuffledSentence =
                "PRENThe meeting VBwas over CSbefore he VBhad a chance VBto speak BKP.";

            Paragraph paragraph =
                GetParagraphFromWordDocument(unShuffledSentence);

            var documentFormatter = new DocumentFormatter(new ClauserUnitChecker());

            // act
            var shufflerParagraph =
                documentFormatter.ShuffleClauserUnits(paragraph);


            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }

        //[Test]
        //public void ShuffleClauserUnits_when_clauserAtStartOfSentence_DontMoveIt()
        //{
        //    same sentence

        //    // act
        //    documentFormatter.ShuffleClauserUnits(sentence);

        //    // assert
        //    Assert.That(sentence.Text, Is.EqualTo(
        //        "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        //}



        // multiple clauser units?
        // Does a clauser unit only end with BKP?


        private Paragraph GetParagraphFromWordDocument(string unShuffledSentence)
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
