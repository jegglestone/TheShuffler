namespace Shuffler.Helper
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using CSWordRemoveBlankPage;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class DocumentFormatter
    {
        private readonly IClauserUnitStrategy _clauserUnitStrategy;

        public DocumentFormatter(IClauserUnitStrategy clauserUnitStrategy)
        {
            _clauserUnitStrategy = clauserUnitStrategy;
        }

        public void ProcessDocument(MainDocumentPart docPart)
        {
            OpenXmlElement documentBodyXml = docPart.Document.Body;

            foreach (var element in documentBodyXml.Elements())
            {
                if (element.LocalName == "p")
                {
                    _clauserUnitStrategy.ShuffleClauserUnits(element as Paragraph);
                }
            }
        }
    }
}
