namespace Main
{
    using System;
    using System.Linq;
    using Extensions;
    using Helper;
    using Interfaces;
    using Model;
    using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
    using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

    public class AdverbUnitStrategy : IAdverbStrategy
    {
        private Sentence _sentence;

        public Paragraph ShuffleAdverbUnits(Paragraph xmlSentenceElement)
        {
            _sentence = new Sentence(xmlSentenceElement); // move to a constructor later!

            var sentenceArray = _sentence.SentenceArray;
           
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
                _sentence.UnderlineJoinedSentenceUnit(
                    sentenceArray, AdverbIndexPosition, breakerPosition);

                xmlSentenceElement = _sentence.HasVBAToTheLeft(sentenceArray, AdverbIndexPosition)
                    ? MoveAdverbUnitBetweenTheVBAAndPASTPRES(sentenceArray, AdverbIndexPosition) 
                    : MoveAdverbUnitBeforeVBOrPASTorPRESUnit(sentenceArray, AdverbIndexPosition);
            }
            else if (IsOneAdverb(AdverbCount))
            {
                if (_sentence.NoVBAFoundInSentence(sentenceArray))
                {
                    xmlSentenceElement =
                        MoveSingleAdverbBeforeVBOrPASTorPRESUnit(sentenceArray, AdverbIndexPosition);
                }
            }

            return xmlSentenceElement;
        }

        private Paragraph MoveSingleAdverbBeforeVBOrPASTorPRESUnit(Text[] sentenceArray, int adverbIndexPosition)
        {
            var VbPastPresPosition = 
                _sentence.GetPositionOfClosestSpecifiedUnit(
                    sentenceArray, adverbIndexPosition, text => text.IsVbPastPres());
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
                .Concat(new[] { adverbTag })
                .Concat(new[] { adverb, new Text(" ") })
                .Concat(vbPastPres)
                .Concat(afterADV).ToArray();

            var wordElements = OpenXmlHelper.BuildWordsIntoOpenXmlElement(arr);

            return new Paragraph(wordElements);
        }

        private Paragraph MoveAdverbUnitBeforeVBOrPASTorPRESUnit(
            Text[] sentenceArray, int adverbIndexPosition)
        {
            int vbPastPresPosition = _sentence.GetPositionOfClosestSpecifiedUnit(
                sentenceArray, adverbIndexPosition, text => text.IsVbPastPres());

            Text[] adverbUnit = GetAdverbUnit(
                sentenceArray, adverbIndexPosition);

            Text[] PastPresUnitAndTag =
                sentenceArray
                .Skip(vbPastPresPosition - 1)
                .Take(vbPastPresPosition + 1).ToArray();

            Text[] beforePastPresent = 
                sentenceArray
                .Take(vbPastPresPosition-1).ToArray();

            Text[] breaker =
                _sentence.GetSentenceBreaker(sentenceArray);

            Text[] shuffledSentence = 
                beforePastPresent
                .Concat(adverbUnit)
                .Concat(PastPresUnitAndTag)
                .Concat(breaker).ToArray();
            
            return new Paragraph(
                OpenXmlHelper.BuildWordsIntoOpenXmlElement(shuffledSentence));
        }

        private Paragraph MoveAdverbUnitBetweenTheVBAAndPASTPRES(Text[] sentenceArray, int adverbIndexPosition)
        {
            //get VBA pos
            Text[] beforeAdverb;
            var VBAPosition = _sentence.GetVBAPosition(out beforeAdverb, sentenceArray, adverbIndexPosition);
            Text[] adverbUnit = GetAdverbUnit(sentenceArray, adverbIndexPosition);

            int pastPresPosition =
                _sentence.GetFirstPastPresPositionAfterVBA(beforeAdverb, VBAPosition);

            Text[] sentenceToVBA = sentenceArray.Take(VBAPosition + 2).ToArray();
            Text[] pastPresUnit = sentenceArray.Skip(pastPresPosition - 1).Take(pastPresPosition + 1).ToArray();
            Text[] breaker = sentenceArray.Skip(sentenceArray.Length - 3).ToArray();
            var shuffledSentence = sentenceToVBA.Concat(adverbUnit).Concat(pastPresUnit).Concat(breaker).ToArray();

            return new Paragraph(OpenXmlHelper.BuildWordsIntoOpenXmlElement(shuffledSentence));
        }

        private static Text[] GetAdverbUnit(Text[] sentenceArray, int adverbIndexPosition)
        {
            Text[] adverbUnit = sentenceArray.Skip(adverbIndexPosition - 1).ToArray();
            adverbUnit = adverbUnit.Take(adverbUnit.Length - 3).ToArray(); // remove full stop
            return adverbUnit;
        }

        private static bool NoAdverbFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsAdverb());
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
