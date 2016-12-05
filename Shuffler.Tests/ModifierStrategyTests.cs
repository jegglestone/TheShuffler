using NUnit.Framework;

namespace Shuffler.Tests
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using Main;

    [TestFixture]
    public class ModifierStrategyTests
    {
        [Test]
        public void WhenNoModifier_Returns_Input()
        {
            const string unShuffledSentence =
                "TM1this time TM2last year BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var modifierUnitStrategy = new ModifierStrategy();

            //  act
            var shufflerParagraph =
                modifierUnitStrategy.ShuffleSentenceUnit(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "TM1this time TM2last year BKP."));
        }

        [Test]
        public void ShufflesModifiersInDescendingOrderOfSerialNumber()
        {
            const string unShuffledSentence =
                "They VBbombed PRENthe NNhouse MD1on PRENthe NNcorner MD2of NNRiver NNStreet MD3in PRENthe NNcity NNcentre BKP.";

              Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var modifierUnitStrategy = new ModifierStrategy();

            //  act
            var shuffledParagraph =
                modifierUnitStrategy.ShuffleSentenceUnit(paragraph);

            Assert.That(shuffledParagraph.InnerText, 
                Is.EqualTo(
                    "They VBbombed PRENthe NNhouse MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner BKP."));
        }
    }
}
