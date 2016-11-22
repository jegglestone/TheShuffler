namespace Main
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Drawing;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
    using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
    using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
    using Underline = DocumentFormat.OpenXml.Wordprocessing.Underline;

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

            //get number of adverbs between first adverb and next breaker
            int ADVCount = 0;
            int breakerPosition = 0;
            for (int i = ADVIndexPosition; i >= ADVIndexPosition; i++)
            {
              
                if (sentenceArray[i].InnerText.Replace(" ", "") == "ADV")
                {
                    ADVCount = ADVCount + 1;
                    // underline
                }

                // once we are at the second ADV 

                if (sentenceArray[i].InnerText.Replace(" ", "") == "VB"         // tests needed for each of these
                        || sentenceArray[i].InnerText.Replace(" ", "") == "PAST"
                        || sentenceArray[i].InnerText.Replace(" ", "") == "PRES"
                        || sentenceArray[i].InnerText.Replace(" ", "") == "BKP")
                {
                    breakerPosition = i;
                    break;
                }
            }

            if (ADVCount > 1)
            {
                // underline from ADVIndexPosition to breakerPosition
                for (int i = ADVIndexPosition; i < breakerPosition; i++)
                {
                    var runProperties =  sentenceArray[i].Parent.Descendants<RunProperties>();
                    
                    var underlineElement = runProperties.Select(x => x.Underline).FirstOrDefault();
                    if (underlineElement != null)
                        underlineElement.Val = "Single";
                    else
                    {
                        // add an underline element
                    }
                    //vertAlign.Val = new EnumValue<VerticalPositionValues>(VerticalPositionValues.Superscript);
                }
            }


            // If a second or even a third ADV is found, underline from the first ADV to and including the last ADV together with all words and punctuations in between.
            //This in effect joins up the individual ADV units to form a large ADV unit -

            return xmlSentenceElement;
        }
    }
}
