namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class MDUnitStrategy : IStrategy
    {
        private MDSentenceDecorator _mdSentenceDecorator;

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsModifier))
                return sentence;

            _mdSentenceDecorator = 
                new MDSentenceDecorator(sentence);

            if (_mdSentenceDecorator.HasMoreThanOneModifier())
            {
                int firstModifierPosition = 
                    _mdSentenceDecorator.FirstModifierPosition;

                List<Text> modifiersUpToVBorBK = 
                    _mdSentenceDecorator.GetModifierUnitUpToVBorBK(firstModifierPosition);

                if (modifiersUpToVBorBK.Count > 1)
                {
                    var mdPositions = ModifierPositionHelper.GetMDUnitPositions(
                        modifiersUpToVBorBK);

                    if (_mdSentenceDecorator.ReversableUnitsAreSortedAscending(
                        modifiersUpToVBorBK, text => text.IsModifier))
                    {
                        SortModifiersInDescendingNumericOrder(
                            modifiersUpToVBorBK, firstModifierPosition, mdPositions);
                    }

                    if (_mdSentenceDecorator.
                        ModifierUnitHasTimerUnit(modifiersUpToVBorBK))
                    {
                        if (TextsBefore(firstModifierPosition)
                            .Any(text => text.IsVbVbaPast))
                        {
                            MoveModifiersBeforeVbVbaPast(
                                mdPositions, firstModifierPosition, modifiersUpToVBorBK);

                            DeleteModifiers();
                        }
                        //the structure of PREN/DIG/ADJ + NN is found 
                        else if (TextsBefore(firstModifierPosition)
                            .Any(text => text.IsNN))
                        {
                            int nnPosition = TextsBefore(firstModifierPosition).ToList().FindLastIndex(
                                text => text.IsNN);

                            if (_mdSentenceDecorator.PrenAdjOrDigBeforeNN(nnPosition))
                            {
                                ApplyPrenDigAdjPlusNNRules(
                                    firstModifierPosition, mdPositions, nnPosition);
                            }
                        }
                    }
                }
            }

            if (_mdSentenceDecorator.SentenceHasSingleModifierAndPyXuyaoUnit())
            {
                int PyXuyaoPosition =
                    _mdSentenceDecorator.Texts.FindIndex(
                        text => text.IsPyXuyao);

                if (_mdSentenceDecorator.PyXuyaoIsWithinMDandPreceededByNN(
                    PyXuyaoPosition))
                {
                    ApplyMDPlusPYXuyaoRules(
                        PyXuyaoPosition);
                }
            }

            return sentence;
        }

        private void ApplyMDPlusPYXuyaoRules(int PyXuyaoPosition)
        {
            int nnPosition = _mdSentenceDecorator
                .TextsBeforePyXuyao(PyXuyaoPosition)
                .ToList()
                .FindLastIndex(text => text.IsNN);

            int modifierStartPosition = PyXuyaoPosition - 1;

            int modifierEndPosition =
                _mdSentenceDecorator.FirstBKPPositionAfterFirstModifier;

            var mdPyXuyaoUnit = _mdSentenceDecorator
                .Texts
                .Skip(modifierStartPosition)
                .ToList()
                .Take(modifierEndPosition - modifierStartPosition);

            _mdSentenceDecorator.Texts.RemoveRange(
                modifierStartPosition,
                modifierEndPosition - modifierStartPosition);

            _mdSentenceDecorator.Texts.InsertRange(
                nnPosition,
                mdPyXuyaoUnit);

            DeleteModifiers();
        }

        private void ApplyPrenDigAdjPlusNNRules(
            int firstModifierPosition, 
            MoveableUnit[] mdPositions, 
            int nnPosition)
        {
            int lastPrenDigAdjPosition = nnPosition - 1;

            int firstModifierCurrentPosition =
                _mdSentenceDecorator.FirstModifierPosition;

            int modifierEndPosition =
                mdPositions.Last().EndPosition + 1;

            modifierEndPosition =
                RemovePrensFromUnitAndDecrementEndPosition(
                    firstModifierCurrentPosition, modifierEndPosition);

            MoveModifierUnitAfter_PrenDigAdj_AndAddDeParticle(
                firstModifierPosition,
                modifierEndPosition,
                firstModifierCurrentPosition,
                lastPrenDigAdjPosition);

            DeleteModifiers();
        }

        private void MoveModifierUnitAfter_PrenDigAdj_AndAddDeParticle(
            int firstModifierPosition, 
            int modifierEndPosition,
            int firstModifierCurrentPosition, 
            int lastPrenDigAdjPosition)
        {
            var modifierUnit =
                _mdSentenceDecorator
                    .Texts
                    .Skip(firstModifierPosition)
                    .Take(modifierEndPosition).ToList();

            _mdSentenceDecorator.Texts.RemoveRange(
                firstModifierCurrentPosition,
                modifierEndPosition);

            modifierUnit.Add(
                DeParticleHelper.CreateNewDeParticle(
                    modifierUnit.Last().pe_order, 0));

            _mdSentenceDecorator.Texts.InsertRange(
                lastPrenDigAdjPosition + 1,
                modifierUnit);
        }

        private int RemovePrensFromUnitAndDecrementEndPosition(
            int firstModifierCurrentPosition, int modifierEndPosition)
        {
            for (int i = firstModifierCurrentPosition;
                 i < firstModifierCurrentPosition + modifierEndPosition; i++)
            {
                if (_mdSentenceDecorator.Texts[i].IsPren)
                {
                    _mdSentenceDecorator.Texts.RemoveAt(i);
                    modifierEndPosition--;
                }
            }

            return modifierEndPosition;
        }

        private IEnumerable<Text> TextsBefore(int position)
        {
            return _mdSentenceDecorator.Texts.Take(position);
        }

        private void DeleteModifiers()
        {
            _mdSentenceDecorator.Texts.RemoveAll(text => text.IsModifier);
        }

        private void MoveModifiersBeforeVbVbaPast(
            MoveableUnit[] mdPositions, 
            int firstModifierPosition,
            List<Text> modifiersUpToVBorBK)
        {
            ModifierPositionHelper.RemoveCurrentMDUnit(
                _mdSentenceDecorator,
                mdPositions,
                _mdSentenceDecorator.FirstModifierPosition);

            int VBVBAPASTPosition = _mdSentenceDecorator
                .Texts
                .Take(firstModifierPosition)
                .ToList()
                .FindIndex(text => text.IsVbVbaPast);

            ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
                _mdSentenceDecorator,
                modifiersUpToVBorBK,
                VBVBAPASTPosition);
        }

        private void SortModifiersInDescendingNumericOrder(
            List<Text> modifiersUpToVBorBK, 
            int firstModifierPosition,
            MoveableUnit[] mdPositions)
        {
            ModifierPositionHelper.RemoveCurrentMDUnit(
                _mdSentenceDecorator,
                mdPositions,
                firstModifierPosition);

            Array.Reverse(mdPositions);

            List<Text> reversedMDUnit =
                MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                    modifiersUpToVBorBK, mdPositions);

            ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
                _mdSentenceDecorator,
                reversedMDUnit,
                firstModifierPosition);
        }
    }
}
