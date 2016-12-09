using NUnit.Framework;

namespace Shuffler.Tests
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using Main;
    using System;
    using System.Linq;

    [TestFixture]
    public class ModifierStrategyTests
    {
        [Test]
        public void WhenNoModifier_Returns_Input()
        {
            AssertReturnIsEqualToExpected(
                "TM1this time TM2last year BKP.",
                "TM1this time TM2last year BKP.");
        }

        [TestCase(
            "MD1on PRENthe NNcorner MD2of NNRiver NNStreet MD3in PRENthe NNcity NNcentre BKP.",
            "MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner BKP.")]
        public void ShufflesModifiersInDescendingOrderOfSerialNumber(
            string input, string expected)
        {
            AssertReturnIsEqualToExpected(
                input, expected);
        }

        [TestCase(
            "MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner BKP.",
            "MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner BKP.")]
        public void WhenModifiersInDescendingOrderOfSerialNumberDoesntReShuffle(
            string input, string expected)
        {
            AssertReturnIsEqualToExpected(
                input,expected);
        }

        [TestCase("They VBbombed PRENthe NNhouse MD1on PRENthe NNcorner MD2of NNRiver NNStreet MD3in PRENthe NNcity NNcentre BKP.")]
        [TestCase("MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner BKP.")]
        public void Underlines_FromFirstToLastModifierUnit_IncludingAllUnitsInBetween(
            string unShuffledSentence)
        {
            Paragraph paragraph =
                DocumentContentHelper
                    .GetParagraphFromWordDocument(unShuffledSentence);

            var modifierStrategy = new ModifierStrategy();

            // act
            var shufflerParagraph =
                modifierStrategy.ShuffleSentenceUnit(paragraph);

            // assert
            Text[] sentenceArray = shufflerParagraph.Descendants<Text>().ToArray();

            int firstModifierPosition = Array.FindIndex(
                sentenceArray, text => text.InnerText.StartsWith("MD"));

            int lastModifierPosition = Array.FindLastIndex(
                sentenceArray, text => text.InnerText == "MD") + 1;

            for (int index = 0; index < sentenceArray.Length; index++)
            {
                Text t = sentenceArray[index];

                if (index >= firstModifierPosition && index <= lastModifierPosition)
                    Assert.That(t.Parent.InnerXml.Contains("<w:u w:val=\"single\" />"));
            }
        }

        [TestCase(
           "They VBbombed PRENthe NNhouse MD1on PRENthe NNcorner MD2of NNRiver NNStreet MD3in PRENthe NNcity NNcentre BKP.",
           "They VBbombed MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner PRENthe NNhouse BKP.")]  
        public void ShufflesMovesModifiersUnitsBeforePREN(
            string input, string expected)
        {
            AssertReturnIsEqualToExpected(
                input, expected);
        }

        [TestCase(
            "They VBbombed MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner PRENthe NNhouse BKP.",
            "They VBbombed NNcity NNcentre NNRiver NNStreet NNcorner of NNhouse BKP.")]
        public void DoSomething(string input, string expected)
        {
            AssertReturnIsEqualToExpected(
                input, expected);
        }

        private static void AssertReturnIsEqualToExpected(string input, string expected)
        {
            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(input);

            var modifierUnitStrategy = new ModifierStrategy();

            //  act
            var shuffledParagraph =
                modifierUnitStrategy.ShuffleSentenceUnit(paragraph);

            Assert.That(shuffledParagraph.InnerText,
                Is.EqualTo(
                    expected));
        }
    }
}
