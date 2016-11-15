namespace Shuffler.Services
{
    using System.Xml.Linq;
    using Microsoft.Office.Interop.Word;

    public interface IDocumentFormatter
    {
        bool ShuffleClauserUnits(XElement sentenceRange);
    }
}
