namespace Shuffler.Tests
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using Main;
    using NUnit.Framework;

    public class PrenStrategyTests
    {
        [Test]
        public void WhenNoModifier_Returns_Input()
        {
            AssertReturnIsEqualToExpected(
                "TM1this time TM2last year BKP.",
                "TM1this time TM2last year BKP.");
        }

        [TestCase(
            "This VB1is PREN1a NNbook PREN2about NNwarBKP.",
            "This VB1is PREN2about warPREN1a book BKP.")]
        [TestCase(
            "PREN1a NNbook PREN2about NNwarBKP.",
            "PREN2about warPREN1a book BKP.")]
        public void PrenUnits_Are_ShuffledInDescendingOrder(
            string input, string expected)
        {
            AssertReturnIsEqualToExpected(
                input,
                expected);
        }

        private static void AssertReturnIsEqualToExpected(
            string input, string expected)
        {
            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(input);

            var prenStrategy = new PrenStrategy();

            //  act
            var shuffledParagraph =
                prenStrategy.ShuffleSentenceUnit(paragraph);

            Assert.That(shuffledParagraph.InnerText,
                Is.EqualTo(
                    expected));
        }
    }
}
