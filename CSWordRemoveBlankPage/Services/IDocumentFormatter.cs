using Microsoft.Office.Interop.Word;

namespace CSWordRemoveBlankPage
{
    public interface IDocumentFormatter
    {
        bool ShuffleClauserUnits(Range sentenceRange);
    }
}
