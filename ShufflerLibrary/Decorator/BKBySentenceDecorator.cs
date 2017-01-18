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

        public int First_VB_VBA_PAST_PRES_PositionAfterBkBy
        {
            get
            {
                return Sentence
                    .Texts
                    .Skip(BKByIndexPosition)
                    .ToList()
                    .FindIndex(
                        text => text.IsVbVbaPast 
                        || text.IsType(UnitTypes.PRES_Participle));
            }
        }

        public int FirstModifierIndexAfterBKBy
        {
            get
            {
                return
                    Texts
                        .Skip(BKByIndexPosition)
                        .ToList()
                        .FindIndex(
                            text => text.IsModifier) + BKByIndexPosition;
            }
        }

        public int FirstModifierAfterMDBK
        {
            get
            {
                int MDBKIndex =
                    Texts.FindIndex(text => text.pe_tag_revised_by_Shuffler == "MDBK") + 1;

                int positionWithinSubset =
                    Texts
                        .Skip(MDBKIndex)
                        .ToList()
                        .FindIndex(
                            text => text.IsType(UnitTypes.MD_Modifier)
                            || text.IsNumberedType(UnitTypes.MD_Modifier)) ;

                return positionWithinSubset + MDBKIndex;
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

        public bool IsPASTUnitBetweenNNandBKBy(
            List<Text> textsBeforeBy, int nnPosition)
        {
            return textsBeforeBy.Skip(nnPosition).Any(
                                text => text.IsType(UnitTypes.PAST_Participle));
        }

        public bool NNUnitBeforeBkBy(
            List<Text> textsBeforeBy)
        {
            return textsBeforeBy.Any(
                            text => text.IsType(UnitTypes.NN));
        }

        public int GetMDBKPosition(List<Text> textsAfterBkByBeforeVbVbaPastPres)
        {
            return textsAfterBkByBeforeVbVbaPastPres.FindIndex(
                    text => text.pe_tag_revised_by_Shuffler == UnitTypes.MDBK);
        }

        public int YouguanPosition
        {
            get { return Texts.FindIndex(
                text => text.pe_tag_revised_by_Shuffler == "youguan"); }
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

        public List<Text> TextsBeforeBy
        {
            get
            {
                return Texts.Take(BKByIndexPosition).ToList();
            }
            
        }

        public int GetFirstPASTUnitPositionAfterNN(
            SentenceDecorator bKBySentenceDecorator, int nnPosition)
        {
            return bKBySentenceDecorator.Texts.Skip(nnPosition).ToList().FindIndex(
                                        text => text.IsType(UnitTypes.PAST_Participle));
        }

        public bool ThereIsAnNNUnitBetweenBKByAndMDBK(
            BKBySentenceDecorator bKBySentenceDecorator, int bkByPosition, int mdbkposition)
        {
            return bKBySentenceDecorator
                .Texts
                .Skip(bkByPosition)
                .Take(mdbkposition - bkByPosition)
                .ToList()
                .Any(text => text.IsNN);
        }

        public BKBySentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }
    }
}
