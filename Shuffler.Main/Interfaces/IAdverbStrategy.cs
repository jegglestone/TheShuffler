namespace Main.Interfaces
{
    using DocumentFormat.OpenXml.Wordprocessing;

    public interface IAdverbStrategy
    {
        Paragraph ShuffleAdverbUnits(Paragraph xmlSentenceElement);
    }
}
