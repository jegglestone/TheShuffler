namespace Main.Interfaces
{
    using DocumentFormat.OpenXml.Wordprocessing;

    public interface IModifierFormatter
    {
        Text[] ApplyFormattingRules(Text[] modifierUnit);
    }
}
