namespace Shuffler.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Main;
    using Main.Extensions;
    using Main.Helper;
    using Main.Services;
    using NUnit.Framework;

    [TestFixture]
    public class DocumentFormatterTests
    {
        [Test]
        public void WhenGivenOpenXmlDocumentWithClauser_AndMultipleSentences_TheParagraphIsShuffled()
        {
            //NNEconomic growth VBhas continued ADV1at a moderate rate TM1so far TM2this year BKP.
            //ADJReal NNgross domestic productNN(GDP)PASTrose ADV1at an annual rate of 
            //PREN1about DG2 NNpercent MD1in PREN2the TM1first quarter CSafter PRESincreasing 
            //MD2at PREN3a DG3 NNpercent NNpace MD3in PREN4the TM2fourth quarter MD4of TMY2011 BKP.
            //NNGrowth TM1last quarter VBwas supportedBKby ADJfurther NNgains MD1in ADJprivate ADJdomestic NNdemand BKP, 
            // BKwhich BKmore than VBoffset NNa drag MD1from PREN1a NNdecline MD2in NNgovernment NNspending BKP.
            string documentName = "Adverbs and Clausers";
            var elements = GetDocument(documentName);

            // assert
            Assert.That(elements != null, "Xml Document came back null");
            foreach (var element in
                elements.Where(element => element.LocalName == "p"))
            {
                if (!element.InnerText.Contains("CS")) continue;
                Assert.That(element.Descendants<Text>().First().InnerText == "CS");
                Assert.That(element.InnerText.Equals(
                        "CSafter PRESincreasing MD2at PREN3a TM2fourth quarter MD4of TMY2011 BKP, ADJReal NNgross domestic productNN(GDP) PASTrose ADV1at an annual rate of PREN1about DG2 NNpercent MD1in PREN2the TM1first quarter DG3 BKP."));
            }
        }

        [Test]
        public void WhenGivenOpenXmlDocumentWithTimerAndPrens_AndMultipleSentences_TheParagraphIsShuffled()
        {
            /*
             This VB1is PREN1a NNbook PREN2about NNwarBKP. 
             Real GDP VBrose TM2last year TM1this time BKP. 
             He VBAwas PRESleaving TM2last year TM1this time BKP. 
             He PASTleft TM2last year TM1this time BKP.
             */
            string documentName = "Timers And Prens";
            var elements = GetDocument(documentName);

            // assert
            Assert.That(elements != null, "Xml Document came back null");

            OpenXmlElement[] elementsArray = 
                elements.Where(element => element.LocalName == "p").ToArray();

            Assert.That(
                elementsArray[0].InnerText == "This VB1is PREN2about war PREN1a bookBKP.");
            Assert.That(
                elementsArray[1].InnerText == "Real GDP TM2last year TM1this time VBrose BKP.");
            Assert.That(
                elementsArray[2].InnerText == "He TM2last year TM1this time VBAwas PRESleaving BKP.");
            Assert.That(
                elementsArray[3].InnerText == "He TM2last year TM1this time PASTleft BKP.");
        }

        [Test]
        public void
            WhenGivenOpenXmlDocumentWithAdversTimerPrensModifiersAndClausers_AndMultipleSentences_TheParagraphIsShuffled()
        {
            /*
             NNEconomic growth VBhas continued ADV1at a moderate rate TM1so far TM2this year BKP. 
             ADJReal NNgross domestic product PASTrose ADV1at an annual rate of PREN1about DG2 NNpercent MD1in PREN2the TM1first 
             quarter CSafter PRESincreasing MD2at PREN3a DG3 NNpercent NNpace MD3in PREN4the TM2fourth quarter MD4of TMY2011 BKP. 
             NNGrowth TM1last quarter VBwas supportedBKby ADJfurther NNgains MD1in ADJprivate ADJdomestic NNdemand BKP, BKwhich 
             BKmore than VBoffset NNa drag MD1from PREN1a NNdecline MD2in NNgovernment NNspending BKP.
             */
            string documentName = "Adverbs, Timers, Prens, Modifiers and Clausers.";
            var elements = GetDocument(documentName);

            // assert
            Assert.That(elements != null, "Xml Document came back null");

            Assert.That(
                elements[0].InnerText.CountTimesThisStringAppearsInThatString("BKP"), 
                Is.EqualTo(5));

            Assert.That(elements[0].InnerText, Is.EqualTo(
                "NNEconomic growth TM2this year TM1so far VBhas continued ADV1at a moderate rate BKP.CSafter PRESincreasing TM2fourth quarter TMY2011 BKP, ADJReal NNgross domestic product PASTrose ADV1at an annual rate of DG2 NNpercent TM1first quarter DG3OfBKP.NNGrowth TM1last quarter VBwas supportedBKby ADJfurther NNgains NNgovernment NNspending NNdecline ADJprivate ADJdomestic NNdemand BKP, BKwhich BKmore than VBoffset NNa drag OfBKP."));
        }

        [TestCase(
            "They VBbombed MD3in PRENthe NNcity NNcentre MD2of NNRiver NNStreet MD1on PRENthe NNcorner PRENthe NNhouse BKP.",
            "They VBbombed NNcity NNcentre NNRiver NNStreet NNcorner NNhouse of BKP.")]
        //  "They VBbombed NNcity NNcentreNNRiver NNStreet NNcorner NNhouse Of BKP." --bug with missing space
        [TestCase(
            "They VBbombed PRENthe NNhouse MD1on PRENthe NNcorner MD2of NNRiver NNStreet MD3in PRENthe NNcity NNcentre BKP.",
            "They VBbombed NNcity NNcentre NNRiver NNStreet NNcorner OfPRENthe house BKP.")]
        public void SentenceWorksCorrectlyWithMultipleRules(string input, string expectedOutput)
        {
            var elements = GetDocument(input);

            // assert
            Assert.That(elements != null, "Xml Document came back null");

            var element = elements.First(e => e.LocalName == "p");

            Assert.That(element.InnerText, Is.EqualTo(expectedOutput));
        }

        private static List<OpenXmlElement> GetDocument(string documentName)
        {
            List<OpenXmlElement> elements;
            using (WordprocessingDocument document =
                DocumentContentHelper.GetMainDocumentPart(documentName))
            {
                // arrange
                var documentFormatter = new DocumentFormatter(
                    new ClauserUnitStrategy(new ClauserUnitChecker()),
                    new AdverbUnitStrategy(),
                    new TimerUnitStrategy(),
                    new ModifierStrategy(new ModifierFormatter()),
                    new PrenStrategy());

                var docPart = document.MainDocumentPart;
                if (docPart?.Document == null)
                    throw new Exception();

                // act
                elements = documentFormatter.ProcessDocument(docPart);
            }
            return elements;
        }
    }
}
