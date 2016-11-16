namespace Shuffler.Helper
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using System;
    using System.Xml.Linq;

    public class DocumentFormatter
    {
        private IUnitChecker _clauserUnitChecker;

        public DocumentFormatter(IUnitChecker clauserUnitChecker)
        {
            _clauserUnitChecker = clauserUnitChecker;
        }

        public bool ProcessDocument(MainDocumentPart docPart)
        {
            OpenXmlElement documentBodyXml = docPart.Document.Body;

            foreach (var element in documentBodyXml.Elements())
            {
                if (element.LocalName == "Paragraph")
                {
                    ShuffleClauserUnits(element);
                }
            }

            return false;
        }

        private bool ShuffleClauserUnits(OpenXmlElement xmlSentenceElement)
        {
            throw new NotImplementedException();

            // locate a CS 

            // use the ClauserUnitChecker to make sure it's superscripted

            // get the ending point (bkp)

            // add a comma if there isn't one

            // move to the beginning of the sentence

        }

    }
}
