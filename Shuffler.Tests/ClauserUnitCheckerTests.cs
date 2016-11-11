namespace Shuffler.Tests
{
    using Helper;
    using Microsoft.Office.Interop.Word;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ClauserUnitCheckerTests
    {
        private ClauserUnitChecker _clauserUnitChecker;

        [SetUp]
        public void Setup()
        {
            _clauserUnitChecker = new ClauserUnitChecker();
        }

        [Test]
        public void IsValidUnit_When_Superscript_And_CS_ReturnsTrue()
        {
            // arrange
            var moqFont = new Mock<Font>();
            moqFont.Setup(x => x.Superscript).Returns(1);

            var moqSelection = new Mock<Selection>();
            moqSelection.Setup(x => x.Text).Returns("CS");
            moqSelection.Setup(x => x.Font).Returns(moqFont.Object);

            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(moqSelection.Object), Is.EqualTo(true));
        }

        [Test]
        public void IsValidUnit_When_NormalFont_ReturnsFalse()
        {
            // arrange
            var moqFont = new Mock<Font>();
            moqFont.Setup(x => x.Superscript).Returns(0);

            var moqSelection = new Mock<Selection>();
            moqSelection.Setup(x => x.Text).Returns("CS");
            moqSelection.Setup(x => x.Font).Returns(moqFont.Object);

            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(moqSelection.Object), Is.EqualTo(false));
        }

        [Test]
        public void IsValidUnit_When_Not_CS_ReturnsFalse()
        {
            // arrange
            var moqFont = new Mock<Font>();
            moqFont.Setup(x => x.Superscript).Returns(1);

            var moqSelection = new Mock<Selection>();
            moqSelection.Setup(x => x.Text).Returns("NOT");
            moqSelection.Setup(x => x.Font).Returns(moqFont.Object);

            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(moqSelection.Object), Is.EqualTo(false));
        }
    }
}
