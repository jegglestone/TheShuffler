namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class ByUnitStrategyTests
    {
        [Test]
        public void When_No_BK_by_does_Not_Shuffle()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_text =" They ",     pe_tag = "NN", pe_tag_revised = "NULL" },
                    new Text() { pe_text =" improved ", pe_tag = "PAST", pe_tag_revised = "NULL" },
                    new Text() { pe_text =" through ",       pe_tag = "NN", pe_tag_revised= "NULL" },
                    new Text() { pe_text =" slowing ", pe_tag = "DYN5", pe_tag_revised= "NN" },
                    new Text() { pe_text =" down ",    pe_tag = "ADV", pe_tag_revised= "ADV1" },
                    new Text() { pe_text =" . ",       pe_tag = "BKP", pe_tag_revised= "" }
                },
                pe_para_no = 456
            };

            var byUnitStrategy = new BKByUnitStrategy();
            var returnedSentence = byUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" They "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" improved "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" through "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" slowing "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" down "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.pe_para_no, Is.EqualTo(456));
            Assert.That(returnedSentence.Sentence_No, Is.EqualTo(1));
            Assert.That(returnedSentence.Sentence_Option_Selected, Is.EqualTo(1));
        }

        [Test]
        public void When_PRES_Betwwen_By_And_NBKP_BKP_VB_VBA_PAST_Replace_With_fangfashi()
        {
            var sentence = new Sentence()
            {
                //They PASTimproved BKby PRESslowing down BKP.
                //They PASTimproved fangfashi PRESslowing down BKP.

                Texts = new List<Text>()
                {
                    new Text() { pe_text =" They ",     pe_tag = "NN", pe_tag_revised = "NULL" },
                    new Text() { pe_text =" improved ", pe_tag = "PAST", pe_tag_revised = "NULL" },
                    new Text() { pe_text =" by ",       pe_tag = "BK", pe_tag_revised= "NULL" },
                    new Text() { pe_text =" slowing ", pe_tag = "DYN5", pe_tag_revised= "PRES" },
                    new Text() { pe_text =" down ",    pe_tag = "ADV", pe_tag_revised= "ADV1" },
                    new Text() { pe_text =" . ",       pe_tag = "BKP", pe_tag_revised= "NULL" }
                },
                pe_para_no = 456
            };

            var bkByUnitStrategy = new BKByUnitStrategy();
            var returnedSentence = bkByUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" They "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" improved "));
            Assert.That(returnedSentence.Texts[2].pe_text_revised, Is.EqualTo(" fangfashi "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" slowing "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" down "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.pe_para_no, Is.EqualTo(456));
        }

        [Test]
        public void When_NN_Between_By_And_NBKP_BKP_VB_VBA_PAST_And_BY_preceeded_by_PAST_Add_de_before_NN_and_after_PAST()
        {
            //3.If instead, NN is found, search left for PAST until reaching NN.If found –
            //3.1.Add ‘de’ to before NN1 and underline ‘de’ together with NN1 to form one unit
            //3.2.Add ‘de’ to after PAST and underline ‘de’ together with PAST to form one unit
            /*
             
            Before

            PREN1An NN1investigation PASTconducted BKby PREN2an NN2expert MD1of PREN3the NN3bank 
            MD2of PREN4the NN4company MD3into NN5mal-practice VBAwas PASTcompleted BKP.

            After

            PREN1An de NN1investigation PASTconducted de BKby PREN2an NN2expert MD1of PREN3the 
            NN3bank MD2of PREN4the NN4company MD3into NN5mal - practice VBAwas PASTcompleted BKP.

            */
      
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag = "PREN", pe_text = " An ", pe_tag_revised = "PREN1", pe_order = 10},
                    new Text() { pe_tag = "NULL", pe_text = " investigation ", pe_tag_revised = "NN", pe_order = 20},
                    new Text() { pe_tag = "PAST", pe_text = " conducted ", pe_tag_revised = "NULL", pe_order = 30},
                    new Text() { pe_tag = "BK", pe_text = " by ", pe_tag_revised = "NULL", pe_order = 40},
                    new Text() { pe_tag = "PREN", pe_text = " an ", pe_tag_revised = "PREN2", pe_order = 50},
                    new Text() { pe_tag = "NN", pe_text = " expert ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD1"},
                    new Text() { pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN3"},
                    new Text() { pe_tag = "NN", pe_text = " bank ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD2"},
                    new Text() { pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN4"},
                    new Text() { pe_tag = "NN", pe_text = " company ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " into ", pe_tag_revised = "MD3"},
                    new Text() { pe_tag = "NN", pe_text = " mal-practice ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "DYN7", pe_text = " was ", pe_tag_revised = "VB"},
                    new Text() { pe_tag = "PAST", pe_text = " completed ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "BKP", pe_text = " . ", pe_tag_revised = "NULL"}
                },
                pe_para_no = 123
            };

            var bkByUnitStrategy = new BKByUnitStrategy();
            var returnedSentence = bkByUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" An "));

            Assert.That(returnedSentence.Texts[1].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[1].pe_order, Is.EqualTo(15));
            Assert.That(returnedSentence.Texts[1].pe_merge_ahead, Is.EqualTo(1));

            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" investigation "));
            Assert.That(returnedSentence.Texts[2].pe_order, Is.EqualTo(20));

            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" conducted "));
            Assert.That(returnedSentence.Texts[3].pe_merge_ahead, Is.EqualTo(1));
            Assert.That(returnedSentence.Texts[3].pe_order, Is.EqualTo(30));

            Assert.That(returnedSentence.Texts[4].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[4].pe_order, Is.EqualTo(35));

            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" an "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" expert "));

            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" company "));

            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[13].pe_text, Is.EqualTo(" bank "));

            Assert.That(returnedSentence.Texts[14].pe_text, Is.EqualTo(" into "));
            Assert.That(returnedSentence.Texts[14].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));

            Assert.That(returnedSentence.Texts[15].pe_text, Is.EqualTo(" mal-practice "));
            Assert.That(returnedSentence.Texts[16].pe_text, Is.EqualTo(" was "));
            Assert.That(returnedSentence.Texts[17].pe_text, Is.EqualTo(" completed "));
            Assert.That(returnedSentence.Texts[18].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.pe_para_no, Is.EqualTo(123));
        }

        [Test]
        public void When_MBKY_Then_MD_Units_Before_And_After_Are_Shuffled()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag = "PREN", pe_text = " An ", pe_tag_revised = "PREN1", pe_order = 10},
                    new Text() { pe_tag = "NULL", pe_text = " investigation ", pe_tag_revised = "NN", pe_order = 20},
                    new Text() { pe_tag = "PAST", pe_text = " conducted ", pe_tag_revised = "NULL", pe_order = 30},
                    new Text() { pe_tag = "BK", pe_text = " by ", pe_tag_revised = "NULL", pe_order = 40},
                    new Text() { pe_tag = "PREN", pe_text = " an ", pe_tag_revised = "PREN2", pe_order = 50},
                    new Text() { pe_tag = "NN", pe_text = " expert ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD1"},
                    new Text() { pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN3"},
                    new Text() { pe_tag = "NN", pe_text = " bank ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD2"},
                    new Text() { pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN4"},
                    new Text() { pe_tag = "NN", pe_text = " company ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " into ", pe_tag_revised = "MD3"},
                    new Text() { pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN5" },
                    new Text() { pe_tag = "TEST", pe_text = " operations ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "MD", pe_text = " of ", pe_tag_revised =  "MD4" },
                    new Text() { pe_tag = "PREN", pe_text = " the ",pe_tag_revised = "PREN6"},
                    new Text() { pe_tag = "NN", pe_text = " department ", pe_tag_revised = "NN6"},
                    new Text() { pe_tag = "MD", pe_text = " of ", pe_tag_revised = "MD5"},
                    new Text() { pe_tag = "PREN", pe_text = " the ", pe_tag_revised = "PREN7"},
                    new Text() { pe_tag = "NN", pe_text = " company ",  pe_tag_revised = "NN5" },
                    new Text() { pe_tag = "DYN7", pe_text = " was ", pe_tag_revised = "VBA"},
                    new Text() { pe_tag = "PAST", pe_text = " completed ", pe_tag_revised = "NULL"},
                    new Text() { pe_tag = "BKP", pe_text = " . ", pe_tag_revised = "NULL"}
                }
            };
            BKByUnitStrategy bkByUnitStrategy = new BKByUnitStrategy();
            var returnedSentence = bkByUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" An "));

            Assert.That(returnedSentence.Texts[1].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[1].pe_order, Is.EqualTo(15));
            Assert.That(returnedSentence.Texts[1].pe_merge_ahead, Is.EqualTo(1));

            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" investigation "));
            Assert.That(returnedSentence.Texts[2].pe_order, Is.EqualTo(20));

            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" conducted "));
            Assert.That(returnedSentence.Texts[3].pe_merge_ahead, Is.EqualTo(1));
            Assert.That(returnedSentence.Texts[3].pe_order, Is.EqualTo(30));

            Assert.That(returnedSentence.Texts[4].pe_text_revised, Is.EqualTo(" de "));
            Assert.That(returnedSentence.Texts[4].pe_order, Is.EqualTo(35));

            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" an "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" expert "));

            Assert.That(returnedSentence.Texts[8].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" of "));

            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" company "));

            Assert.That(returnedSentence.Texts[11].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo(" of "));

            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[13].pe_text, Is.EqualTo(" bank "));

            Assert.That(returnedSentence.Texts[14].pe_text, Is.EqualTo(" into "));
            Assert.That(returnedSentence.Texts[14].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));

            Assert.That(returnedSentence.Texts[15].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[16].pe_text, Is.EqualTo(" operations "));

            Assert.That(returnedSentence.Texts[17].pe_tag_revised, Is.EqualTo("MD5"));
            Assert.That(returnedSentence.Texts[17].pe_text, Is.EqualTo(" of "));
            Assert.That(returnedSentence.Texts[18].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[19].pe_text, Is.EqualTo(" company "));

            Assert.That(returnedSentence.Texts[20].pe_tag_revised, Is.EqualTo("MD4"));
            Assert.That(returnedSentence.Texts[20].pe_text, Is.EqualTo(" of "));

            Assert.That(returnedSentence.Texts[21].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[22].pe_text, Is.EqualTo(" department "));
            Assert.That(returnedSentence.Texts[23].pe_text, Is.EqualTo(" was "));
            Assert.That(returnedSentence.Texts[24].pe_text, Is.EqualTo(" completed "));
            Assert.That(returnedSentence.Texts[25].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_All_MD_Units_After_BkBy_Are_Of_Replace_All_With_MDBK()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_text = "And", pe_tag = "" },
                    new Text() { pe_text = " by ", pe_tag = "BK" },
                    new Text() { pe_text = "then", pe_tag = "" },
                    new Text() { pe_text = "three", pe_tag = "" },
                    new Text() { pe_text = " of ", pe_tag = "MD1" },
                    new Text() { pe_text = "the", pe_tag = "" },
                    new Text() { pe_text = "people", pe_tag = "" },
                    new Text() { pe_text = " of ", pe_tag = "MD2" },
                    new Text() { pe_text = "the", pe_tag = "" },
                    new Text() { pe_text = "house", pe_tag = "" },
                    new Text() { pe_text = " of ", pe_tag = "MD3" },
                    new Text() { pe_text = "commons", pe_tag = "" },
                    new Text() { pe_text = "complained", pe_tag = "PAST" },
                    new Text() { pe_text = " . ", pe_tag = "BKP" },
                }
            };
            
            var bkByUnitStrategy = new BKByUnitStrategy();
            var returnedSentence = bkByUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("And"));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo("then"));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo("three"));

            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" of ")); //MD1
            Assert.That(returnedSentence.Texts[4].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo("the"));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo("people"));

            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" of ")); //MD2
            Assert.That(returnedSentence.Texts[7].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));

            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo("the"));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo("house"));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" of ")); //MD3
            Assert.That(returnedSentence.Texts[10].pe_tag_revised_by_Shuffler, Is.EqualTo("MDBK"));
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo("commons"));

            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo("complained"));
            Assert.That(returnedSentence.Texts[13].pe_text, Is.EqualTo(" . "));
        }
    }
}
