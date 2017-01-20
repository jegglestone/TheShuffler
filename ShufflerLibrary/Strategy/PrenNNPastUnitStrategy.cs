namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Model;

    public class PrenNNPastUnitStrategy : IStrategy
    {
        private int _nnPosition;
        private int _prenPosition;

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

            if (IsModifierOrTimerBetween_NNPast_And_VbNbkpBkp(
                sentence, pastPosition, firstVbNbkpbkp))
            {
                // if many MD shuffle descending

                // if many TM shuffle descending

                // move the MD/TM to before PAST

                // Move newly formed MD/TM + PAST to before NN
            }


            return sentence;
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
    }
}
