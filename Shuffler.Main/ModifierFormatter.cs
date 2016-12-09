namespace Main
{
    using System;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Interfaces;
    public class ModifierFormatter : IModifierFormatter
    {
        public Text[] ApplyFormattingRules(Text[] modifierUnit)
        {
            //remove md tags
            for (int i = 0; i < modifierUnit.Length; i++)
            {
                if (modifierUnit[i].Text.IsModifier() || modifierUnit[i].Text.IsPren())
                {
                    modifierUnit = modifierUnit.RemoveAt(i);
                    modifierUnit = modifierUnit.RemoveAt(i);
                    i--;
                }
            }
           
            // add of
            Array.Resize(ref modifierUnit, modifierUnit.Length+1);
            modifierUnit[modifierUnit.Length-1]=new Text("Of");

            return modifierUnit;
        }
    }
}
