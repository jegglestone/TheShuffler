namespace Main
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class AdverbStrategy : IAdverbStrategy
    {
        private const string _ADVTag = "ADV";

        public Paragraph ShuffleAdverbUnits(Paragraph xmlSentenceElement)
        {
            //  Search for ADV
            Text[] sentenceArray = xmlSentenceElement.Descendants<Text>().ToArray();

            if (!Array.Exists(
                sentenceArray, element => element.InnerText.Replace(" ", "") == _ADVTag))
                return xmlSentenceElement;

            // If an ADV is found, continue to search for the next ADV until reaching any of VB / PAST / PRES / Full - Stop.
            int ADVIndexPosition =
                Array.FindIndex(sentenceArray, i => i.InnerText == _ADVTag);

            var nextBreakerPosition =
                Array.FindIndex(sentenceArray, i =>    // need test cases for all three of these
                        i.InnerText.Replace(" ", "") == "VB"
                        || i.InnerText.Replace(" ", "") == "PAST"
                        || i.InnerText.Replace(" ", "") == "PRES"
                        || i.InnerText.Replace(" ", "") == "BKP");

            // how many ADV elements between ADVIndexPosition and nextBreakerPosition?
            int ADVCount = 0;
            for (int i = ADVIndexPosition; i < nextBreakerPosition; i++)
            {
                if (sentenceArray[i].InnerText.Replace(" ", "") == "ADV")
                {
                    ADVCount = ADVCount + 1;
                }
            }

            //if (ADVCount > 1)
            //{
            //    foreach (int i = ADVIndexPosition; i < nextBreakerPosition; i++)
            //    {
            //        sentenceArray[i].Parent.Elements("underline") = "superscript";
            //    }
            //}


            // If a second or even a third ADV is found, underline from the first ADV to and including the last ADV together with all words and punctuations in between.
            //This in effect joins up the individual ADV units to form a large ADV unit -

            return xmlSentenceElement;
        }
    }
}
