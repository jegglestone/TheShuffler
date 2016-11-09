namespace CSWordRemoveBlankPage.Services
{
    using System;
    using Microsoft.Office.Interop.Word;

    public class DocumentFormatter : IDocumentFormatter
    {
        public static Paragraphs RemoveBlankPages(
            Document doc,
            Application wordapp)
        {
            Paragraphs paragraphs = doc.Paragraphs;
            foreach (Paragraph paragraph in paragraphs)
            {
                if (paragraph.Range.Text.Trim() != string.Empty) continue;

                paragraph.Range.Select();
                wordapp.Selection.Delete();
            }
            return paragraphs;
        }

        public bool ShuffleClauserUnits(Range sentenceRange)
        {
            throw new NotImplementedException();
        }
    }
}
