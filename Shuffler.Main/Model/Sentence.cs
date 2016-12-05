namespace Main.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;

    public class Sentence
    {
        private readonly Paragraph _sentenceElement;

        public int TimerUnitCount { get; set; }

        public int ModifierCount { get; set; }

        public Text[] SentenceArray {
            get
            {
               return _sentenceElement.Descendants<Text>().ToArray();
            }
        }

        public Sentence(Paragraph sentenceElement)
        {
            _sentenceElement = sentenceElement;
        }

        public void UnderlineJoinedSentenceUnit(Text[] sentenceArray, int unitStartPosition, int unitEndPosition)
        {
            for (int i = unitStartPosition; i < unitEndPosition; i++)
            {
                OpenXmlTextHelper.UnderlineWordRun(
                    OpenXmlTextHelper.GetParentRunProperties(sentenceArray, i));
            }
        }

        public Text[] GetMoveableUnitsInSerialNumberDescendingOrder(
            int moveableUnitCount, IMoveableUnit[] moveableUnitsUnits, IList<Text> sentenceArray)
        {
            Text[][] arrayOfMoveableTextElements = new Text[moveableUnitCount][];
            int totalArrayWords = 0;
            for (int index = 0; index < moveableUnitsUnits.Length; index++)
            {
                var moveableUnit = moveableUnitsUnits[index];
                int moveableUnitSize = moveableUnit.EndPosition - moveableUnit.StartPosition;
                Text[] timerArray = new Text[moveableUnitSize];
                totalArrayWords += moveableUnitSize;

                int iterationCount = 0;
                for (int i = moveableUnit.StartPosition; i < moveableUnit.EndPosition; i++)
                {
                    timerArray[iterationCount] = sentenceArray[i];
                    iterationCount++;
                }

                arrayOfMoveableTextElements[index] = timerArray;
            }

            if(arrayOfMoveableTextElements[0][0].Text.EndsWith("1"))
                Array.Reverse(arrayOfMoveableTextElements);

            var reversedTimerUnit = new Text[totalArrayWords];

            int iteratorCount = -1;

            foreach (var timerTextArray in arrayOfMoveableTextElements)
            {
                foreach (Text t in timerTextArray)
                {
                    iteratorCount++;
                    var timerText = t;
                    reversedTimerUnit[iteratorCount] = timerText;
                }
            }
            return reversedTimerUnit;
        }

        public bool HasVBAToTheLeft(Text[] sentenceArray, int currentUnitIndexPosition)
        {
            var beforeCurrentUnit = sentenceArray.Take(currentUnitIndexPosition).ToArray();
            return !UnitNotFoundInSentence(
                beforeCurrentUnit,
                element => element.InnerText.IsVBA());
        }

        public bool HasDGToTheLeft(Text[] sentenceArray, int currentUnitIndexPosition)
        {
            var beforeCurrentUnit = sentenceArray.Take(currentUnitIndexPosition).ToArray();
            return !UnitNotFoundInSentence(beforeCurrentUnit, t => t.InnerText.IsDG());
        }

        public bool HasVbVbaPastToTheLeft(Text[] sentenceArray, int currentUnitIndexPosition)
        {
            var beforeCurrentUnit = sentenceArray.Take(currentUnitIndexPosition).ToArray();
            return !UnitNotFoundInSentence(
                beforeCurrentUnit,
                element =>
                    element.InnerText.IsVBA()
                    || element.InnerText.IsVB()
                    || element.InnerText.IsPast());
        }

        public bool UnitNotFoundInSentence(Text[] sentenceArray, Predicate<Text> p)
        {
            return !Array.Exists(
                sentenceArray, p);
        }

        public int GetVBAPosition(out Text[] beforeCurrentUnit, Text[] sentenceArray, int currentUnitIndexPosition)
        {
            beforeCurrentUnit = sentenceArray.Take(currentUnitIndexPosition).ToArray();
            int VBAPosition = Array.FindIndex(beforeCurrentUnit, text => text.InnerText == TagMarks.VBA);
            return VBAPosition;
        }

        public int GetFirstPastPresPositionAfterVBA(Text[] beforeCurrentUnit, int VBAPosition)
        {
            var afterVBA = beforeCurrentUnit.Skip(VBAPosition).ToArray();

            if (Array.Exists(afterVBA, text => text.InnerText.RemoveWhiteSpaces() == "PAST"))
                return Array.FindIndex(afterVBA, text => text.InnerText.RemoveWhiteSpaces() == "PAST") + VBAPosition;
            return Array.FindIndex(afterVBA, text => text.InnerText.RemoveWhiteSpaces() == "PRES") + VBAPosition;
        }

        public int GetPositionOfClosestSpecifiedUnit(Text[] sentenceArray, int currentUnitIndexPosition, Predicate<Text> p)
        {
            Text[] beforeCurrentUnit;
            Text[] afterCurrentUnit;

            ArrayUtility.SplitArrayAtPosition(
                sentenceArray, currentUnitIndexPosition, out beforeCurrentUnit, out afterCurrentUnit);

            // = i => i.IsVbPastPres();
            var VbPastPresPosition = Array.FindLastIndex(
                beforeCurrentUnit, p);

            return VbPastPresPosition;
        }

        public Text[] GetSentenceBreaker(Text[] sentenceArray)
        {
            return sentenceArray.Skip(sentenceArray.Length - 3).ToArray();
        }

        public void PopulateMoveableUnits(
            IList<Text> sentenceArray,
            ref int moveableUnitCount,
            IList<MoveableUnit> moveableUnits)
        {
            for (int index = 0; index < sentenceArray.Count; index++)
            {
                if (sentenceArray[index].IsTimer())
                {
                    AddMoveableUnit(ref moveableUnitCount, moveableUnits, index);
                }
                else if (IsFullStopEndOfSentence(sentenceArray, index))
                {
                    moveableUnits[moveableUnitCount - 1].EndPosition = index;
                    break;
                }
            }
        }

        private static bool IsFullStopEndOfSentence(IList<Text> sentenceArray, int index)
        {
            return sentenceArray[index].InnerText.IsBreakerPunctuation()
                   && sentenceArray[index + 1].Text == ".";
        }

        private static void AddMoveableUnit(ref int moveableUnitCount, IList<MoveableUnit> moveableUnits, int index)
        {
            moveableUnits[moveableUnitCount] = new MoveableUnit
            {
                StartPosition = index
            };

            moveableUnitCount++;

            if (moveableUnitCount <= 1) return;

            moveableUnits[moveableUnitCount - 2].EndPosition = index;
        }
    }
}
