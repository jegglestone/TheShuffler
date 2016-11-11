namespace Shuffler.Helper
{
    using System;
    using System.Windows.Forms;
    using Helper;
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
                if (ParagraphHasText(paragraph)) continue;

                paragraph.Range.Select();
                wordapp.Selection.Delete();
            }
            return paragraphs;
        }

        private static bool ParagraphHasText(Paragraph paragraph)
        {
            return paragraph.Range.Text.Trim() != string.Empty;
        }

        public bool ShuffleClauserUnits(Range sentenceRange)
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
