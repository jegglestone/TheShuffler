namespace Main.Services
{
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

        public void ProcessDocument(MainDocumentPart docPart)
        {
            OpenXmlElement documentBodyXml = docPart.Document.Body;

            foreach (var element in documentBodyXml.Elements())
            {
                if (element.LocalName == "p")
                {
                    _clauserUnitStrategy.ShuffleClauserUnits(element as Paragraph);
                    _adverbStrategy.ShuffleAdverbUnits(element as Paragraph);
                }
            }
        }
    }
}
