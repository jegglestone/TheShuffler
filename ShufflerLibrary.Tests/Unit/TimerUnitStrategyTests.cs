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
                    new Text() {pe_text = " Real ", pe_tag = "", pe_tag_revised = "ADJ", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "", pe_tag_revised = "NULL", pe_order = 1663980},
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
        public void When_DIG_Is_Found_Move_TM_before_it()
        {
            // Real GDP PASTrose by DIG100 pence TMper month
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " Real ", pe_tag = "ADJ", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "NN", pe_order = 1663980},
                    new Text() {pe_text = " rose ", pe_tag_revised = "PAST", pe_order = 1663990},
                    new Text() {pe_text = " by ", pe_tag = "BK", pe_order = 1663990},
                    new Text() {pe_text = " 100 ", pe_tag = "DIG",  pe_order = 1664010},
                    new Text() {pe_text = " pence ", pe_tag = null,pe_order = 1664010},

                    new Text() {pe_text = " per month ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
                }
            };

            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            // per month Real  GPD  rose  by per month 100 pence
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" Real ")); //TM
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" GDP "));  //ADJ
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" per month "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" 100 ")); //DG
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" pence "));
        
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenAdvBeforeVbVbaPastThatIsbeforeTimer_MoveTMbeforeAdv()
        {
            // Bef: He gladly accepted the offer TM1last week.
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " He ", pe_tag = "", pe_tag_revised = "", pe_order = 1663970},
                    new Text() {pe_text = " gladly ", pe_tag = "ADV", pe_tag_revised = "ADV", pe_order = 1663980},
                    new Text() {pe_text = " accepted ", pe_tag = "PAST", pe_tag_revised = null, pe_order = 1663990},
                    new Text() {pe_text = " the ", pe_tag = "", pe_tag_revised = "", pe_order = 1663990},
                    new Text() {pe_text = " offer ", pe_tag = "", pe_tag_revised = "", pe_order = 1664010},
                    new Text() {pe_text = " last ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664010, pe_merge_ahead = 1},
                    new Text() {pe_text = " week ", pe_tag = null, pe_tag_revised = null, pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
                }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            //Aft: He TM1last week gladly accepted the offer.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" He "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" last ")); //TM
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" week "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" gladly ")); //ADV
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" accepted "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" offer "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" . "));

        }

        [Test]
        public void WhenNoAdverbBeforeTimer_MoveBeforeVbVbaPast()
        {
            //Bef: He PASTaccepted the offer TM1last week.
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " He ", pe_tag = "", pe_tag_revised = "", pe_order = 1663970},
                    new Text() {pe_text = " accepted ", pe_tag = "PAST", pe_tag_revised = "PAST", pe_order = 1663990},
                    new Text() {pe_text = " the ", pe_tag = "", pe_tag_revised = "", pe_order = 1663990},
                    new Text() {pe_text = " offer ", pe_tag = "", pe_tag_revised = "", pe_order = 1664010},
                    new Text() {pe_text = " last ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664010, pe_merge_ahead = 1},
                    new Text() {pe_text = " week ", pe_tag = null, pe_tag_revised = null, pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
                }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);


            //Aft: He TM1last week accepted the offer.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" He "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" last ")); //TM
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" week "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" accepted "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" offer "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenPrenBeforeNNBeforeTM_MoveTMBeforePren()
        {
            //He went out and the warm weather TM1this past winter was poor
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                    {
                        new Text() {pe_text = "He", pe_tag = null, pe_tag_revised = null, pe_order = 1663970},
                        new Text() {pe_text = "went", pe_tag = null, pe_tag_revised = null, pe_order = 1663980},
                        new Text() {pe_text = "out", pe_tag = null, pe_tag_revised = null, pe_order = 1663990},
                        new Text() {pe_text = "and", pe_tag = null, pe_tag_revised = null, pe_order = 1664000},
                        new Text() {pe_text = "the", pe_tag = "PREN", pe_tag_revised = null, pe_order = 1664010},
                        new Text() {pe_text = "warm", pe_tag = null, pe_tag_revised = null, pe_order = 1664020},
                        new Text() {pe_text = "weather", pe_tag = "NN", pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "this", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664030, pe_merge_ahead=2},
                        new Text() {pe_text = "past", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "winter", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "was", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "poor", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = null, pe_order = 1664030}
                    }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            // He went out and TM1this past winter the warm weather was poor
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("He"));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo("went"));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo("out"));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo("and"));  //and
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo("this")); //TM1
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo("past"));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo("winter"));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo("was")); //PREN
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo("poor"));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo("the"));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo("warm"));
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo("weather"));
            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo(" . "));
        }

        [Test, Ignore]
        public void WhenADJBeforeNNBeforeTM_MoveTMBeforePren()
        {

            //Bef: …VBup from ADJabout 150, 000 jobs TMper month… 
             
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                    {
                        new Text() {pe_text = "Up", pe_tag = null, pe_tag_revised = null, pe_order = 1663970},
                        new Text() {pe_text = "from", pe_tag = null, pe_tag_revised = null, pe_order = 1663980},
                        new Text() {pe_text = "about", pe_tag = "ADJ", pe_tag_revised = null, pe_order = 1663990},
                        new Text() {pe_text = "150,000", pe_tag = "DIG", pe_tag_revised = null, pe_order = 1664000},
                        new Text() {pe_text = "jobs", pe_tag = "NN", pe_tag_revised = null, pe_order = 1664010},
                        new Text() {pe_text = "per month", pe_tag = "TM", pe_tag_revised = null, pe_order = 1664020},
                        new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = null, pe_order = 1664030},

                    }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);


            //Actual: VBup from ADJabout TMper month 150,000 NNjobs   - 3.1 Move timer before Dig

            //Aft: …  VBup from TMper month ADJabout 150, 000 jobs …
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("Up")); 
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo("from"));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo("per month")); //TM
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo("about"));  //ADJ
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo("150,000")); //TM1
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo("jobs"));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenNnAndTM_MoveTmBeforeNn()
        {
            //Bef: NNGrowth TM1last quarter
            //Aft: TM1last quarter NNGrowth
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                    {
                        new Text() {pe_text = "Growth", pe_tag = "NN", pe_tag_revised = null, pe_order = 1663970},
                        new Text() {pe_text = "last quarter", pe_tag = "TM", pe_tag_revised = null, pe_order = 1663980},
                        new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = null, pe_order = 1664030},
                }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("last quarter"));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo("Growth"));
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
        }
    }

    [TestFixture]
    public class TimerUnitStrategyLongerSentenceTests
    {
        [Test]
        public void WhenMultipleTimerReverseOrder()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " However ", pe_tag = "CS", pe_tag_revised = null, pe_order = 1663930},
                    new Text() {pe_text = " it ", pe_tag = "NN", pe_tag_revised = null, pe_order = 1663940},
                    new Text() {pe_text = " was ", pe_tag = "PAST", pe_tag_revised = null, pe_order = 1663950},
                    new Text() {pe_text = " , ", pe_tag = "BKP", pe_tag_revised = null, pe_order = 1663950},


                    new Text() {pe_text = " Real ", pe_tag = "", pe_tag_revised = "ADJ", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "", pe_tag_revised = "NULL", pe_order = 1663980},
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

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" However "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" it "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" was "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" , "));

            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" Real "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" GDP "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" last year "));
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" this "));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo(" time "));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenAdvBeforeVbVbaPastThatIsbeforeTimer_MoveTMbeforeAdv()
        {
            // Bef: He gladly accepted the offer TM1last week.
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = " And ", pe_tag = "", pe_order = 1663970},
                    new Text() {pe_text = " by ", pe_tag = "BK",  pe_order = 1663980},
                    new Text() {pe_text = " that ", pe_tag = "",  pe_order = 1663990},
                    new Text() {pe_text = " time ", pe_tag = "", pe_order = 1663990},


                    new Text() {pe_text = " he ", pe_tag = "", pe_tag_revised = "", pe_order = 1663970},
                    new Text() {pe_text = " gladly ", pe_tag = "ADV", pe_tag_revised = "ADV", pe_order = 1663980},
                    new Text() {pe_text = " accepted ", pe_tag = "PAST", pe_tag_revised = null, pe_order = 1663990},
                    new Text() {pe_text = " the ", pe_tag = "", pe_tag_revised = "", pe_order = 1663990},
                    new Text() {pe_text = " offer ", pe_tag = "", pe_tag_revised = "", pe_order = 1664010},
                    new Text() {pe_text = " last ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664010, pe_merge_ahead = 1},
                    new Text() {pe_text = " week ", pe_tag = null, pe_tag_revised = null, pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
                }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            //Aft: He TM1last week gladly accepted the offer.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" And "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" by ")); //TM
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" that "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" time ")); //ADV

            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" he "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" last ")); //TM
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" week "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" gladly ")); //ADV
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" accepted "));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo(" the "));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" offer "));
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenNnAndTM_MoveTmBeforeNn()
        {
            //Bef: NNGrowth TM1last quarter
            //Aft: TM1last quarter NNGrowth
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                    {
                        new Text() {pe_text = "However", pe_tag = "CS", pe_order = 1663970},
                        new Text() {pe_text = " , ", pe_tag = "BKP", pe_order = 1663980},
                        new Text() {pe_text = "Growth", pe_tag = "NN", pe_tag_revised = null, pe_order = 1663970},
                        new Text() {pe_text = "last quarter", pe_tag = "TM", pe_tag_revised = null, pe_order = 1663980},
                        new Text() {pe_text = "was", pe_tag = "PAST", pe_tag_revised = null, pe_order = 1663970},
                        new Text() {pe_text = "tremendous", pe_tag = "ADJ", pe_tag_revised = null, pe_order = 1663980},
                        new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = null, pe_order = 1664030},
                }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("However"));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" , "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo("last quarter"));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo("was"));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo("tremendous"));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo("Growth"));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_DIG_Is_Found_Twice_Move_TM_before_closest_one()
        {
            // Real GDP PASTrose by DIG100 pence TMper month
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_text = "However", pe_tag = "CS", pe_order = 1663970},
                    new Text() {pe_text = "150", pe_tag = "DIG", pe_order = 1663970},
                    new Text() {pe_text = "journalists", pe_tag = "NN", pe_order = 1663970},
                    new Text() {pe_text = "said", pe_tag = "VB", pe_order = 1663970},
                    new Text() {pe_text = " , ", pe_tag = "BKP", pe_order = 1663980},

                    new Text() {pe_text = " Real ", pe_tag = "ADJ", pe_order = 1663970},
                    new Text() {pe_text = " GDP ", pe_tag = "NN", pe_order = 1663980},
                    new Text() {pe_text = " rose ", pe_tag_revised = "PAST", pe_order = 1663990},
                    new Text() {pe_text = " by ", pe_tag = "BK", pe_order = 1663990},
                    new Text() {pe_text = " 100 ", pe_tag = "DIG",  pe_order = 1664010},
                    new Text() {pe_text = " pence ", pe_tag = null,pe_order = 1664010},

                    new Text() {pe_text = " per month ", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664020},
                    new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = "NULL", pe_order = 1664030},
                }
            };

            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            // per month Real  GPD  rose  by per month 100 pence
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("However")); 
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo("150")); 
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo("journalists"));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo("said")); 
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" , "));  

            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" Real ")); 
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" GDP ")); 
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" rose "));
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo(" by "));
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo(" per month "));
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo(" 100 ")); //DG
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo(" pence "));

            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenPrenBeforeNNBeforeTM_MoveTMBeforePren()
        {
            //He went out and the warm weather TM1this past winter was poor
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                    {
                        new Text() {pe_text = "However", pe_tag = "CS", pe_order = 1663970},
                        new Text() {pe_text = "150", pe_tag = "DIG", pe_order = 1663970},
                        new Text() {pe_text = "journalists", pe_tag = "NN", pe_order = 1663970},
                        new Text() {pe_text = "noted", pe_tag = "VB", pe_order = 1663970},
                        new Text() {pe_text = " , ", pe_tag = "BKP", pe_order = 1663980},

                        new Text() {pe_text = "He", pe_tag = null, pe_tag_revised = null, pe_order = 1663970},
                        new Text() {pe_text = "went", pe_tag = null, pe_tag_revised = null, pe_order = 1663980},
                        new Text() {pe_text = "out", pe_tag = null, pe_tag_revised = null, pe_order = 1663990},
                        new Text() {pe_text = "and", pe_tag = null, pe_tag_revised = null, pe_order = 1664000},
                        new Text() {pe_text = "the", pe_tag = "PREN", pe_tag_revised = null, pe_order = 1664010},
                        new Text() {pe_text = "warm", pe_tag = null, pe_tag_revised = null, pe_order = 1664020},
                        new Text() {pe_text = "weather", pe_tag = "NN", pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "this", pe_tag = "TM", pe_tag_revised = "TM1", pe_order = 1664030, pe_merge_ahead=2},
                        new Text() {pe_text = "past", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "winter", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "was", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = "poor", pe_tag = null, pe_tag_revised = null, pe_order = 1664030},
                        new Text() {pe_text = " . ", pe_tag = "BKP", pe_tag_revised = null, pe_order = 1664030}
                    }
            };
            var timerUnitStrategy = new TimerUnitStrategy();
            var returnedSentence = timerUnitStrategy.ShuffleSentence(sentence);

            // He went out and TM1this past winter the warm weather was poor
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo("He"));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo("went"));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo("out"));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo("and")); 
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo("this")); 

            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo("He"));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo("went"));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo("out"));
            Assert.That(returnedSentence.Texts[8].pe_text, Is.EqualTo("and"));  //and
            Assert.That(returnedSentence.Texts[9].pe_text, Is.EqualTo("this")); //TM1
            Assert.That(returnedSentence.Texts[10].pe_text, Is.EqualTo("past"));
            Assert.That(returnedSentence.Texts[11].pe_text, Is.EqualTo("winter"));
            Assert.That(returnedSentence.Texts[12].pe_text, Is.EqualTo("was")); //PREN
            Assert.That(returnedSentence.Texts[13].pe_text, Is.EqualTo("poor"));
            Assert.That(returnedSentence.Texts[14].pe_text, Is.EqualTo("the"));
            Assert.That(returnedSentence.Texts[15].pe_text, Is.EqualTo("warm"));
            Assert.That(returnedSentence.Texts[16].pe_text, Is.EqualTo("weather"));
            Assert.That(returnedSentence.Texts[17].pe_text, Is.EqualTo(" . "));
        }
    }
}