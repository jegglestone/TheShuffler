namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class BKBySentenceDecorator : SentenceDecorator
    {
        public int BKByIndexPosition
        {
            get
            {
                if (!Sentence.HasBKBy) return -1;

                return Sentence.Texts.FindIndex(
                    text => text.IsBKBy);
            }
        }

        public int Proceeding_NBKP_BKP_VB_VBA_PAST_position
        {
            get
            {
                return 
                    Sentence.Texts.Skip(BKByIndexPosition).ToList().FindIndex(
                        text => text.IsVbVbaPast
                            || text.IsType(UnitTypes.BKP_BreakerPunctuation)
                            || text.IsType(UnitTypes.NBKP_NonBreakerPunctuation));
            }
        }

        public bool HasPRES(List<Text> textUnit)
        {
            return textUnit.Any(
                text => text.IsType(UnitTypes.PRES_Participle));
        }

        public bool HasNN(List<Text> textUnit)
        {
            return textUnit.Any(
                text => text.IsType(UnitTypes.NN));
        }

        public int NNPosition
        {
            get
            {
                return Sentence.Texts.FindIndex(
                    text => text.IsType(UnitTypes.NN));
            }
        }

        public Text BkByText
        {
            get
            {
                return Sentence.Texts.Find(
                    text => text.IsBKBy);
            }
        }

        public BKBySentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }
    }
}
