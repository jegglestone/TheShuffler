namespace Shuffler.Tests
{
    using Shuffler.Helper;
    using Microsoft.Office.Interop.Word;
    using Moq;
    using Rhino.Mocks;
    using Range = Microsoft.Office.Interop.Word.Range;
    using NUnit.Framework;

    [TestFixture]
    public class DocumentFormatterTests
    {
        public IUnitChecker GetClauserUnitChecker()
        {
            var mockClauserUnitChecker = new Mock<IUnitChecker>();
             mockClauserUnitChecker.Setup(
                s => s.IsValidUnit(Arg<Selection>.Is.Anything)).Returns(true);

            return mockClauserUnitChecker.Object;
        }

        [Test]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentenceAndAddComma()
        {
            // arrange
            var mockRange = new Mock<Range>();
            mockRange.Setup(f => f.Text).Returns(
                "PRENThe meeting VBwas over CSbefore he VBhad a chance VBto speak BKP.");
            var sentence = mockRange.Object;

            var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

            // act
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

            var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

            // act
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

            var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

            // act
            documentFormatter.ShuffleClauserUnits(sentence);

            // assert
            Assert.That(sentence.Text, Is.EqualTo(
                "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP."));
        }

        // multiple clauser units?
        // Does a clauser unit only end with BKP?
    }
}
