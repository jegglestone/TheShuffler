namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class BKByUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if(!sentence.HasBKBy)
                return sentence;

            var bKBySentenceDecorator = 
                new BKBySentenceDecorator(sentence);

            int bkByPosition = 
                bKBySentenceDecorator.BKByIndexPosition;

            var bKBYUnit = bKBySentenceDecorator.Texts.GetRange(
                bkByPosition,
                bKBySentenceDecorator.Proceeding_NBKP_BKP_VB_VBA_PAST_position);

            if (bKBySentenceDecorator.HasPRES(bKBYUnit))
            {
                ReplaceBKByWithFangFashi(
                    bKBySentenceDecorator);
            }
            else if (bKBySentenceDecorator.HasNN(bKBYUnit))
            {
                ApplyDeParticlesWhereNNThenPASTThenBKBy(
                    bKBySentenceDecorator);
            }
           
            List<Text> textsAfterBkByBeforeVB_VBA_PAST_PRES = 
                GetTextsAfterBkByBeforeVB_VBA_PAST_PRES(bKBySentenceDecorator);

            if (textsAfterBkByBeforeVB_VBA_PAST_PRES.Any(
                text => text.IsModifier))
            {
                if (AllModifiersAreOf(textsAfterBkByBeforeVB_VBA_PAST_PRES))
                {
                    ReplaceModifiersWithMDBK(
                        textsAfterBkByBeforeVB_VBA_PAST_PRES);
                }
                else
                {
                    SetFirstMDThatIsNotOfToMDBK(
                        textsAfterBkByBeforeVB_VBA_PAST_PRES);

                    ShuffleMDUnitsBeforeMDBKInDescendingOrder(
                        textsAfterBkByBeforeVB_VBA_PAST_PRES,
                        bKBySentenceDecorator);

                    ShuffleMDUnitsAfterMDBKInDescendingOrder(
                        textsAfterBkByBeforeVB_VBA_PAST_PRES,
                        bKBySentenceDecorator);
                }
            }

            return sentence;
        }

        private static bool AllModifiersAreOf(List<Text> textsAfterBkByBeforeVB_VBA_PAST_PRES)
        {
            return textsAfterBkByBeforeVB_VBA_PAST_PRES.Where(
                text => text.IsModifier).All(text => text.actual_text_used == " of ");
        }

        private static void ReplaceModifiersWithMDBK(List<Text> textsAfterBkByBeforeVB_VBA_PAST_PRES)
        {
            var modifiers = textsAfterBkByBeforeVB_VBA_PAST_PRES.Where(
                text => text.IsModifier).ToList();

            foreach (var modifier in modifiers)
            {
                modifier.pe_tag_revised_by_Shuffler
                    = UnitTypes.MDBK;
            }
        }

        private static void SetFirstMDThatIsNotOfToMDBK(
            List<Text> textsAfterBkByBeforeVB_VBA_PAST_PRES)
        {
            foreach (var text in textsAfterBkByBeforeVB_VBA_PAST_PRES)
            {
                if (IsModifierThatIsNotOf(text))
                {
                    text.pe_tag_revised_by_Shuffler = UnitTypes.MDBK;
                    break;
                }
            }
        }

        private static bool IsModifierThatIsNotOf(Text text)
        {
            return text.actual_tag_used != null && 
                (text.actual_tag_used.StartsWith(UnitTypes.MD_Modifier)
                && text.actual_text_used != " of ");
        }

        private void ShuffleMDUnitsBeforeMDBKInDescendingOrder(
            List<Text> textsAfterBkByBeforeVbVbaPastPres,
            BKBySentenceDecorator bkBySentenceDecorator)
        {
            int MDBKPosition = 
                bkBySentenceDecorator
                    .GetMDBKPosition(textsAfterBkByBeforeVbVbaPastPres);

            var beforeMDBK =
                textsAfterBkByBeforeVbVbaPastPres.Take(MDBKPosition).ToList();

            MoveableUnit[] MDPositions =
                ModifierPositionHelper.GetMDUnitPositions(beforeMDBK);

            if (MDPositions.Any())
            {
                Array.Reverse(MDPositions);

                List<Text> reversedMDUnit =
                    MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                        beforeMDBK, MDPositions);

                ReplaceMDUnitBeforeMDBKWithReversedMDUnit(
                    bkBySentenceDecorator, MDPositions, reversedMDUnit);
            }
        }

        private void ShuffleMDUnitsAfterMDBKInDescendingOrder(
            List<Text> textsAfterBkByBeforeVbVbaPastPres,
            BKBySentenceDecorator bKBySentenceDecorator)
        {
            int MDBKPosition =
                bKBySentenceDecorator
                    .GetMDBKPosition(textsAfterBkByBeforeVbVbaPastPres);

            var afterMDBK =
                textsAfterBkByBeforeVbVbaPastPres
                .Skip(MDBKPosition + 1)
                .ToList();

            MoveableUnit[] MDPositions =
                ModifierPositionHelper.GetMDUnitPositions(afterMDBK);

            Array.Reverse(MDPositions);

            List<Text> reversedMDUnit =
               MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                   afterMDBK, MDPositions);

            ReplaceMDUnitAfterMDBKWithReversedMDUnit(
                bKBySentenceDecorator, reversedMDUnit);
        }

        private static void ReplaceMDUnitBeforeMDBKWithReversedMDUnit(
            BKBySentenceDecorator bkBySentenceDecorator,
            MoveableUnit[] MDPositions, 
            List<Text> reversedMDUnit)
        {
            int firstModifierIndexAfterBKBy = 
                bkBySentenceDecorator.FirstModifierIndexAfterBKBy;

            ModifierPositionHelper.RemoveCurrentMDUnit(
                bkBySentenceDecorator, MDPositions, firstModifierIndexAfterBKBy);
            
            ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
                bkBySentenceDecorator, reversedMDUnit, firstModifierIndexAfterBKBy);
        }

        private void ReplaceMDUnitAfterMDBKWithReversedMDUnit(
             BKBySentenceDecorator bKBySentenceDecorator,
             List<Text> reversedMdUnit)
        {
            int firstModifierAfterMDBK = 
                bKBySentenceDecorator.FirstModifierAfterMDBK;

            bKBySentenceDecorator.Texts.RemoveRange(
                firstModifierAfterMDBK, reversedMdUnit.Count);

            ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
                bKBySentenceDecorator, reversedMdUnit, firstModifierAfterMDBK);
        }

        private static List<Text> GetTextsAfterBkByBeforeVB_VBA_PAST_PRES(BKBySentenceDecorator bkBySentenceDecorator)
        {
            int bkByPos = bkBySentenceDecorator.BKByIndexPosition;
            int VB_VBA_PAST_PRES_position = bkBySentenceDecorator.First_VB_VBA_PAST_PRES_PositionAfterBkBy;

            return bkBySentenceDecorator
                    .Texts
                    .Skip(bkByPos)
                    .Take(VB_VBA_PAST_PRES_position).ToList();
        }

        private void ApplyDeParticlesWhereNNThenPASTThenBKBy(BKBySentenceDecorator bKBySentenceDecorator)
        {
            var textsBeforeBy =
                bKBySentenceDecorator.TextsBeforeBy;

            if (bKBySentenceDecorator.NNUnitBeforeBkBy(textsBeforeBy))
            {
                int nnPosition = bKBySentenceDecorator.NNPosition;

                if (bKBySentenceDecorator.IsPASTUnitBetweenNNandBKBy(textsBeforeBy, nnPosition))
                {
                    DeParticleHelper.InsertDeParticleBeforeAndUnderline(
                        bKBySentenceDecorator, nnPosition);

                    int pastPosition = bKBySentenceDecorator
                        .GetFirstPASTUnitPositionAfterNN(
                            bKBySentenceDecorator, nnPosition);

                    DeParticleHelper.InsertDeParticleAfterAndUnderline(
                        bKBySentenceDecorator, pastPosition + 1);
                }
            }
        }

        private static void ReplaceBKByWithFangFashi(
            BKBySentenceDecorator bKBySentenceDecorator)
        {
            var bkByText = bKBySentenceDecorator.BkByText;
            bkByText.pe_text_revised = " fangfashi ";
        }
    }
}
