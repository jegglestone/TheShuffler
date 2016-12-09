namespace Main.Helper
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;

    public class OpenXmlTextHelper
    {
        public static Text[] RemoveUnitFromOriginalPosition(
            Text[] unit, Predicate<Text> p)
        {
            return unit.Take(
                Array.FindIndex(unit, p)).ToArray();
        }

        public static void UnderlineWordRun(RunProperties runProperties)
        {
            if (runProperties == null)
                return;

            var underlineElement = runProperties.Underline;
            if (underlineElement != null)
                underlineElement.Val = new EnumValue<UnderlineValues>(UnderlineValues.Single);
            else
            {
                // add/append an underline element
                runProperties.Append(
                    new OpenXmlElement[]
                    {
                        new Underline() { Val = new EnumValue<UnderlineValues>(UnderlineValues.Single)}
                    });
            }
        }

        public static RunProperties GetParentRunProperties(Text[] sentenceArray, int i)
        {
            try
            {
                return sentenceArray[i].Parent.Descendants<RunProperties>().First();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
