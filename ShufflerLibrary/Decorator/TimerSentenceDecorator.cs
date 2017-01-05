namespace ShufflerLibrary.Decorator
{
    using System.Linq;
    using Model;

    public class TimerSentenceDecorator : SentenceDecorator
    {
        public int TimerUnitCount => Texts.Count(text => text.IsTimer);

        public int TimerIndexPosition
        {
            get
            {
                if (!Sentence.HasTimer()) return -1;

                return Sentence.Texts.FindIndex(
                    text => text.IsTimer);
            }
        }

        public int LastTimerIndexPosition
        {
            get
            {
                return
                    Sentence.Texts.FindLastIndex(
                        text => text.IsTimer);
            }
        }

        public TimerSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }
    }
}
