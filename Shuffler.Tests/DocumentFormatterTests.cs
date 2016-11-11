using NUnit.Framework;
using CSWordRemoveBlankPage.Services;

namespace Shuffler.Tests
{
    using Moq;
    using MockRepository = Rhino.Mocks.MockRepository;
    using Range = Microsoft.Office.Interop.Word.Range;

    [TestFixture]
    public class DocumentFormatterTests
    {
        [Test]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentenceAndAddComma()
        {
            var mockRange = new Mock<Range>();
            mockRange.Setup(f => f.Text).Returns(
                "PRENThe meeting VBwas over CSbefore he VBhad a chance VBto speak BKP.");
          
            var sentence = mockRange.Object;

            // act
            var documentFormatter = new DocumentFormatter();
            documentFormatter.ShuffleClauserUnits(sentence);

            // assert
            Assert.That(sentence.Text, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }

        [Test]
        public void ShuffleClauserUnits_when_clauserAtStartOfSentence_DontMoveIt()
        {
            var mockRange = new Mock<Range>();
            mockRange.Setup(f => f.Text).Returns(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP.");

            var sentence = mockRange.Object;

            // act
            var documentFormatter = new DocumentFormatter();
            documentFormatter.ShuffleClauserUnits(sentence);

            // assert
            Assert.That(sentence.Text, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }


        [Test]
        public void ShuffleClauserUnits_when_clauserHasComma_MoveTheClauserAndCommaToStartOfSentence()
        {
            var mockRange = new Mock<Range>();
            mockRange.Setup(f => f.Text).Returns(
                "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP.");

            var sentence = mockRange.Object;

            // act
            var documentFormatter = new DocumentFormatter();
            documentFormatter.ShuffleClauserUnits(sentence);

            // assert
            Assert.That(sentence.Text, Is.EqualTo(
                "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP."));
        }
    }
}
