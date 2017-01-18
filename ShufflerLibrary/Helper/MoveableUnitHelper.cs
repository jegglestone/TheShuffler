using ShufflerLibrary.Model;
using System.Collections.Generic;

namespace ShufflerLibrary.Helper
{
    using System.Windows.Forms;

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
            var unitPositions =
               new MoveableUnit[unitCount];
            
            int unitCounter = -1;

            for (int i = 0; i < texts.Count; i++)
            {
                if (!CurrentTextIsMatchingType(texts, numberableUnitType, i))
                    continue;

                unitCounter++;
                unitPositions[unitCounter] =
                    new MoveableUnit() { StartPosition = i };

                if (PastTheFirstUnit(
                    unitCounter))
                {
                    SetEndPositionOfPreviousItem(unitPositions, unitCounter, i);
                }

                if (IsLastUnit(
                    unitCount, unitCounter))
                {
                    SetEndPositionOfFinalItem(
                        texts, unitPositions);
                }
            }

            return unitPositions;
        }

        private static void SetEndPositionOfPreviousItem(MoveableUnit[] unitPositions, int unitCounter, int currentPosition)
        {
            unitPositions[unitCounter - 1].EndPosition = currentPosition - 1;
        }

        private static bool CurrentTextIsMatchingType(List<Text> texts, NumberableUnitType numberableUnitType, int i)
        {
            return (numberableUnitType == NumberableUnitType.Timer && texts[i].IsTimer)
                                || (numberableUnitType == NumberableUnitType.Modifier && texts[i].IsModifier);
        }

        private static bool PastTheFirstUnit(int unitCounter)
        {
            return unitCounter > 0;
        }

        private static bool IsLastUnit(
            int unitCount, int unitCounter)
        {
            return unitCounter == unitCount - 1;
        }

        private static void SetEndPositionOfFinalItem(
            List<Text> texts, MoveableUnit[] unitPositions)
        {
            int endPosition = 
                texts.FindLastIndex(
                    text => text.IsType(UnitTypes.BKP_BreakerPunctuation)
                            || text.IsType(UnitTypes.BK_Breaker)) -1;
            
            if (endPosition <= -1)
                endPosition = texts.Count - 1;

            if (unitPositions[unitPositions.Length - 1].StartPosition > endPosition)
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
