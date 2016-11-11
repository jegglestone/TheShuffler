namespace Shuffler.Services
{
    using Microsoft.Office.Interop.Word;

    public interface IDocumentFormatter
    {
        bool ShuffleClauserUnits(Range sentenceRange);
    }
}
