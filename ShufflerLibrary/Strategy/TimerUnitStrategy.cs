namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
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

            if (timerSentenceDecorator.HasVBVBAPAST)
            {
                MoveTimerUnitToPosition(
                    timerSentenceDecorator.FirstVbVbaPastPosition,
                    reversedTexts,
                    timerSentenceDecorator);
               
                shuffled = true;
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
                timerSentenceDecorator);
            timerSentenceDecorator.Texts.InsertRange(
                            newPosition,
                            reversedTexts);
        }

        private static void RemoveCurrentTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            int firstTimerIndexPosition = timerSentenceDecorator.TimerIndexPosition;
            timerSentenceDecorator.Texts.RemoveRange(
                            firstTimerIndexPosition,
                            timerSentenceDecorator.LastTimerIndexPosition - firstTimerIndexPosition + 1);
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
