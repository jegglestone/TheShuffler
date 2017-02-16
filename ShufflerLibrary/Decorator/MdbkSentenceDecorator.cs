namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class MdbkSentenceDecorator : SentenceDecorator
    {
        public MdbkSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }

        public int MdbkPosition
        {
            get
            {
                return Texts.FindIndex(text => text.IsMDBK());
            }
        }

        public int NNPositionBeforeMdbk()
        {
            return Texts.Take(MdbkPosition).ToList().FindIndex(text => text.IsNN);
        }

        public int LastNNPositionAfter(List<Text> mdbkUnitUpToVbVbaPastPresBkp)
        {
            return mdbkUnitUpToVbVbaPastPresBkp.FindLastIndex(
                text => text.IsNN) + MdbkPosition;
        }

        public int ModifierPositionAfterMdbk
        {
            get
            {
                return Texts.Skip(MdbkPosition).ToList()
                    .FindIndex(text => text.IsModifier) + MdbkPosition;                
            }
        }

        public List<Text> MdbkUnitUpToVbVbaPastPresBkp()
        {
            int mdbkUnitEndPosition =
                    Texts.Skip(MdbkPosition).ToList().FindIndex(
                      text => text.IsVbVbaPast
                              || text.IsType(UnitTypes.PRES_Participle)
                              || text.IsType(UnitTypes.BKP_BreakerPunctuation))
                    + MdbkPosition;

            var mdbkUnitUpToVbVbaPastPresBkp = new List<Text>();

            for (int i = MdbkPosition; i < mdbkUnitEndPosition; i++)
            {
                mdbkUnitUpToVbVbaPastPresBkp.Add(Texts[i]);
            }
            return mdbkUnitUpToVbVbaPastPresBkp;
        }

        public bool NnUnitBeforeMdbk(Sentence sentence, int mdbkPosition)
        {
            return sentence.Texts.Take(mdbkPosition).Any(text => text.IsNN);
        }

        public bool PrenBeforeNN(Sentence sentence, int nnPosition)
        {
            var textsUpToNn = GetTextsUpToNN(sentence, nnPosition);
            return textsUpToNn.Any(text => text.IsPren);
        }

        public bool ByBeforeNN(Sentence sentence, int nnPosition)
        {
            var textsUpToNn = GetTextsUpToNN(sentence, nnPosition);
            return textsUpToNn.Any(text => text.IsBKBy);
        }

        public bool AdjBeforeNN(Sentence sentence, int nnPosition)
        {
            var textsUpToNn = GetTextsUpToNN(sentence, nnPosition);
            return textsUpToNn.Any(text => text.IsType(UnitTypes.ADJ_Adjective));
        }

        public List<Text> GetTextsUpToNN(Sentence sentence, int nnPosition)
        {
            var textsUpToNn = sentence.Texts.Take(nnPosition).ToList();

            if (textsUpToNn.Any(text => text.IsVbPastPres || text.IsType(UnitTypes.BKP_BreakerPunctuation)))
            {
                textsUpToNn =
                    textsUpToNn
                    .Skip(
                        textsUpToNn
                        .ToList()
                        .FindLastIndex(
                            text => text.IsVbPastPres || text.IsType(UnitTypes.BKP_BreakerPunctuation)))
                    .ToList();
            }

            return textsUpToNn;
        }

    }
}
