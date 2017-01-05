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

            MoveableUnit[] timerPositions =
                GetTimerUnitPositions(timerSentenceDecorator);

            Array.Reverse(timerPositions);
            List<Text> reversedTexts = MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                timerSentenceDecorator.Texts, timerPositions);

            int firstTimerIndexPosition =
                timerSentenceDecorator.TimerIndexPosition;

            RemoveTimerUnit(
                timerSentenceDecorator, firstTimerIndexPosition);

            InsertReversedTimerUnit(
                timerSentenceDecorator, reversedTexts, firstTimerIndexPosition);

            // underline with merge_ahead

            return sentence;
        }

        private static void InsertReversedTimerUnit(TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts, int firstTimerIndexPosition)
        {
            timerSentenceDecorator.Texts.InsertRange(
                            firstTimerIndexPosition,
                            reversedTexts);
        }

        private static void RemoveTimerUnit(TimerSentenceDecorator timerSentenceDecorator, int firstTimerIndexPosition)
        {
            timerSentenceDecorator.Texts.RemoveRange(
                            timerSentenceDecorator.TimerIndexPosition,
                            timerSentenceDecorator.LastTimerIndexPosition - firstTimerIndexPosition + 1);
        }

        private static MoveableUnit[] GetTimerUnitPositions(TimerSentenceDecorator timerSentenceDecorator)
        {
            return MoveableUnitHelper.GetMoveableUnitPositions(
                timerSentenceDecorator.Texts, 
                MoveableUnitHelper.NumberableUnitType.Timer, 
                timerSentenceDecorator.TimerUnitCount);
        }
    }
}
