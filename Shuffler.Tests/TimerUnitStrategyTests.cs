namespace Shuffler.Tests
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using Main;
    using NUnit.Framework;

    [TestFixture]
    public class TimerUnitStrategyTests
    {
        [Test]  // this will need changing once the 2nd test is implemented > Real GDP TM2last year TM1this time VBrose BKP.
        public void TimerUnits_Are_ShuffledInDescendingOrderOfTheirSerialNumber()
        {
            const string unShuffledSentence =
                "Real GDP VBrose TM1this time TM2last year BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var timerUnitStrategy = new TimerUnitStrategy();

            //  act
            var shufflerParagraph =
                timerUnitStrategy.ShuffleTimerUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "Real GDP VBrose TM2last year TM1this time BKP."));
        }

        [TestCase(
            "Real GDP VBrose TM2last year TM1this time BKP.",
            "Real GDP TM2last year TM1this time VBrose BKP.")]
        [TestCase(
            "He VBAwas PRESleaving TM2last year TM1this time BKP.",
            "He TM2last year TM1this time VBAwas PRESleaving BKP.")]
        [TestCase(
            "He PASTleft TM2last year TM1this time BKP.",
            "He TM2last year TM1this time PASTleft BKP.")]
        public void When_VB_VBA_PAST_Found_Move_TimerUnit_InFront(
            string unShuffledSentence, string output)
        {
            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var timerUnitStrategy = new TimerUnitStrategy();

            //  act
            var shufflerParagraph =
                timerUnitStrategy.ShuffleTimerUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                output));
        }

        [Test]
        public void When_DG_Found_Move_TimerUnit_InFront()
        {
            const string unShuffledSentence = "DG100 TMper month.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var timerUnitStrategy = new TimerUnitStrategy();

            //  act
            var shufflerParagraph =
                timerUnitStrategy.ShuffleTimerUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "TMper month DG100."));
        }
    }
}
