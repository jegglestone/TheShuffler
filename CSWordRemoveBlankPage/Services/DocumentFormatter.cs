namespace Shuffler.Helper
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using System;
    using System.Xml.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;

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
                if (element.LocalName == "p")
                {
                    //ShuffleClauserUnits(element as Paragraph);
                }
            }

            return false;
        }

        public Paragraph ShuffleClauserUnits(Paragraph xmlSentenceElement)
        {
           throw  new NotImplementedException();

           // loop through all word runs

            int[] clauserPosition;

            // get the position of any valid CS

            // get the position of the next comma

            // get the ending point (bkp)

            // add a comma if there isn't one

            // move the block to the beginning of the sentence

        }

    }
}
