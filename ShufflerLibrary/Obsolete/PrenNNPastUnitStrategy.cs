namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class PrenNNPastUnitStrategy : IStrategy
    {
        private int _nnPosition;
        private int _prenPosition;

        private PrenNNPastSentenceDecorator
            _prenNNPastSentenceDecorator;

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!HasPrenNNPastStructure(sentence) && !HasNNPastStructure(sentence))
                return sentence;

            int pastPosition = 
                sentence
                .Texts
                .Skip(_nnPosition).ToList()
                .FindIndex(text => text.IsPast) + _nnPosition;

            int firstVbNbkpbkp = 
                sentence
                .Texts
                .Skip(pastPosition).ToList()
                .FindIndex(
                    text => text.IsType(UnitTypes.VB_Verb)
                    || text.IsType(UnitTypes.NBKP_NonBreakerPunctuation)
                    || text.IsType(UnitTypes.BKP_BreakerPunctuation));

            _prenNNPastSentenceDecorator = new PrenNNPastSentenceDecorator(sentence);

            if (IsModifierOrTimerBetween_NNPast_And_VbNbkpBkp(
                sentence, pastPosition, firstVbNbkpbkp))
            {
                if (_prenNNPastSentenceDecorator.HasMoreThanOneModifier())
                {
                    SortModifiersAndMoveModifiersAndPastBeforeNN(sentence);
                }
                if (_prenNNPastSentenceDecorator.HasMoreThanOneTimer)
                {
                    SortTimersAndMoveTimersAndPastBeforeNN(sentence);
                }
            }

            return sentence;
        }

        private void SortTimersAndMoveTimersAndPastBeforeNN(Sentence sentence)
        {
            int firstTimerPosition =
                _prenNNPastSentenceDecorator.FirstTimerPosition;

            List<Text> timersUpToVBorBK =
                _prenNNPastSentenceDecorator.GetTimerUnitUpToVBorBK(
                    firstTimerPosition);

            if (timersUpToVBorBK.Count > 1)
            {
                var positions = TimerPositionHelper.GetTMUnitPositions(
                    timersUpToVBorBK);
                if (_prenNNPastSentenceDecorator.ReversableUnitsAreSortedAscending(
                    timersUpToVBorBK, text => text.IsTimer))  
                {
                    SortReversableUnitInDescendingNumericOrderAndMoveBeforeNN(
                        timersUpToVBorBK,
                        firstTimerPosition,
                        _nnPosition,
                        positions);
                }

                MovePastBeforeNN(sentence, UnitTypes.TM_TimerPrefix);
            }
        }

        private void SortModifiersAndMoveModifiersAndPastBeforeNN(Sentence sentence)
        {
            int firstModifierPosition =
                _prenNNPastSentenceDecorator.FirstModifierPosition;

            List<Text> modifiersUpToVBorBK = new List<Text>();
               // _prenNNPastSentenceDecorator.GetModifierUnitUpToVBorBK(firstModifierPosition);

            if (modifiersUpToVBorBK.Count > 1)
            {
                var positions = ModifierPositionHelper.GetMDUnitPositions(
                    modifiersUpToVBorBK);
                if (_prenNNPastSentenceDecorator.ReversableUnitsAreSortedAscending(
                    modifiersUpToVBorBK, text => text.IsModifier))
                {
                    SortReversableUnitInDescendingNumericOrderAndMoveBeforeNN(
                        modifiersUpToVBorBK,
                        firstModifierPosition,
                        _nnPosition,
                        positions);
                }

                MovePastBeforeNN(sentence, UnitTypes.MD_Modifier);
            }
        }

        private void MovePastBeforeNN(Sentence sentence, string unitType)
        {
            int reversedUnitPosition;

            if (unitType == UnitTypes.TM_TimerPrefix)
                reversedUnitPosition = _prenNNPastSentenceDecorator.FirstTimerPosition;
            else if (unitType == UnitTypes.MD_Modifier)
                reversedUnitPosition = _prenNNPastSentenceDecorator.FirstModifierPosition;
            else return;

            // move past beforeNN
            _nnPosition = sentence
                .Texts
                .Skip(reversedUnitPosition) 
                .ToList()
                .FindIndex(text => text.IsNN) + reversedUnitPosition;

            var pastPosition = sentence
                .Texts
                .Skip(reversedUnitPosition)
                .ToList()
                .FindIndex(text => text.IsPast) + reversedUnitPosition;

            var pastUnit = sentence.Texts[pastPosition];

            sentence.Texts.RemoveAt(pastPosition);
            sentence.Texts.Insert(_nnPosition, pastUnit);
        }

        private static bool IsModifierOrTimerBetween_NNPast_And_VbNbkpBkp(
            Sentence sentence, int pastPosition, int firstVbNbkpbkp)
        {
            return sentence
                .Texts
                .Skip(pastPosition)
                .Take(firstVbNbkpbkp)
                .Any(text => text.IsModifier || text.IsTimer);
        }

        private bool HasNNPastStructure(Sentence sentence)
        {
            if (sentence.Texts.Any(text => text.IsNN))
            {
                _nnPosition =
                    sentence.Texts.FindIndex(text => text.IsNN);

                return sentence.Texts.Skip(_nnPosition).Any(text => text.IsPast);

            }
            return false;
        }

        private bool HasPrenNNPastStructure(Sentence sentence)
        {
            if (sentence.Texts.Any(text => text.IsPren))
            {
                _prenPosition = sentence.Texts.FindIndex(text => text.IsPren);

                if (sentence.Texts.Skip(_prenPosition).Any(text => text.IsNN))
                {
                    _nnPosition = 
                        _prenPosition + sentence.Texts.Skip(_prenPosition).ToList().FindIndex(text => text.IsNN);

                    return sentence.Texts.Skip(_nnPosition).Any(text => text.IsPast);
                }
            }
            return false;
        }

        private void SortReversableUnitInDescendingNumericOrderAndMoveBeforeNN(
            List<Text> modifiersUpToVBorBK,
            int firstModifierPosition,
            int newPosition,
            MoveableUnit[] mdPositions)
        {
            ModifierPositionHelper.RemoveCurrentMDUnit(
                _prenNNPastSentenceDecorator,
                mdPositions,
                firstModifierPosition);

            Array.Reverse(mdPositions);

            List<Text> reversedMDUnit =
                MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                    modifiersUpToVBorBK, mdPositions);

            ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
                _prenNNPastSentenceDecorator,
                reversedMDUnit,
                newPosition);
        }
    }
}
