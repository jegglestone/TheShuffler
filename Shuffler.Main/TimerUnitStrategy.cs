namespace Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Interfaces;
    using Model;

    public class TimerUnitStrategy : IShuffleStrategy
    {
        private Sentence _sentence;

        public Paragraph ShuffleSentenceUnit(Paragraph xmlSentenceElement)
        {
            _sentence = new Sentence(xmlSentenceElement);
            Text[] sentenceArray = _sentence.SentenceArray;

            if (_sentence.UnitNotFoundInSentence(
                sentenceArray, 
                element => element.InnerText.IsTimer()))
                return xmlSentenceElement;

            _sentence.TimerUnitCount = 0;
            var timerUnits = GetTimerUnits(
                sentenceArray).ToArray<IMoveableUnit>();

            _sentence.UnderlineJoinedSentenceUnit(
                sentenceArray, 
                timerUnits[0].StartPosition, 
                timerUnits[_sentence.TimerUnitCount - 1].EndPosition);

            Text[] timerUnitsInSerialNumberOrder =
                _sentence.GetMoveableUnitsInSerialNumberDescendingOrder(
                    _sentence.TimerUnitCount, timerUnits, sentenceArray);

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
                .Take(timerUnitStartPosition-vbVbaPastUnitPosition).ToArray();

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

        private IEnumerable<MoveableUnit> GetTimerUnits(
            IList<Text> sentenceArray)
        {
            var timerUnits =
                new MoveableUnit[sentenceArray.Count(
                    x => x.InnerText.IsTimer())];

            int timerUnitCount = 0;

            _sentence.PopulateMoveableUnits(
                sentenceArray, ref timerUnitCount, timerUnits);

            _sentence.TimerUnitCount = timerUnitCount;
            return timerUnits;
        }
    }
}
