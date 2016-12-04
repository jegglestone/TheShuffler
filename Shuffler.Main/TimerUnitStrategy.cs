namespace Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Model;

    public class TimerUnitStrategy
    {
        private Sentence _sentence;

        public Paragraph ShuffleTimerUnits(Paragraph xmlSentenceElement)
        {
            _sentence = new Sentence(xmlSentenceElement);
            Text[] sentenceArray = _sentence.SentenceArray;

            if (NoTimerFoundInSentence(sentenceArray))
                return xmlSentenceElement;

            int timerUnitCount;
            var timerUnits = GetTimerUnits(
                sentenceArray, out timerUnitCount).ToArray<IMoveableUnit>();

            _sentence.UnderlineJoinedSentenceUnit(
                sentenceArray, 
                timerUnits[0].StartPosition, 
                timerUnits[timerUnitCount - 1].EndPosition);

            Text[] timerUnitsInSerialNumberOrder =
                _sentence.GetMoveableUnitsInSerialNumberDescendingOrder(
                    timerUnitCount, timerUnits, sentenceArray);

            Text[] newSentence;

            if (_sentence.HasVbVbaPastToTheLeft(
                sentenceArray, timerUnits[0].StartPosition))
            {
                newSentence = MoveTimerUnitBeforeVbVbaPast(
                    sentenceArray, 
                    timerUnitsInSerialNumberOrder, 
                    timerUnits[0].StartPosition);
            }
            else if (_sentence.HasDGToTheLeft(
                sentenceArray, timerUnits[0].StartPosition))
            {
                int dGIndexPosition = 
                    Array.FindIndex(sentenceArray, i => i.IsDG());
 
                newSentence = MoveTimerUnitBeforeDGUnit(
                    sentenceArray, 
                    dGIndexPosition,
                    timerUnitsInSerialNumberOrder);
            }
            else
            {
                var beforeTimer = 
                    sentenceArray.Take(timerUnits[0].StartPosition);

                newSentence = 
                    beforeTimer.Concat(timerUnitsInSerialNumberOrder).ToArray();
            }
            
            
            newSentence = RemoveAnyBlankSpaceFromEndOfUnit
                (newSentence
                    .Concat(
                        _sentence.GetSentenceBreaker(sentenceArray)).ToArray());

            return new Paragraph(
                OpenXmlHelper.BuildWordsIntoOpenXmlElement(
                    newSentence));
        }

        private Text[] MoveTimerUnitBeforeVbVbaPast( 
            Text[] sentenceArray, 
            Text[] reversedTimerUnits, 
            int timerUnitStartPosition)
        {
            int vbVbaPastUnitPosition
                = _sentence.GetPositionOfClosestSpecifiedUnit(
                    sentenceArray, 
                    timerUnitStartPosition, 
                    t => t.IsVbVbaPast());

            Text[] vbVbaPastTagAndUnit =
                sentenceArray
                .Skip(vbVbaPastUnitPosition)
                .Take(2).ToArray();

            // if after vbVbaPastUnitPosition+2 is not  (" ", "BKP", ".")
            // add the rest of the sentence up to BKP to vbVbaPastTagAndUnit

            Text[] beforeVbVbaPastUnit =
                sentenceArray
                .Take(vbVbaPastUnitPosition).ToArray();

            return
                beforeVbVbaPastUnit
                    .Concat(reversedTimerUnits)
                    .Concat(vbVbaPastTagAndUnit).ToArray();
        }

        private Text[] MoveTimerUnitBeforeDGUnit(
            Text[] sentenceArray, 
            int dGIndexPosition, 
            IEnumerable<Text> reversedTimerUnits)
        {
            Text[] dGUnitAndTag =
                sentenceArray
                .Skip(dGIndexPosition)
                .Take(2).ToArray();

            Text[] beforeDG =
                sentenceArray
                .Take(dGIndexPosition).ToArray();

            return 
                beforeDG
                    .Concat(reversedTimerUnits)
                    .Concat(dGUnitAndTag).ToArray();
        }

        private static Text[] RemoveAnyBlankSpaceFromEndOfUnit(Text[] newSentence)
        {
            int positionOfTextBeforeBreakerUnit = newSentence.Length - 3;
            int positionOfTextTwoPlacesBeforeBreakerUnit = newSentence.Length - 4;

            string textBeforeBreakerUnit = newSentence[positionOfTextBeforeBreakerUnit].Text;
            string textTwoPlacesBeforeBreakerUnit = newSentence[positionOfTextTwoPlacesBeforeBreakerUnit].Text;

            if (string.IsNullOrWhiteSpace(textBeforeBreakerUnit))
            {
                if (string.IsNullOrWhiteSpace(textTwoPlacesBeforeBreakerUnit) 
                    || textTwoPlacesBeforeBreakerUnit.EndsWith(" "))
                {
                    newSentence = newSentence.RemoveAt(positionOfTextBeforeBreakerUnit);
                }
            }

            return newSentence;
        }

        private static IEnumerable<TimerUnit> GetTimerUnits(IList<Text> sentenceArray, out int timerUnitCount)
        {
            TimerUnit[] timerUnits =
                new TimerUnit[sentenceArray.Count(x => x.InnerText.IsTimer())];

            timerUnitCount = 0;

            for (int index = 0; index < sentenceArray.Count; index++)
            {
                if (sentenceArray[index].IsTimer())
                {
                    timerUnits[timerUnitCount] = new TimerUnit
                    {
                        StartPosition = index
                    };

                    timerUnitCount++;

                    if (timerUnitCount <= 1) continue;

                    timerUnits[timerUnitCount - 2].EndPosition = index;
                }
                else if (sentenceArray[index].InnerText.IsBreakerPunctuation())
                {
                    timerUnits[timerUnitCount - 1].EndPosition = index;
                    break;
                }
            }
            return timerUnits;
        }

        private static bool NoTimerFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsTimer());
        }
    }
}
