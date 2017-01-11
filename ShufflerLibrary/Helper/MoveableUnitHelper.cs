using ShufflerLibrary.Model;
using System.Collections.Generic;

namespace ShufflerLibrary.Helper
{
    public static class MoveableUnitHelper
    {
        public enum NumberableUnitType
        {
            Adverb,
            Modifier,
            Timer
        };

        public static MoveableUnit[] GetMoveableUnitPositions(
            List<Text> texts, NumberableUnitType numberableUnitType, int unitCount)
        {
            MoveableUnit[] unitPositions =
               new MoveableUnit[unitCount];

            int unitCounter = -1;
            for (int i = 0; i < texts.Count; i++)
            {
                if ((numberableUnitType == NumberableUnitType.Timer && texts[i].IsTimer)
                    || (numberableUnitType == NumberableUnitType.Modifier && texts[i].IsModifier))
                {
                    unitCounter++;
                    unitPositions[unitCounter] =
                        new MoveableUnit() { StartPosition = i };

                    if (unitCounter > 0)
                        unitPositions[unitCounter - 1].EndPosition = i - 1;

                    if (IsLastUnit(
                        unitCount, unitCounter))
                    {
                        SetEndPositionOfLastItem(
                            texts, unitPositions);

                    }
                }
            }
            
            return unitPositions;
        }

        private static bool IsLastUnit(
            int unitCount, int unitCounter)
        {
            return unitCounter == unitCount - 1;
        }

        private static void SetEndPositionOfLastItem(
            List<Text> texts, MoveableUnit[] unitPositions)
        {
            int endPosition = 
                texts.FindLastIndex(
                    text => text.IsType(UnitTypes.BKP_BreakerPunctuation)
                            || text.IsType(UnitTypes.BK_Breaker)) - 1;

            // should be greater than start position
            if (endPosition <= -1)
                endPosition = texts.Count - 1;

            unitPositions[unitPositions.Length - 1].EndPosition
                = endPosition;
        }

        public static List<Text> GetTextsFromMoveablePositionsList(
            List<Text> texts, MoveableUnit[] unitPositions)
        {
            var reversedTexts = new List<Text>();
            
            for (int i = 0; i < unitPositions.Length; i++)
            {
                reversedTexts.AddRange(texts.GetRange(
                    unitPositions[i].StartPosition,
                    unitPositions[i].EndPosition - unitPositions[i].StartPosition + 1));
            }

            return reversedTexts;
        }
    }
}
