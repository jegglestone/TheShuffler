﻿namespace ShufflerLibrary.Decorator
{
    using System.Linq;
    using Model;

    public class TimerSentenceDecorator : SentenceDecorator
    {
        public int TimerUnitCount => Texts.Count(text => text.IsTimer);

        public int LastTimerIndexPosition
        {
            get
            {
                return
                    Sentence.Texts.FindLastIndex(
                        text => text.IsTimer);
            }
        }
        
        public int FirstVbVbaPastPosition
        {
            get
            {
                return Sentence.HasVBVBAPAST
                    ? Sentence.Texts.FindIndex(
                        t => t.IsVbVbaPast)
                    : -1;
            }
        }

        public int DIGPosition
        {
            get
            {
                return Sentence.Texts.FindIndex(
                    text => text.IsType(UnitTypes.DIG_Digit));
            }
        }

        public TimerSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }
    }
}
