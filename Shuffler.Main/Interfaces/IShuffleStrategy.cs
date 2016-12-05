namespace Main.Interfaces
{
    using DocumentFormat.OpenXml.Wordprocessing;

    public interface IShuffleStrategy
    {
        Paragraph ShuffleSentenceUnit(Paragraph xmlSentenceElement);
    }
}
