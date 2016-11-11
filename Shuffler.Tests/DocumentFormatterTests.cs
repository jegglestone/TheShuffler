using NUnit.Framework;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using Rhino.Mocks;
using CSWordRemoveBlankPage.Services;

namespace Shuffler.Tests
{
    [TestFixture]
    public class DocumentFormatterTests
    {
        [Test]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentence()
        {
            // arrange
            var sentence = MockRepository.GenerateMock<Range>();
            sentence.Text = "PRENThe meeting VBwas over CSbefore he VBhad a chance VBto speak BKP.";

            /*var mockRange = new Mock<Word::Range>();
            mockRange.Setup(f => f.Text).Returns("This is the text");
            mockRange.Setup(f => f.Bookmarks).Returns(bookmarks);
            var range = mockRange.Object;*/


            // act
            var documentFormatter = new DocumentFormatter();
            documentFormatter.ShuffleClauserUnits(sentence);

            // assert
            Assert.That(sentence.Text, Is.EqualTo("CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }
    }
}
