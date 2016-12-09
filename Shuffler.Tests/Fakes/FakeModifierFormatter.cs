namespace Shuffler.Tests.Fakes
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using Main.Interfaces;
    public class FakeModifierFormatter : IModifierFormatter
    {
        public Text[] ApplyFormattingRules(Text[] modifierUnit)
        {
            return modifierUnit;
        }
    }
}
