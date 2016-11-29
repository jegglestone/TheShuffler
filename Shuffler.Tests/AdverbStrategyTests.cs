namespace Shuffler.Tests
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using NUnit.Framework;
    using Main;

    [TestFixture]
    public class AdverbUnitStrategyTests
    {
        [Test]
        public void DontShuffle_AdverbUnits_ThatAreInFrontOfAdjectives()
        {
            const string unShuffledSentence =
                "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.";
            
            const string expectedSentence = 
                "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.";

            AssertThatUnShuffledSentenceReturnExpectedSentence(
                unShuffledSentence, expectedSentence);
        }

        [TestCase("He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.")]
        [TestCase("He VBAis PRESdoing NNit ADVconsistently BKand ADVcarefully BKP.")]
        public void Underlines_FromFirstToLastAdverbUnit_IncludingAllUnitsInBetween(
            string unShuffledSentence)
        {
            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new AdverbUnitStrategy();

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

        [TestCase(
            "He VBAhas PASTplanned NNit ADVpatientlyBK, ADVquitely BKand ADVmeticulously BKP.",
            "He VBAhas ADVpatientlyBK, ADVquitely BKand ADVmeticulously PASTplanned NNit BKP.")]
        [TestCase(
            "He VBAis PRESdoing NNit ADVconsistently BKand ADVcarefully BKP.",
            "He VBAis ADVconsistently BKand ADVcarefully PRESdoing NNit BKP.")]
        public void AdverbUnits_VBA_IsPresentToTheLeft_ADVerbUnit_IsMoved_Between_The_VBA_And_PAST_or_PRES(
            string unShuffledSentence, string expectedSentence)
        {
            AssertThatUnShuffledSentenceReturnExpectedSentence(
                unShuffledSentence, expectedSentence);
        }

        [TestCase(
            "He VBtries ADVhard BKP.",
            "He PASTshouted ADVloudlyBK, ADVemotionally BKand ADVnon-stop BKP.")]
        [TestCase(
            "He ADVhard VBtries BKP.",
            "He ADVloudlyBK, ADVemotionally BKand ADVnon-stop PASTshouted BKP.")]
        public void When_Only_One_ADVerb_And_No_VBA_unit_Move_ADV_To_Before_The_VB_or_PAST_or_PRES(
            string unShuffledSentence, string expectedSentence)
        {
            AssertThatUnShuffledSentenceReturnExpectedSentence(
                unShuffledSentence, expectedSentence);
        }

        private static void AssertThatUnShuffledSentenceReturnExpectedSentence(string unShuffledSentence,
            string expectedSentence)
        {
            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new AdverbUnitStrategy();

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleAdverbUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                expectedSentence));
        }
    }
}
