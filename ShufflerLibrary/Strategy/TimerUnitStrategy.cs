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

            // TODO: underlineTimerUnit
            int newFirstTimerIndexPosition =
                timerSentenceDecorator.TimerIndexPosition;

            UnderlineTimerUnit(
                timerSentenceDecorator, newFirstTimerIndexPosition, reversedTexts.Count);

            

            return sentence;
        }

        private static void InsertReversedTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, 
            List<Text> reversedTexts, 
            int firstTimerIndexPosition)
        {
            int positionToInsert = firstTimerIndexPosition;

            if (timerSentenceDecorator.HasVBVBAPAST)
            {
                positionToInsert = 
                    timerSentenceDecorator.FirstVbVbaPastPosition;
            }
            // check for DG before Timer Unit
            if (timerSentenceDecorator.Texts.Exists(
                text => text.pe_tag == "DG"))
            {
                // refactor checking both tag and revised tag and move to Text property etc
                positionToInsert =
                    timerSentenceDecorator.Texts.FindIndex(text => text.pe_tag == "DG"); //TODO: property in decorator
            }

            timerSentenceDecorator.Texts.InsertRange(
                            positionToInsert,
                            reversedTexts);
        }

        private static void RemoveTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, int firstTimerIndexPosition)
        {
            timerSentenceDecorator.Texts.RemoveRange(
                            timerSentenceDecorator.TimerIndexPosition,
                            timerSentenceDecorator.LastTimerIndexPosition - firstTimerIndexPosition + 1);
        }

        private static void UnderlineTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, int newFirstTimerIndexPosition, int timerUnitCount)
        {
            timerSentenceDecorator.Texts[newFirstTimerIndexPosition].pe_merge_ahead
                = timerUnitCount - 1;
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
