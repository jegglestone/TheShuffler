namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Model;

    public class BKByUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if(!sentence.HasBKBy)
                return sentence;

            var bKBySentenceDecorator = 
                new BKBySentenceDecorator(sentence);

            int bkByPosition = 
                bKBySentenceDecorator.BKByIndexPosition;

            var bKBYUnit = bKBySentenceDecorator.Texts.GetRange(
                bkByPosition,
                bKBySentenceDecorator.Proceeding_NBKP_BKP_VB_VBA_PAST_position);

            if (bKBySentenceDecorator.HasPRES(bKBYUnit))
            {
                ReplaceBKByWithFangFashi(
                    bKBySentenceDecorator);
            }
            else if (bKBySentenceDecorator.HasNN(bKBYUnit))
            {
                ApplyDeParticles(
                    bKBySentenceDecorator);
            }
            
            return sentence;
        }

        private void ApplyDeParticlesWhereNNThenPASTThenBKBy(BKBySentenceDecorator bKBySentenceDecorator)
        {
            var textsBeforeBy =
                GetTextsPreceedingBy(bKBySentenceDecorator);

            if (NNUnitBeforeBkBy(textsBeforeBy))
            {
                int nnPosition = bKBySentenceDecorator.NNPosition;

                if (IsPASTUnitBetweenNNandBKBy(textsBeforeBy, nnPosition))
                {
                    InsertDeParticleBeforeAndUnderline(
                        bKBySentenceDecorator, nnPosition);

                    int pastPosition =
                        GetFirstPASTUnitPositionAfterNN(bKBySentenceDecorator, nnPosition);

                    InsertDeParticleAfterAndUnderline(bKBySentenceDecorator, pastPosition + 1);
                }
            }
        }

        private static int GetFirstPASTUnitPositionAfterNN(
            SentenceDecorator bKBySentenceDecorator, int nnPosition)
        {
            return bKBySentenceDecorator.Texts.Skip(nnPosition).ToList().FindIndex(
                                        text => text.IsType(UnitTypes.PAST_Participle));
        }

        private static bool IsPASTUnitBetweenNNandBKBy(
            List<Text> textsBeforeBy, int nnPosition)
        {
            return textsBeforeBy.Skip(nnPosition).Any(
                                text => text.IsType(UnitTypes.PAST_Participle));
        }

        private static bool NNUnitBeforeBkBy(
            List<Text> textsBeforeBy)
        {
            return textsBeforeBy.Any(
                            text => text.IsType(UnitTypes.NN));
        }

        private void InsertDeParticleBeforeAndUnderline(
            SentenceDecorator bkBySentenceDecorator, int i)
        {
            bkBySentenceDecorator.Texts.Insert(
                i,
                CreateNewDeParticle(bkBySentenceDecorator.Texts[i-1].pe_order, 1));
            
        }

        private void InsertDeParticleAfterAndUnderline(
            SentenceDecorator bKBySentenceDecorator, int i)
        {
            bKBySentenceDecorator.Texts.Insert(
                i+1,
                CreateNewDeParticle(
                    bKBySentenceDecorator.Texts[i].pe_order, 0));

            bKBySentenceDecorator.Texts[i].pe_merge_ahead = 1;
        }

        private static Text CreateNewDeParticle(int previous_pe_order, int pe_merge_ahead)
        {
            return new Text()
            {
                pe_text_revised = " de "
                , pe_order = previous_pe_order + 5
                , pe_merge_ahead = pe_merge_ahead
            };
        }

        private List<Text> GetTextsPreceedingBy(
            BKBySentenceDecorator bKBySentenceDecorator)
        {
            return bKBySentenceDecorator
                .Texts
                .Take(bKBySentenceDecorator.BKByIndexPosition)
                .ToList();
        }

        private static void ReplaceBKByWithFangFashi(
            BKBySentenceDecorator bKBySentenceDecorator)
        {
            var bkByText = bKBySentenceDecorator.BkByText;
            bkByText.pe_text_revised = " fangfashi ";
        }
    }
}
