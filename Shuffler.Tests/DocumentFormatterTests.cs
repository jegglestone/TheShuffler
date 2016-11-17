namespace Shuffler.Tests
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
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
            Paragraph paragraph = 
                GetParagraph_TMIn_April_and_May_BK_CShowever_BKP_PRENthe_NNreport_VBwasnt_ADJgood_BKP();

            var documentFormatter = new DocumentFormatter(new ClauserUnitChecker());

            // act
            var shufflerParagraph = 
                documentFormatter.ShuffleClauserUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }

        #region not finished tests
        //[Test]
        //public void ShuffleClauserUnits_when_clauserAtStartOfSentence_DontMoveIt()
        //{
        //    var mockRange = new Mock<Range>();
        //    mockRange.Setup(f => f.Text).Returns(
        //        "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP.");
        //    var sentence = mockRange.Object;

        //    var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

        //    // act
        //    documentFormatter.ShuffleClauserUnits(sentence);

        //    // assert
        //    Assert.That(sentence.Text, Is.EqualTo(
        //        "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        //}


        //[Test]
        //public void ShuffleClauserUnits_when_clauserHasComma_MoveTheClauserAndCommaToStartOfSentence()
        //{
        //    var mockRange = new Mock<Range>();
        //    mockRange.Setup(f => f.Text).Returns(
        //        "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP.");
        //    var sentence = mockRange.Object;

        //    var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

        //    // act
        //    documentFormatter.ShuffleClauserUnits(sentence);

        //    // assert
        //    Assert.That(sentence.Text, Is.EqualTo(
        //        "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP."));
        //}

        // multiple clauser units?
        // Does a clauser unit only end with BKP?

        #endregion

        private Paragraph GetParagraph_TMIn_April_and_May_BK_CShowever_BKP_PRENthe_NNreport_VBwasnt_ADJgood_BKP()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            using (
                var document =
                    WordprocessingDocument.Open(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        "\\Shuffler.TestFiles\\TestFiles\\TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP..docx"
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
                                x.InnerText.Contains(
                                    "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP"
                                    ));
                    return p as Paragraph;
                }
            }

            throw new XmlException();
        }
    }
}
