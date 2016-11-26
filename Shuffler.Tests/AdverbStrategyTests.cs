namespace Shuffler.Tests
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using NUnit.Framework;
    using Main;

    [TestFixture]
    public class AdverbStrategyTests
    {
        [Test]
        public void DontShuffle_AdverbUnits_ThatAreInFrontOfAdjectives()
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

        [TestCase("He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.")]
        [TestCase("He VBAis PRESdoing NNit ADVconsistently BKand ADVcarefully BKP.")]
        public void Underlines_FromFirstToLastAdverbUnit_IncludingAllUnitsInBetween(
            string unShuffledSentence)
        {
            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new AdverbStrategy();

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleAdverbUnits(paragraph);

            // assert
            Text[] sentenceArray = shufflerParagraph.Descendants<Text>().ToArray();
           
            int firstAdverbPosition = Array.FindIndex(
                sentenceArray, text => text.InnerText == "ADV");

            for (int index = 0; index < sentenceArray.Length; index++)
            {
                Text t = sentenceArray[index];

                if (index >= firstAdverbPosition)
                    Assert.That(t.Parent.InnerXml.Contains("<w:u w:val=\"single\" />"));
            }
        }        
    }
}
