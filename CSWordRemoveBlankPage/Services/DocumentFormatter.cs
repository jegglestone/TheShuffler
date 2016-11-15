namespace Shuffler.Helper
{
    using System;
    using System.Xml.Linq;
    using Microsoft.Office.Interop.Word;
    using Services;
    using Application = Microsoft.Office.Interop.Word.Application;

    public class DocumentFormatter : IDocumentFormatter
    {
        private IUnitChecker _clauserUnitChecker;

        public DocumentFormatter(IUnitChecker clauserUnitChecker)
        {
            _clauserUnitChecker = clauserUnitChecker;
        }

        public static Paragraphs RemoveBlankPages(
            Document doc,
            Application wordapp)
        {
            var paragraphs = doc.Paragraphs;
            foreach (Paragraph paragraph in paragraphs)
            {
                
            }
            return paragraphs;
        }

        public bool ShuffleClauserUnits(XElement xmlSentenceElement)
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
