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
            return !NoVBAFoundInSentence(beforeCurrentUnit);
        }

        public bool HasDGToTheLeft(Text[] sentenceArray, int currentUnitIndexPosition)
        {
            var beforeCurrentUnit = sentenceArray.Take(currentUnitIndexPosition).ToArray();
            return !NoDGFoundInSentence(beforeCurrentUnit);
        }

        public bool HasVbVbaPastToTheLeft(Text[] sentenceArray, int currentUnitIndexPosition)
        {
            var beforeCurrentUnit = sentenceArray.Take(currentUnitIndexPosition).ToArray();
            return !NoVbVbaPastFoundInSentence(beforeCurrentUnit);
        }

        public bool NoVBAFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsVBA());
        }

        public bool NoVbVbaPastFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => 
                    element.InnerText.IsVBA()
                    || element.InnerText.IsVB()
                    || element.InnerText.IsPast());
        }

        public bool NoDGFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsDG());
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
    }
}
