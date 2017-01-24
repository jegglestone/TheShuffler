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
                    new Text() {pe_text = " He ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1663970},
                    new Text() {pe_text = " shouted ", pe_tag = "PAST", pe_tag_revised = "NULL", pe_order = 1663980},
                    new Text() {pe_text = " loudly ", pe_tag = "ADV", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " , ", pe_tag = "NBKP", pe_tag_revised = "NULL", pe_order = 1664000},
                    new Text() {pe_text = " emotionally ", pe_tag = "ADV", pe_tag_revised = "NULL", pe_order = 1664010},
                    new Text() {pe_text = " and ", pe_tag = "DYN2", pe_tag_revised = "BK", pe_order = 1664020},
                    new Text() {pe_text = " non-stop ", pe_tag = "ADV", pe_tag_revised = "NULL", pe_order = 1664030},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664040},
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
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " Real ", pe_tag = "ADJ", pe_tag_revised = "NULL", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1663980},
                    new Text() {pe_text = " rose ", pe_tag = "NULL", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " this ", pe_tag = "PREN", pe_tag_revised = "TM1", pe_order = 1664000},
                    new Text() {pe_text = " time ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1664010},
                    new Text() {pe_text = " last year ", pe_tag = "TM", pe_tag_revised = "TM2", pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
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
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " Real ", pe_tag = "ADJ", pe_tag_revised = "NULL", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1663980},
                    new Text() {pe_text = " rose ", pe_tag = "PAST", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " this ", pe_tag = "PREN", pe_tag_revised = "TM1", pe_order = 1664000},
                    new Text() {pe_text = " time ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1664010},
                    new Text() {pe_text = " last year ", pe_tag = "TM", pe_tag_revised = "TM2", pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
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
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " Real ", pe_tag = "ADJ", pe_tag_revised = "NULL", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1663980},
                    new Text() {pe_text = " rose ", pe_tag = "NULL", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " by ", pe_tag = "BK", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " 100 ", pe_tag = "DG", pe_tag_revised = "NULL", pe_order = 1664010},
                    new Text() {pe_text = " per month ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
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

        [Test]
        public void When_DG_And_VBVBAPAST_Found_Move_TM_before_DG_Then_TM_Before_VBVBAPAST()
        {
            // Real GDP PASTrose by DG100 TMper month
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " Real ", pe_tag = "ADJ", pe_tag_revised = "NULL", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "NN", pe_tag_revised = "NULL", pe_order = 1663980},
                    new Text() {pe_text = " rose ", pe_tag = "PAST", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " by ", pe_tag = "BK", pe_tag_revised = "NULL", pe_order = 1663990},
                    new Text() {pe_text = " 100 ", pe_tag = "DG", pe_tag_revised = "NULL", pe_order = 1664010},
                    new Text() {pe_text = " per month ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
                }
            };

            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            // Real GDP TMper month PASTrose by DG100  <= Move the TM before DG bringing DG to the end. Then move TM before PAST

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" per month "));
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
                    new Text() {pe_tag = "ADJ", pe_text = "Real"},
                    new Text() {pe_tag = "NN", pe_text = "gross domestic product "},
                    new Text() {pe_tag = null, pe_text = "( "},
                    new Text() {pe_tag = "NN", pe_text = "(GDP) "},
                    new Text() {pe_tag = "", pe_text = "~)"},
                    new Text() {pe_tag = "PAST", pe_text = "rose"},
                    new Text() {pe_tag = "MD1", pe_text = "at"},
                    new Text() {pe_tag = "PREN1", pe_text = "an"},
                    new Text() {pe_tag = "NN", pe_text = "annual rate"},
                    new Text() {pe_tag = "MD2", pe_text = " of "},
                    new Text() {pe_tag = "PREN2", pe_text = "about"},
                    new Text() {pe_tag = "DG", pe_text = "2"},
                    new Text() {pe_tag = "NN", pe_text = "percent"},
                    new Text() {pe_tag = "MD3", pe_text = "in"},
                    new Text() {pe_tag = "PREN3", pe_text = "the"},
                    new Text() {pe_tag = "TM1", pe_text = "first quarter"},
                    new Text() {pe_tag = "CS", pe_text = "after"},
                    new Text() {pe_tag = "PRES", pe_text = "increasing"},
                    new Text() {pe_tag = "MD4", pe_text = "at"},
                    new Text() {pe_tag = "PREN4", pe_text = "a"},
                    new Text() {pe_tag = "DG", pe_text = "3"},
                    new Text() {pe_tag = "NN", pe_text = "percent"},
                    new Text() {pe_tag = "NN", pe_text = "pace"},
                    new Text() {pe_tag = "MD5", pe_text = "in"},
                    new Text() {pe_tag = "PREN5", pe_text = "the"},
                    new Text() {pe_tag = "TM2", pe_text = "fourth quarter"},
                    new Text() {pe_tag = "MD6", pe_text = " of "},
                    new Text() {pe_tag = "TMY", pe_text = "2011"},
                    new Text() {pe_tag = "PY", pe_text = "zhihou"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };
            TimerUnitStrategy timerUnitStrategy = new TimerUnitStrategy();
            timerUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.TextCount, Is.EqualTo(30));
            Assert.That(sentence.Texts[29].pe_text == " . ");
        }

        [Test]
        public void TimerUnitIsStoppedAtTheFirstBKPNotTheLast()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "BK", pe_text = "if"},
                    new Text() {pe_tag = "ADJ", pe_text = "so"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "PREN1", pe_text = "the"},
                    new Text() {pe_tag = "NN", pe_text = "deceleration"},
                    new Text() {pe_tag = "MD1", pe_text = "in"},
                    new Text() {pe_tag = "N", pe_text = "employment"},
                    new Text() {pe_tag = "MD2", pe_text = "in"},
                    new Text() {pe_tag = "TM1", pe_text = "recent months"}, // Timer start 8
                    new Text() {pe_tag = "TM2", pe_text = "may"},
                    new Text() {pe_tag = "VB", pe_text = "indicate"},
                    new Text() {pe_tag = "NUL", pe_text = "that"},
                    new Text() {pe_tag = "PREN2", pe_text = "this"},
                    new Text() {pe_tag = "NN", pe_text = "catch-up"},
                    new Text() {pe_tag = "VB", pe_text = "has largely been completed"},
                    new Text() {pe_tag = "BKP", pe_text = " , "}, // End - BKP
                    new Text() {pe_tag = "BK", pe_text = "and"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "ADV1", pe_text = "consequently"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "NUL", pe_text = "that"},
                    new Text() {pe_tag = null, pe_text = "more-rapid"},
                    new Text() {pe_tag = "NN", pe_text = "gains"},
                    new Text() {pe_tag = "MD1", pe_text = "in"},
                    new Text() {pe_tag = "ADJ", pe_text = "economic"},
                    new Text() {pe_tag = "NN", pe_text = "activity"},
                    new Text() {pe_tag = "VB", pe_text = "will be required"},
                    new Text() {pe_tag = "MD1", pe_text = "to"},
                    new Text() {pe_tag = "VB", pe_text = "achieve"},
                    new Text() {pe_tag = "ADJ", pe_text = "significant"},
                    new Text() {pe_tag = "ADJ", pe_text = "further"},
                    new Text() {pe_tag = "NN", pe_text = "improvement"},
                    new Text() {pe_tag = "MD1", pe_text = "in"},
                    new Text() {pe_tag = "NN", pe_text = "labor market"},
                    new Text() {pe_tag = "NN", pe_text = "conditions"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            TimerUnitStrategy timerUnitStrategy = new TimerUnitStrategy();
            sentence = timerUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text == "if");
            Assert.That(sentence.Texts[1].pe_text == "so");
            Assert.That(sentence.Texts[2].pe_text == " , ");
            Assert.That(sentence.Texts[3].pe_text == "the");
            Assert.That(sentence.Texts[4].pe_text == "deceleration");
            Assert.That(sentence.Texts[5].pe_text == "in");
            Assert.That(sentence.Texts[6].pe_text == "employment");
            Assert.That(sentence.Texts[7].pe_text == "in");

            Assert.That(sentence.Texts[8].pe_text == "may"); //new timer start
            Assert.That(sentence.Texts[9].pe_text == "indicate");
            Assert.That(sentence.Texts[10].pe_text == "that");
            Assert.That(sentence.Texts[11].pe_text == "this");
            Assert.That(sentence.Texts[12].pe_text == "catch-up");
            Assert.That(sentence.Texts[13].pe_text == "has largely been completed");
            Assert.That(sentence.Texts[14].pe_text == "recent months"); // Timer start 8
            Assert.That(sentence.Texts[15].pe_text == " , "); // End - BKP

            Assert.That(sentence.Texts[16].pe_text == "and");
            Assert.That(sentence.Texts[17].pe_text == " , ");
            Assert.That(sentence.Texts[18].pe_text == "consequently");
            Assert.That(sentence.Texts[19].pe_text == " , ");
            Assert.That(sentence.Texts[20].pe_text == "that");
            Assert.That(sentence.Texts[21].pe_text == "more-rapid");
            Assert.That(sentence.Texts[22].pe_text == "gains");
            Assert.That(sentence.Texts[23].pe_text == "in");
            Assert.That(sentence.Texts[24].pe_text == "economic");
            Assert.That(sentence.Texts[25].pe_text == "activity");
            Assert.That(sentence.Texts[26].pe_text == "will be required");
            Assert.That(sentence.Texts[27].pe_text == "to");
            Assert.That(sentence.Texts[28].pe_text == "achieve");
            Assert.That(sentence.Texts[29].pe_text == "significant");
            Assert.That(sentence.Texts[30].pe_text == "further");
            Assert.That(sentence.Texts[31].pe_text == "improvement");
            Assert.That(sentence.Texts[32].pe_text == "in");
            Assert.That(sentence.Texts[33].pe_text == "labor market");
            Assert.That(sentence.Texts[34].pe_text == "conditions");
            Assert.That(sentence.Texts[35].pe_text == " . ");
        }

        [Test]
        public void When_DG_And_PAST_Shuffles_Keeps_DGAfter_TImer()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "ADJ", pe_text = "real"},
                    new Text() {pe_tag = "NN", pe_text = " gross domestic product "},
                    new Text() {pe_tag = null, pe_text = "("},
                    new Text() {pe_tag = "NN", pe_text = "(gdp),"},
                    new Text() {pe_tag = null, pe_text = " ),"},
                    new Text() {pe_tag = "PAST", pe_text = "rose"}, // first and only VbVbaPast - 5
                    new Text() {pe_tag = "MD1", pe_text = " at "},
                    new Text() {pe_tag = "PREN1", pe_text = "an"},
                    new Text() {pe_tag = "NN", pe_text = "annual rate "},
                    new Text() {pe_tag = "MD2", pe_text = " of "},
                    new Text() {pe_tag = "PREN2", pe_text = "about"},
                    new Text() {pe_tag = "DG", pe_text = "2"}, // DG 
                    new Text() {pe_tag = "NN", pe_text = "percent"},
                    new Text() {pe_tag = "MD3", pe_text = "in"},
                    new Text() {pe_tag = "PREN3", pe_text = "the"},
                    new Text() {pe_tag = "TM1", pe_text = "first quarter"}, // timer unit start - 15
                    new Text() {pe_tag = "CS", pe_text = "after"},
                    new Text() {pe_tag = "PRES", pe_text = "increasing"},
                    new Text() {pe_tag = "MD4", pe_text = "at"},
                    new Text() {pe_tag = "PREN4", pe_text = "a"},
                    new Text() {pe_tag = "DG", pe_text = "3"},
                    new Text() {pe_tag = "NN", pe_text = "percent"},
                    new Text() {pe_tag = "NN", pe_text = "pace"},
                    new Text() {pe_tag = "MD5", pe_text = "in"},
                    new Text() {pe_tag = "PREN5", pe_text = "the"},
                    new Text() {pe_tag = "TM2", pe_text = "fourth quarter"}, // last timer - 25
                    new Text() {pe_tag = "MD6", pe_text = " of "},
                    new Text() {pe_tag = "TMY", pe_text = "2011"}, // end pos before BKP -27
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            sentence = timerUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[sentence.TextCount - 1].pe_text, Is.EqualTo(" . "));
            Assert.That(sentence.TextCount, Is.EqualTo(29));

            // reversed timer goes before DG
            Assert.That(sentence.Texts[11].pe_tag == "TM2");
            Assert.That(sentence.Texts[24].pe_tag == "DG");
        }
    }
}