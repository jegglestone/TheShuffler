namespace Main.Interfaces
{
    using DocumentFormat.OpenXml.Wordprocessing;

    public interface ITimerUnitStrategy
    {
        Paragraph ShuffleTimerUnits(Paragraph xmlSentenceElement);
    }
}