namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class BKByMDBKUnitStrategyTests
    {
        [Test]
        public void When_No_MDBK_Then_Does_Not_Shuffle()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Model.Text>()
                {
                    new Model.Text() { pe_text = "Bk", pe_tag_revised = "BK"},
                    new Model.Text() { pe_text = "NN", pe_tag_revised = "NN"},
                    new Model.Text() { pe_text = "MD3", pe_tag_revised = "MD3"},
                    new Model.Text() { pe_text = "MD2", pe_tag_revised = "MD2"}
                }
            };

            BKByMDBKStrategy bkbyMdbkStrategy = new BKByMDBKStrategy();
            var returnedSentence = bkbyMdbkStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_tag_revised, Is.EqualTo("BK"));
            Assert.That(returnedSentence.Texts[1].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(returnedSentence.Texts[2].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(returnedSentence.Texts[3].pe_tag_revised, Is.EqualTo("MD2"));
        }

        [Test]
        public void Shuffles_MDUnits_BeforeAndAfterMDBK()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Model.Text>()
                {
                    new Model.Text() {pe_tag = "PREN", pe_text = " An ", pe_tag_revised = "PREN1", pe_order = 10},
                    new Model.Text() {pe_tag = "NULL", pe_text = " investigation ", pe_tag_revised = "NN", pe_order = 20},
                    new Model.Text() {pe_tag = "PAST", pe_text = " conducted ", pe_tag_revised = "NULL", pe_order = 30},
                    new Model.Text() {pe_tag = "BK", pe_text = " by ", pe_tag_revised = "NULL", pe_order = 40},
                    new Model.Text() {pe_tag = "PREN", pe_text = " an ", pe_tag_revised = "PREN2", pe_order = 50},
                    new Model.Text() {pe_tag = "NN", pe_text = " expert ", pe_tag_revised = "NULL"},
                    new Model.Text() {pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD1"},
                    new Model.Text() {pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN3"},
                    new Model.Text() {pe_tag = "NN", pe_text = " bank ", pe_tag_revised = "NULL"},
                    new Model.Text() {pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD2"},
                    new Model.Text() {pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN4"},
                    new Model.Text() {pe_tag = "NN", pe_text = " company ", pe_tag_revised = "NULL"},
                    new Model.Text() {pe_tag = "MD", pe_text = " into ", pe_tag_revised = "MD3"},   //MDBK
                    new Model.Text() {pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN5"},
                    new Model.Text() {pe_tag = "TEST", pe_text = " operations ", pe_tag_revised = "NULL"},
                    new Model.Text() {pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD4"},
                    new Model.Text() {pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN6"},
                    new Model.Text() {pe_tag = "NN", pe_text = " department ", pe_tag_revised = "NN6"},
                    new Model.Text() {pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD5"},
                    new Model.Text() {pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN7"},
                    new Model.Text() {pe_tag = "NN", pe_text = " company ", pe_tag_revised = "NN5"},
                    new Model.Text() {pe_tag = "DYN7", pe_text = " was ", pe_tag_revised = "VBA"},
                    new Model.Text() {pe_tag = "PAST", pe_text = " completed ", pe_tag_revised = "NULL"},
                    new Model.Text() {pe_tag = "BKP", pe_text = " . ", pe_tag_revised = "NULL"}
                },
                pe_para_no = 123
            };
            BKByUnitStrategy bkByUnitStrategy = new BKByUnitStrategy();
            var sentenceShuffledByBKByStrategy = bkByUnitStrategy.ShuffleSentence(sentence);

            BKByMDBKStrategy bkbyMdbkStrategy = new BKByMDBKStrategy();
            var returnedSentence = bkbyMdbkStrategy.ShuffleSentence(sentenceShuffledByBKByStrategy);

            /*
             Before

            PREN1An de NN1investigation PASTconducted de 
            BKby PREN2an 
            NN2expert 
            MD2of the company MD1of PREN3the NN3bank 
            MDBKinto PREN4the operations 
            MD5of PREN6the NN6company MD4of PREN5the NN5department 
            VBAwas PASTcompleted BKP.
                	
            5.3.	Move the shuffled MD units before MDBK to before the first NN after ‘BKby’ and then -
            5.4.	Move the shuffled MD units after MDBK to immediately after the MDBK word (it’s ‘into’ in the example here)
            After

            PREN1An de NN1investigation PASTconducted de 
            BKby PREN2an 
            MD2of the company MD1of PREN3the NN3bank 
            NN2expert 
            MDBKinto 
            MD5of PREN6the NN6company MD4of PREN5the NN5department 
            PREN4the operations VBAwas PASTcompleted BKP.

            5.5.	Move PAST + de unit to before the MDBK unit
            After

            PREN1An de NN1investigation 
            BKby PREN2an 
            MD2of the company MD1of PREN3the NN3bank NN2expert 
            PASTconducted de 
            MDBKinto 
            MD5of PREN6the NN6company MD4of PREN5the NN5department 
            PREN4the operations VBAwas PASTcompleted BKP.

            5.6.	Move de + NN1 unit to after MDBK unit or the last MD unit after MDBK 
            if there are any such MD units after MDBK. 
            After

            PREN1An 
            BKby PREN2an 
            MD2of the company MD1of PREN3the NN3bank NN2expert 
            PASTconducted de 
            MDBKinto 
            MD5of PREN6the NN6company MD4of PREN5the NN5department PREN4the operations 
            de NN1investigation 
            VBAwas PASTcompleted BKP.

            
            5.7.	Delete the MDs (both tags and words) and replace MDBK 
            (both tag and word) with ‘youguan’
            After

            PREN1An 
            BKby PREN2an 
            the company PREN3the NN3bank NN2expert 
            PASTconducted de 
            youguan 
            PREN6the NN6company PREN5the NN5department PREN4the operations 
            de NN1investigation 
            VBAwas PASTcompleted BKP.

             */

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" An "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" an "));

            //MD Unit
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" company "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" bank "));

            // NN
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" expert "));

            //PAST + de
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" conducted "));
            Assert.That(returnedSentence.Texts[9].pe_text_revised, Is.EqualTo(" de "));

            //MDBK
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" into "));
            Assert.That(returnedSentence.Texts[10].pe_text_revised, Is.EqualTo(" youguan "));
            Assert.That(returnedSentence.Texts[10].pe_tag_revised_by_Shuffler, Is.EqualTo("youguan"));

            //MD Unit
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo(" company "));
            Assert.That(returnedSentence.Texts[13].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[14].pe_text, Is.EqualTo(" department "));
            Assert.That(returnedSentence.Texts[15].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[16].pe_text, Is.EqualTo(" operations "));

            Assert.That(returnedSentence.Texts[17].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[18].pe_text, Is.EqualTo(" investigation "));

            Assert.That(returnedSentence.Texts[19].pe_text, Is.EqualTo(" was "));
            Assert.That(returnedSentence.Texts[20].pe_text, Is.EqualTo(" completed "));
            Assert.That(returnedSentence.Texts[21].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.pe_para_no, Is.EqualTo(123));
        }

        [Test]
        public void Shuffles_MDUnits_That_are_All_Of_And_Produces_Options()
        {
            /*
            Before

            PRENAn de NN1investigation PASTconducted de 
            BKby 
            PRENan NN2expert 
            MD1of PRENthe NN3bank MD2of PRENthe NN4company 
            MD3of PRENthe NN5operations MD4of DIGtwo NN6departments MD5of PRENthe NN7company 
            VBAwas PASTcompleted BKP.

            After

            PRENAn  
            BKby PRENan NN2expert PASTconducted de
            youguan 
            PRENthe NN3bank PRENthe NN7company MD4of DIGtwo NN6departments PRENthe NN5operations PRENthe NN4company de NN1iknvestigation 
            VBAwas PASTcompleted BKP.
            */

            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "PREN", pe_text = " An "},
                    new Text() { pe_tag_revised = "NN", pe_text = " investigation "},
                    new Text() { pe_tag_revised = "PAST", pe_text = " conducted "},
                    new Text() { pe_tag_revised = "BK", pe_text = " by "},
                    new Text() { pe_tag_revised = "PREN", pe_text = " an "},
                    new Text() { pe_tag_revised = "NN", pe_text = " expert "},
                    new Text() { pe_tag_revised = "MD1", pe_text = " of "},
                    new Text() { pe_tag_revised = "PREN", pe_text = " the "},
                    new Text() { pe_tag_revised = "NN", pe_text = " bank "},
                    new Text() { pe_tag_revised = "MD2", pe_text = " of "},
                    new Text() { pe_tag_revised = "PREN", pe_text = " the "},
                    new Text() { pe_tag_revised = "NN", pe_text = " company "},
                    new Text() { pe_tag_revised = "MD3", pe_text = " of "},
                    new Text() { pe_tag_revised = "PREN", pe_text = " the "},
                    new Text() { pe_tag_revised = "NN", pe_text = " operations "},
                    new Text() { pe_tag_revised = "MD4", pe_text = " of "},
                    new Text() { pe_tag_revised = "DIG", pe_text = " two "},
                    new Text() { pe_tag_revised = "NN", pe_text = " departments "},
                    new Text() { pe_tag_revised = "MD5", pe_text = " of "},
                    new Text() { pe_tag_revised = "PREN", pe_text = " the "},
                    new Text() { pe_tag_revised = "NN", pe_text = " company "},
                    new Text() { pe_tag_revised = "VBA", pe_text = " was "},
                    new Text() { pe_tag_revised = "PAST", pe_text = " completed "},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "}
                }
            };

            BKByUnitStrategy bkByUnitStrategy = new BKByUnitStrategy();
            BKByMDBKStrategy bkbyMdbkStrategy = new BKByMDBKStrategy();

            var returnedSentence = bkByUnitStrategy.ShuffleSentence(sentence);
            //returnedSentence = bkbyMdbkStrategy.ShuffleSentence(returnedSentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" An "));
            Assert.That(returnedSentence.Texts[1].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" investigation "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" conducted "));
            Assert.That(returnedSentence.Texts[4].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" an "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" expert "));
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[8].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" bank "));
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[11].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[13].pe_text, Is.EqualTo(" company "));
            Assert.That(returnedSentence.Texts[14].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[14].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[15].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[16].pe_text, Is.EqualTo(" operations "));
            Assert.That(returnedSentence.Texts[17].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[17].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[18].pe_text, Is.EqualTo(" two "));
            Assert.That(returnedSentence.Texts[19].pe_text, Is.EqualTo(" departments "));
            Assert.That(returnedSentence.Texts[20].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[20].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[21].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[22].pe_text, Is.EqualTo(" company "));
            Assert.That(returnedSentence.Texts[23].pe_text, Is.EqualTo(" was "));
            Assert.That(returnedSentence.Texts[24].pe_text, Is.EqualTo(" completed "));
            Assert.That(returnedSentence.Texts[25].pe_text, Is.EqualTo(" . "));
        }
    }
}
