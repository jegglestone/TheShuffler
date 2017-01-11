using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShufflerLibrary.Strategy
{
    using Model;

    public class BKByUnitStrategy2 : IStrategy
    {
        

        public BKByUnitStrategy2()
        {
            
        }

        public Sentence ShuffleSentence(Sentence sentence)
        {
            //if Not BKby and MDBK and multiple modifiers
            //  return

            /*  before

            PREN1An de NN1investigation PASTconducted de 
            BKby 
            PREN2an 
            NN2expert 
            MD2of the company 
            MD1of PREN3the NN3bank 
            MDBKinto 
            PREN4the operations 
            MD5of PREN6the NN6company 
            MD4of PREN5the NN5department 
            VBAwas PASTcompleted BKP.

            5.3.Move the shuffled MD units before MDBK to before the first NN after ‘BKby’ and then -
            5.4.Move the shuffled MD units after MDBK to immediately after the MDBK word (it’s ‘into’ in the example here)

            After

            PREN1An de NN1investigation PASTconducted de 
            BKby 
            PREN2an 
            MD2of the company 
            MD1of PREN3the NN3bank 
            NN2expert 
            MDBKinto 
            MD5of PREN6the NN6company 
            MD4of PREN5the NN5department 
            PREN4the operations 
            VBAwas PASTcompleted BKP.

            */
            throw new NotImplementedException();
        }
    }
}
