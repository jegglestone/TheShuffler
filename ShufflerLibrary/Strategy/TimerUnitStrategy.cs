namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class TimerUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasTimer())
                return sentence;

            var timerSentenceDecorator =
                new TimerSentenceDecorator(sentence);

            List<Text> reversedTimerUnit = 
                GetTimerUnitsInReverse(timerSentenceDecorator);

            int firstTimerIndexPositionBeforeShuffling =
                timerSentenceDecorator.TimerIndexPosition;
                        
            ShuffleReversedTimerUnit(
                timerSentenceDecorator, reversedTimerUnit, firstTimerIndexPositionBeforeShuffling);
            
            UnderlineTimerUnit(
                timerSentenceDecorator, timerSentenceDecorator.TimerIndexPosition, reversedTimerUnit.Count);
            
            return sentence;
        }

        private static List<Text> GetTimerUnitsInReverse(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            MoveableUnit[] timerPositions =
                GetTimerUnitPositions(timerSentenceDecorator);

            timerPositions[timerPositions.Length-1].EndPosition =
                timerSentenceDecorator
                    .Texts
                    .Skip(timerSentenceDecorator.TimerIndexPosition)
                    .ToList()
                    .FindIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation))
                    +timerSentenceDecorator.TimerIndexPosition - 1;

            Array.Reverse(timerPositions);

            List<Text> reversedTimerUnit =
                MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                    timerSentenceDecorator.Texts, timerPositions);

            return reversedTimerUnit;
        }

        private static void ShuffleReversedTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, 
            List<Text> reversedTexts, 
            int originalTimerIndexPosition)
        {
            bool shuffled = false;
            if (timerSentenceDecorator.HasDG)
            {
                MoveTimerUnitToPosition(
                    timerSentenceDecorator.DGPosition,
                    reversedTexts,
                    timerSentenceDecorator);

                shuffled = true;
            }
            else if (timerSentenceDecorator.HasVBVBAPAST)
            {

                if (timerSentenceDecorator.FirstVbVbaPastPosition
                    < timerSentenceDecorator.TimerIndexPosition)
                {
                    MoveTimerUnitToPosition(
                        timerSentenceDecorator.FirstVbVbaPastPosition,
                        reversedTexts,
                        timerSentenceDecorator);

                    shuffled = true;
                }
            }

            if (shuffled == false)
            {
                MoveTimerUnitToPosition(
                    originalTimerIndexPosition,
                    reversedTexts, 
                    timerSentenceDecorator);
            }
        }

        public static void MoveTimerUnitToPosition(
            int newPosition, 
            List<Text> reversedTexts, 
            TimerSentenceDecorator timerSentenceDecorator)
        {
            RemoveCurrentTimerUnit(
                timerSentenceDecorator, reversedTexts.Count);

            if (newPosition > timerSentenceDecorator.TextCount - 1)
                timerSentenceDecorator.Texts.AddRange(reversedTexts);
            else
                timerSentenceDecorator.Texts.InsertRange(
                            newPosition,
                            reversedTexts);
        }

        private static void RemoveCurrentTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, int timerUnitSize)
        {
            int firstTimerIndexPosition = timerSentenceDecorator.TimerIndexPosition;
            timerSentenceDecorator.Texts.RemoveRange(
                            firstTimerIndexPosition,
                            timerUnitSize);
            //timerSentenceDecorator.LastTimerIndexPosition - firstTimerIndexPosition + 1);// TODO: try reversedTimers.count
        }

        private static void UnderlineTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, int newFirstTimerIndexPosition, int timerUnitCount)
        {
            timerSentenceDecorator.Texts[newFirstTimerIndexPosition].pe_merge_ahead
                = timerUnitCount - 1;
        }
        
        private static MoveableUnit[] GetTimerUnitPositions(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            return MoveableUnitHelper.GetMoveableUnitPositions(
                timerSentenceDecorator.Texts, 
                MoveableUnitHelper.NumberableUnitType.Timer, 
                timerSentenceDecorator.TimerUnitCount);
        }
    }
}
