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

            int bkByPosition = bKBySentenceDecorator.BKByIndexPosition;

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
                // search left in front of By for a PAST until reaching NN
                 var textsPreceedingBy = 
                    GetTextsPreceedingBy(bKBySentenceDecorator);

                if (textsPreceedingBy.Any(text => text.IsType(UnitTypes.NN)))
                {
                    int nnPosition = bKBySentenceDecorator.NNPosition;

                    if (textsPreceedingBy.Skip(nnPosition).Any(
                        text => text.IsType(UnitTypes.PAST_Participle)))
                    {
                        //3.1.Add ‘de’ to before NN1 and underline ‘de’ together with NN1 to form one unit
                        InsertDeParticleAndUnderline(bKBySentenceDecorator, nnPosition);
                        //3.2.Add ‘de’ to after PAST and underline ‘de’ together with PAST to form one unit
                        int pastPosition = textsPreceedingBy.Skip(nnPosition).ToList().FindIndex(
                            text => text.IsType(UnitTypes.PAST_Participle));
                        InsertDeParticleAndUnderline(bKBySentenceDecorator, pastPosition + 1);

                    }
                }

                //Before

                //PREN1An NN1investigation PASTconducted BKby PREN2an NN2expert MD1of PREN3the 
                //NN3bank MD2of PREN4the NN4company MD3into NN5mal - practice VBAwas PASTcompleted BKP.

                //After
                //PREN1An de NN1investigation PASTconducted de BKby PREN2an NN2expert MD1of 
                //PREN3the NN3bank MD2of PREN4the NN4company MD3into NN5mal - practice VBAwas 
                //PASTcompleted BKP.


            }


            return sentence;
        }

        private void InsertDeParticleAndUnderline(object timerSentenceDecorator, int nnPosition)
        {
            throw new System.NotImplementedException();
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
