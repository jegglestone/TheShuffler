namespace Main.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class DocumentFormatter
    {
        private readonly IClauserUnitStrategy _clauserUnitStrategy;
        private readonly IAdverbStrategy _adverbStrategy;

        public DocumentFormatter(
            IClauserUnitStrategy clauserUnitStrategy
            , AdverbStrategy adverbStrategy)
        {
            _clauserUnitStrategy = clauserUnitStrategy;
            _adverbStrategy = adverbStrategy;
        }

        public List<OpenXmlElement> ProcessDocument(MainDocumentPart docPart)
        {
            var shuffledElements = new List<OpenXmlElement>();

            foreach (var element in docPart.Document.Body.Elements().ToList())
            {
                // if a paragraph as more than one full stop
                // split the paragraph into multiple elements
                // pass each of them in
                // then amalagamate them into a single paragraph at the end

                if (element.InnerText.Split("BKP.").Length > 1)
                {
                    
                }



                if (element.LocalName == "p")
                {
                    var shuffledElement = element as Paragraph;
                    shuffledElement = _clauserUnitStrategy.ShuffleClauserUnits(shuffledElement);
                    shuffledElement = _adverbStrategy.ShuffleAdverbUnits(shuffledElement);

                    shuffledElements.Add(shuffledElement);
                }
                else {
                    shuffledElements.Add(element);
                }
            }

            return shuffledElements;
        }
    }
}
