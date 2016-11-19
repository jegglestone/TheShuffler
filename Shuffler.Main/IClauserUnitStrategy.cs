namespace CSWordRemoveBlankPage
{
    using DocumentFormat.OpenXml.Wordprocessing;

    public interface IClauserUnitStrategy
    {
        Paragraph ShuffleClauserUnits(Paragraph xmlSentenceElement);
    }
}
