namespace Shuffler.Tests
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using NUnit.Framework;
    using Main;

    [TestFixture]
    public class AdverbStrategyTests
    {
        [Test]
        public void DontShuffle_AdverUnits_ThatAreInFrontOfAdjectives()
        {
            const string unShuffledSentence =
                "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new AdverbStrategy();

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleAdverbUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP."));

        }

        [Test]
        public void Underlines_FromFirstToLastAdverbUnit_IncludingAllUnitsInBetween()
        {
            const string unShuffledSentence =
                "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new AdverbStrategy();

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleAdverbUnits(paragraph);
            
            // assert
            Assert.That(shufflerParagraph.InnerXml,
                    Is.EqualTo(DocumentContentHelper.GetParagraphFromWordDocument(
                        "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP_EXPECTATION").InnerXml));
        }

    }
}
