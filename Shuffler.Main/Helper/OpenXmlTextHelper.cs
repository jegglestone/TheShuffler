namespace Main.Helper
{
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;

    public class OpenXmlTextHelper
    {
        public static Text[] RemoveUnitFromOriginalPosition(Text[] unit)
        {
            return unit.RemoveAt(unit.Length - 1)   //remove the tag unit
                .RemoveAt(unit.Length - 2)          //remove the word itself
                .RemoveAt(unit.Length - 3);         // remove the preceedingspace
        }

        public static void UnderlineWordRun(RunProperties runProperties)
        {
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
            return sentenceArray[i].Parent.Descendants<RunProperties>().First();
        }
    }
}
