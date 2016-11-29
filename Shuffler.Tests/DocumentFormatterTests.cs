namespace Shuffler.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Helper;
    using Main;
    using Main.Services;
    using NUnit.Framework;

    [TestFixture]
    public class DocumentFormatterTests
    {
        [Test]
        public void WhenGivenOpenXmlDocumentWithClauser_AndMultipleSentences_TheClauserParagraphIsShuffled()
        {
            //NNEconomic growth VBhas continued ADV1at a moderate rate TM1so far TM2this year BKP.
            //ADJReal NNgross domestic productNN(GDP)PASTrose ADV1at an annual rate of 
            //PREN1about DG2 NNpercent MD1in PREN2the TM1first quarter CSafter PRESincreasing 
            //MD2at PREN3a DG3 NNpercent NNpace MD3in PREN4the TM2fourth quarter MD4of TMY2011 BKP.
            //NNGrowth TM1last quarter VBwas supportedBKby ADJfurther NNgains MD1in ADJprivate ADJdomestic NNdemand BKP, 
            // BKwhich BKmore than VBoffset NNa drag MD1from PREN1a NNdecline MD2in NNgovernment NNspending BKP.

            List<OpenXmlElement> elements;
            using (WordprocessingDocument document =
                DocumentContentHelper.GetMainDocumentPart("Adverbs and Clausers"))
            {
                // arrange
                var documentFormatter = new DocumentFormatter(
                new ClauserUnitStrategy(new ClauserUnitChecker()),
                new AdverbUnitStrategy());

                var docPart = document.MainDocumentPart;
                if (docPart?.Document == null)
                    throw new Exception();

                // act
                elements = documentFormatter.ProcessDocument(docPart);                
            }

            // assert
            Assert.That(elements != null, "Xml Document came back null");
            foreach (var element in
                elements.Where(element => element.LocalName == "p"))
            {
                if (!element.InnerText.Contains("CS")) continue;
                Assert.That(element.Descendants<Text>().First().InnerText == "CS");
                Assert.That(element.InnerText.Equals(
                        "CSafter PRESincreasing MD2at PREN3a DG3 NNpercent NNpace MD3in PREN4the TM2fourth quarter MD4of TMY2011 BKP, ADJReal NNgross domestic productNN(GDP) PASTrose ADV1at an annual rate of PREN1about DG2 NNpercent MD1in PREN2the TM1first quarter BKP."));
            }
        }
    }
}
