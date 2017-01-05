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

            Assert.That(returnedSentence.pe_para_no, Is.EqualTo(123));
        }

        [Test]
        public void When_DG_Is_Found_Move_TM_before_it()
        {
            // Real GDP PASTrose by DG100 TMper month 

            // Real GPD rose by per month DG100   <= ignore PAST as there is a DG, just swap TM and DG instead
            // Real GDP TMper month DG100 PASTrose by  <= swap TM and DG then move both before PAST
            // Real GDP TMper month PASTrose by DG100  <= Move the TM before DG bringing DG to the end. Then move TM before PAST

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

            
              //  Real  GPD  rose  by  per month  100

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" per month "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" 100 "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));
        }

        //TODO: Timer unit is underlined
    }
}