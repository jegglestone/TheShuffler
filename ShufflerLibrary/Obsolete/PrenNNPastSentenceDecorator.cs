namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using Model;
    using System.Linq;

    public class PrenNNPastSentenceDecorator : SentenceDecorator
    {
         public PrenNNPastSentenceDecorator(Sentence sentence)
         {
             Sentence = sentence;
         }

        public bool HasMoreThanOneTimer
        {
            get { return Texts.Count(text => text.IsTimer) > 1; }
        }

        public int FirstVBorBKPPositionAfterFirstTimer
        {
            get
            {
                return Texts
                    .Skip(FirstTimerPosition)
                    .ToList()
                    .FindIndex(text => text.IsType(UnitTypes.VB_Verb)
                                       || text.IsType(UnitTypes.BKP_BreakerPunctuation)
                                       || text.IsType(UnitTypes.NbkpNonBreakerPunctuation))
                       + FirstTimerPosition;
            }
        }

        public List<Text> GetTimerUnitUpToVBorBK(
            int firstTimerPosition)
        {
            List<Text> timersUpToVBorBK = new List<Text>();

            for (int i = firstTimerPosition;
                i < FirstVBorBKPPositionAfterFirstTimer; i++)
            {
                timersUpToVBorBK.Add(Texts[i]);
            }

            return timersUpToVBorBK;
        }
    }
}
