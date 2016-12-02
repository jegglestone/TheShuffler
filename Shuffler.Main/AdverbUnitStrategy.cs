namespace Main
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;
    using Interfaces;
    using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
    using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
    using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

    public class AdverbUnitStrategy : IAdverbStrategy
    {
        public Paragraph ShuffleAdverbUnits(Paragraph xmlSentenceElement)
        {
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
                    OpenXmlTextHelper.UnderlineWordRun(
                        OpenXmlTextHelper.GetParentRunProperties(sentenceArray, i));
                }
            }
            else if (IsOneAdverb(AdverbCount))
            {
                if (NoVBAFoundInSentence(sentenceArray))
                {
                    xmlSentenceElement = 
                        MoveBeforeVBOrPASTorPRESUnit(sentenceArray, AdverbIndexPosition);
                }
            }

            return xmlSentenceElement;
        }

        private static Paragraph MoveBeforeVBOrPASTorPRESUnit(Text[] sentenceArray, int adverbIndexPosition)
        {
            var VbPastPresPosition = GetClosestVbPastOrPresUnit(sentenceArray, adverbIndexPosition);
            var adverbTag = sentenceArray[adverbIndexPosition];
            var adverb = sentenceArray[adverbIndexPosition + 1];

            Text[] beforeVbPastPres;
            Text[] afterVbPastPres;

            ArrayUtility.SplitArrayAtPosition(
                sentenceArray, VbPastPresPosition, out beforeVbPastPres, out afterVbPastPres);

            Text[] beforeADV;
            Text[] afterADV;

            ArrayUtility.SplitArrayAtPosition(
                afterVbPastPres, adverbIndexPosition, out beforeADV, out afterADV);

            var vbPastPres = 
                OpenXmlTextHelper.RemoveUnitFromOriginalPosition(beforeADV);
          
            var arr = 
                beforeVbPastPres
                .Concat(new[] {adverbTag})
                .Concat(new[] {adverb, new Text(" ") })
                .Concat(vbPastPres)
                .Concat(afterADV).ToArray();

            var wordElements = OpenXmlHelper.BuildWordsIntoOpenXmlElement(arr);

            return new Paragraph(wordElements);
        }

        private static int GetClosestVbPastOrPresUnit(Text[] sentenceArray, int adverbIndexPosition)
        {
            Text[] beforeAdverb;
            Text[] afterAdverb;

            ArrayUtility.SplitArrayAtPosition(
                sentenceArray, adverbIndexPosition, out beforeAdverb, out afterAdverb);

            var VbPastPresPosition = Array.FindLastIndex(
                beforeAdverb, i => i.IsVbPastPres());

            return VbPastPresPosition;
        }

        private static bool NoAdverbFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsAdverb());
        }

        private static bool NoVBAFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsVBA());
        }

        private static bool IsMoreThanOneAdverb(int ADVCount)
        {
            return ADVCount > 1;
        }

        private static bool IsOneAdverb(int ADVCount)
        {
            return ADVCount == 1;
        }
    }
}
