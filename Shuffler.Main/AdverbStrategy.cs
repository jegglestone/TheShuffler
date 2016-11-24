namespace Main
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
    using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
    using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

    public class AdverbStrategy : IAdverbStrategy
    {
        public Paragraph ShuffleAdverbUnits(Paragraph xmlSentenceElement)
        {
            //  Search for ADV
            Text[] sentenceArray = xmlSentenceElement.Descendants<Text>().ToArray();

            if (NoAdverbFoundInSentence(sentenceArray))
                return xmlSentenceElement;

            // If an ADV is found, continue to search for the next ADV until reaching any of VB / PAST / PRES / Full - Stop.
            int AdverbIndexPosition =
                Array.FindIndex(sentenceArray, i => i.IsAdverb());

            //get number of adverbs between first adverb and next breaker
            int AdverbCount = 0;
            int breakerPosition = 0;
            for (int i = AdverbIndexPosition; i >= AdverbIndexPosition; i++)
            {
                if (sentenceArray[i].IsAdverb())
                {
                    AdverbCount = AdverbCount + 1;
                }

                if (sentenceArray[i].ReachedSentenceBreaker())
                {
                    breakerPosition = i;
                    break;
                }
            }

            if (IsMoreThanOneAdverb(AdverbCount))
            {
                // underline from ADVIndexPosition to breakerPosition
                for (int i = AdverbIndexPosition; i < breakerPosition; i++)
                {
                    UnderlineWordRun(GetParentRunProperties(sentenceArray, i));
                }
            }


            // If a second or even a third ADV is found, underline from the first ADV to and including the last ADV together with all words and punctuations in between.
            //This in effect joins up the individual ADV units to form a large ADV unit -

            return xmlSentenceElement;
        }

        private static bool NoAdverbFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.Replace(" ", "") == TagMarks.AdverbTag);
        }

        private static bool IsMoreThanOneAdverb(int ADVCount)
        {
            return ADVCount > 1;
        }

        private static void UnderlineWordRun(RunProperties runProperties)
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

        private static RunProperties GetParentRunProperties(Text[] sentenceArray, int i)
        {
            return sentenceArray[i].Parent.Descendants<RunProperties>().First();
        }
    }

    public static class OpenXmlExtensions
    {
        public static bool IsAdverb(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.Replace(" ", "") == TagMarks.AdverbTag;
        }

        public static bool ReachedSentenceBreaker(this OpenXmlLeafElement openXmlLeafElement)
        {
            return openXmlLeafElement.InnerText.Replace(" ", "") == "VB"         // tests needed for each of these
                   || openXmlLeafElement.InnerText.Replace(" ", "") == "PAST"
                   || openXmlLeafElement.InnerText.Replace(" ", "") == "PRES"
                   || openXmlLeafElement.InnerText.Replace(" ", "") == "BKP";
        }
    }

    public class TagMarks
    {
        public const string AdverbTag = "ADV";
        public const string ClauserTag = "CS";
    }
}
