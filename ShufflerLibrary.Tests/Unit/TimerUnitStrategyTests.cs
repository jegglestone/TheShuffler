using System.Collections.Generic;

namespace ShufflerLibrary.Tests.Unit
{
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class TimerUnitStrategyTests
    {
        [Test]
        public void WhenNoTimerUnits_DoesNotShuffle()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>
                {
                    new Text(){ pe_text = " He ",          pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " shouted ",     pe_tag="PAST", pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " loudly ",      pe_tag="ADV", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " , ",           pe_tag="NBKP", pe_tag_revised="NULL", pe_order=1664000 },
                    new Text(){ pe_text = " emotionally ", pe_tag="ADV", pe_tag_revised="NULL", pe_order=1664010 },
                    new Text(){ pe_text = " and ",         pe_tag="DYN2", pe_tag_revised="BK", pe_order=1664020 },
                    new Text(){ pe_text = " non-stop ",    pe_tag="ADV", pe_tag_revised="NULL", pe_order=1664030 },
                    new Text(){ pe_text = " . ",           pe_tag="BKP",  pe_tag_revised="NULL", pe_order=1664040 },
                },
                pe_para_no = 123
            };
            var tmUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = tmUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" He "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" shouted "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" loudly "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" , "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" emotionally "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" and "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" non-stop "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenMultipleTimerReverseOrder()
        {
            //Before Shuffling
            //Real GDP PASTrose TM1this time TM2last year BKP.

            var sentence = new Sentence()
            {
                Texts = new List<Text>(){
                    new Text(){ pe_text = " Real ",    pe_tag="ADJ",  pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " GDP ",     pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " rose ",    pe_tag="NULL", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " this ",    pe_tag="PREN", pe_tag_revised="TM1", pe_order=1664000 },
                    new Text(){ pe_text = " time ",    pe_tag="NN",   pe_tag_revised="NULL", pe_order=1664010 },
                    new Text(){ pe_text = " last year ", pe_tag="TM", pe_tag_revised="TM2",   pe_order=1664020 },   
                    new Text(){ pe_text = " . ",         pe_tag="BKP", pe_tag_revised="NULL", pe_order=1664030 },
                },
                pe_para_no = 123
            };

            var tmUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = tmUnitStrategy.ShuffleSentence(sentence);

            //After Shuffling
            //Real GDP PASTrose TM2last year TM1this time BKP.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" last year "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" this "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" time "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.Texts[3].pe_merge_ahead, Is.EqualTo(2));
        }

        [Test]
        public void WhenMultipleTimerReverseOrder_and_VBVBAPAST_Move_Timers_Before_VBVBAPAST()
        {
            //Before Shuffling
            //Real GDP PASTrose TM1this time TM2last year BKP.

            var sentence = new Sentence()
            {
                Texts = new List<Text>(){
                    new Text(){ pe_text = " Real ",    pe_tag="ADJ",  pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " GDP ",     pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " rose ",    pe_tag="PAST", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " this ",    pe_tag="PREN", pe_tag_revised="TM1", pe_order=1664000 },
                    new Text(){ pe_text = " time ",    pe_tag="NN",   pe_tag_revised="NULL", pe_order=1664010 },
                    new Text(){ pe_text = " last year ", pe_tag="TM", pe_tag_revised="TM2",   pe_order=1664020 },
                    new Text(){ pe_text = " . ",         pe_tag="BKP", pe_tag_revised="NULL", pe_order=1664030 },
                },
                pe_para_no = 123
            };

            var tmUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = tmUnitStrategy.ShuffleSentence(sentence);

            //After Shuffling
            //Real GDP TM2last year TM1this time PASTrose BKP.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" last year "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" this "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" time "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.Texts[2].pe_merge_ahead, Is.EqualTo(2));

            Assert.That(returnedSentence.pe_para_no, Is.EqualTo(123));
        }

        [Test]
        public void When_DG_Is_Found_Move_TM_before_it()
        {
            // Real GDP PASTrose by DG100 TMper month 

            
            var sentence = new Sentence()
            {
                Texts = new List<Text>(){
                    new Text(){ pe_text = " Real ",    pe_tag="ADJ",  pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " GDP ",     pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " rose ",    pe_tag="NULL", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " by ",      pe_tag="BK", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " 100 ",    pe_tag="DG",   pe_tag_revised="NULL", pe_order=1664010 },
                    new Text(){ pe_text = " per month ", pe_tag="TM", pe_tag_revised="TM1",   pe_order=1664020 },
                    new Text(){ pe_text = " . ",         pe_tag="BKP", pe_tag_revised="NULL", pe_order=1664030 },
                }
            };

            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            
              //  Real  GPD  rose  by  per month  100

            
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" per month "));
            Assert.That(returnedSentence.Texts[4].pe_merge_ahead, Is.EqualTo(0));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" 100 "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_DG_And_VBVBAPAST_Found_Move_TM_before_DG_Then_TM_Before_VBVBAPAST()
        {
            // Real GDP PASTrose by DG100 TMper month

            var sentence = new Sentence()
            {
                Texts = new List<Text>(){
                    new Text(){ pe_text = " Real ",    pe_tag="ADJ",  pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " GDP ",     pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " rose ",    pe_tag="PAST", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " by ",      pe_tag="BK", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " 100 ",    pe_tag="DG",   pe_tag_revised="NULL", pe_order=1664010 },
                    new Text(){ pe_text = " per month ", pe_tag="TM", pe_tag_revised="TM1",   pe_order=1664020 },
                    new Text(){ pe_text = " . ",         pe_tag="BKP", pe_tag_revised="NULL", pe_order=1664030 },
                }
            };

            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            // Real GDP TMper month PASTrose by DG100  <= Move the TM before DG bringing DG to the end. Then move TM before PAST
            
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" per month "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" 100 "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_Many_Units_To_Shuffle_Items_Dont_Get_Duplicated()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag = "ADJ", pe_text = "Real"},
                    new Text() { pe_tag = "NN", pe_text = "gross domestic product "},
                    new Text() { pe_tag = null, pe_text = "( "},
                    new Text() { pe_tag = "NN", pe_text = "(GDP) "},
                    new Text() { pe_tag = "", pe_text = "~)"},
                    new Text() { pe_tag = "PAST", pe_text = "rose"},
                    new Text() { pe_tag = "MD1", pe_text = "at"},
                    new Text() { pe_tag = "PREN1", pe_text = "an"},
                    new Text() { pe_tag = "NN", pe_text = "annual rate"},
                    new Text() { pe_tag = "MD2", pe_text = " of "},
                    new Text() { pe_tag = "PREN2", pe_text = "about"},
                    new Text() { pe_tag = "DG", pe_text = "2"},
                    new Text() { pe_tag = "NN", pe_text = "percent"},
                    new Text() { pe_tag = "MD3", pe_text = "in"},
                    new Text() { pe_tag = "PREN3", pe_text = "the"},
                    new Text() { pe_tag = "TM1", pe_text = "first quarter"},
                    new Text() { pe_tag = "CS", pe_text = "after"},
                    new Text() { pe_tag = "PRES", pe_text = "increasing"},
                    new Text() { pe_tag = "MD4", pe_text = "at"},
                    new Text() { pe_tag = "PREN4", pe_text = "a"},
                    new Text() { pe_tag = "DG", pe_text = "3"},
                    new Text() { pe_tag = "NN", pe_text = "percent"},
                    new Text() { pe_tag = "NN", pe_text = "pace"},
                    new Text() { pe_tag = "MD5", pe_text = "in"},
                    new Text() { pe_tag = "PREN5", pe_text = "the"},
                    new Text() { pe_tag = "TM2", pe_text = "fourth quarter"},
                    new Text() { pe_tag = "MD6", pe_text = " of "},
                    new Text() { pe_tag = "TMY", pe_text = "2011"},
                    new Text() { pe_tag = "PY", pe_text = "zhihou"},
                    new Text() { pe_tag = "BKP", pe_text = " . "}
                }
            };
            TimerUnitStrategy timerUnitStrategy = new TimerUnitStrategy();
            timerUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.TextCount, Is.EqualTo(30));
            Assert.That(sentence.Texts[29].pe_text == " . ");
        }
    }
}