using System.Collections.Generic;
using System.Linq;

namespace ShufflerLibrary.Helper
{
    using Decorator;
    using Model;

    public static class ModifierPositionHelper
    { 
        public static MoveableUnit[] GetMDUnitPositions(
            List<Text> modifierUnitTexts)
        {
            return MoveableUnitHelper.GetMoveableUnitPositions(
                modifierUnitTexts,
                MoveableUnitHelper.NumberableUnitType.Modifier,
                modifierUnitTexts.Count(text => 
                text.IsNumberedType(UnitTypes.MD_Modifier)));
        }

        public static void RemoveCurrentMDUnit(
            SentenceDecorator sentenceDecorator,
            MoveableUnit[] MDPositions,
            int startPosition)
        {
            sentenceDecorator.Texts.RemoveRange(
                startPosition,
                MDPositions[MDPositions.Length - 1].EndPosition + 1);
        }

        public static void InsertReversedMDUnitBeforePosition(
            SentenceDecorator sentenceDecorator,
            List<Text> reversedMDUnit,
            int startPosition)
        {
            sentenceDecorator.Texts.InsertRange(
                startPosition,
                reversedMDUnit);
        }
    }
}
