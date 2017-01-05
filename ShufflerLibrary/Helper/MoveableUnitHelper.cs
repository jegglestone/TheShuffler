using ShufflerLibrary.Model;
using System.Collections.Generic;

namespace ShufflerLibrary.Helper
{
    public static class MoveableUnitHelper
    {
        public enum NumberableUnitType
        {
            Adverb,
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
                if (numberableUnitType == NumberableUnitType.Timer && texts[i].IsTimer)
                {
                    unitCounter++;
                    unitPositions[unitCounter] =
                        new MoveableUnit() { StartPosition = i };

                    if (unitCounter > 0)
                        unitPositions[unitCounter - 1].EndPosition = i - 1;
                }
            }
            unitPositions[unitPositions.Length - 1].EndPosition =
                unitPositions[unitPositions.Length - 1].StartPosition;
            return unitPositions;
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
