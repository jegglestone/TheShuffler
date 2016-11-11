namespace CSWordRemoveBlankPage.Services
{
    using System;
    using System.Windows.Forms;
    using Microsoft.Office.Interop.Word;
    using Application = Microsoft.Office.Interop.Word.Application;

    public class DocumentFormatter : IDocumentFormatter
    {
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
        }
    }
}
