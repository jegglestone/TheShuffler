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
                if (textsAfterBkByBeforeVB_VBA_PAST_PRES.Where(
                    text => text.IsModifier).All(text => text.text_used == " of "))
                {
                    var modifiers = textsAfterBkByBeforeVB_VBA_PAST_PRES.Where(
                        text => text.IsModifier).ToList();

                    foreach (var modifier in modifiers)
                    {
                        modifier.pe_tag_revised_by_Shuffler 
                            = UnitTypes.MDBK;
                    }
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

        private static void SetFirstMDThatIsNotOfToMDBK(
            List<Text> textsAfterBkByBeforeVB_VBA_PAST_PRES)
        {
            foreach (var text in textsAfterBkByBeforeVB_VBA_PAST_PRES)
            {
                if (text.tag_used.StartsWith(UnitTypes.MD_Modifier)
                    && text.text_used != " of ")
                {
                    text.pe_tag_revised_by_Shuffler = UnitTypes.MDBK;
                    break;
                }
            }
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
                GetMDUnitPositions(beforeMDBK);

            Array.Reverse(MDPositions);

            List<Text> reversedMDUnit =
               MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                   beforeMDBK, MDPositions);

            ReplaceMDUnitBeforeMDBKWithReversedMDUnit(
                bkBySentenceDecorator, MDPositions, reversedMDUnit);
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
                GetMDUnitPositions(afterMDBK);

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

            RemoveCurrentMDUnit(
                bkBySentenceDecorator, MDPositions, firstModifierIndexAfterBKBy);
            
            InsertReversedMDUnitBeforeMDBK(
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

            InsertReversedMDUnitBeforeMDBK(
                bKBySentenceDecorator, reversedMdUnit, firstModifierAfterMDBK);
        }

        private static void InsertReversedMDUnitBeforeMDBK(
            BKBySentenceDecorator bkBySentenceDecorator, 
            List<Text> reversedMDUnit,
            int position)
        {
            bkBySentenceDecorator.Texts.InsertRange(
                position,
                reversedMDUnit);
        }

        private static void RemoveCurrentMDUnit(
            BKBySentenceDecorator bkBySentenceDecorator,
            MoveableUnit[] MDPositions,
            int position)
        {
            bkBySentenceDecorator.Texts.RemoveRange(
                position,
                MDPositions[MDPositions.Length - 1].EndPosition + 1);
        }
        
        private MoveableUnit[] GetMDUnitPositions(List<Text> modifierUnitTexts)
        {
            return MoveableUnitHelper.GetMoveableUnitPositions(
                modifierUnitTexts,
                MoveableUnitHelper.NumberableUnitType.Modifier,
                modifierUnitTexts.Count(text => text.IsNumberedType(UnitTypes.MD_Modifier)));
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
                    InsertDeParticleBeforeAndUnderline(
                        bKBySentenceDecorator, nnPosition);

                    int pastPosition = bKBySentenceDecorator
                        .GetFirstPASTUnitPositionAfterNN(
                            bKBySentenceDecorator, nnPosition);

                    InsertDeParticleAfterAndUnderline(
                        bKBySentenceDecorator, pastPosition + 1);
                }
            }
        }

        private void InsertDeParticleBeforeAndUnderline(
            SentenceDecorator bkBySentenceDecorator, int i)
        {
            bkBySentenceDecorator.Texts.Insert(
                i,
                CreateNewDeParticle(bkBySentenceDecorator.Texts[i-1].pe_order, 1));
        }

        private void InsertDeParticleAfterAndUnderline(
            SentenceDecorator bKBySentenceDecorator, int i)
        {
            bKBySentenceDecorator.Texts.Insert(
                i+1,
                CreateNewDeParticle(
                    bKBySentenceDecorator.Texts[i].pe_order, 0));

            bKBySentenceDecorator.Texts[i].pe_merge_ahead = 1;
        }

        private static Text CreateNewDeParticle(int previous_pe_order, int pe_merge_ahead)
        {
            return new Text()
            {
                pe_text_revised = " de "
                , pe_order = previous_pe_order + 5
                , pe_merge_ahead = pe_merge_ahead
            };
        }

        private static void ReplaceBKByWithFangFashi(
            BKBySentenceDecorator bKBySentenceDecorator)
        {
            var bkByText = bKBySentenceDecorator.BkByText;
            bkByText.pe_text_revised = " fangfashi ";
        }
    }
}
