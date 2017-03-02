namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class MdUnitStrategyTests
    {
        [Test]
        public void WhenUnsortedMDUnitThenSortDescendingAndAddPyDe()
        {
            // MD1, MD2, MD3
            var sentenceWithAscendingMdUnits = new Sentence()
            {
                Texts=new List<Text>()
                {
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "NN", pe_text = "something"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "}
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithAscendingMdUnits = 
                strategy.ShuffleSentence(sentenceWithAscendingMdUnits);

            // MD3, MD2, MD1, PY
            Assert.That(sentenceWithAscendingMdUnits.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithAscendingMdUnits.Texts[1].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithAscendingMdUnits.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithAscendingMdUnits.Texts[3].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithAscendingMdUnits.Texts[4].pe_tag_revised, Is.EqualTo("PY"));  // new de particle
            Assert.That(sentenceWithAscendingMdUnits.Texts[5].pe_tag_revised, Is.EqualTo("BKP"));

            Assert.That(sentenceWithAscendingMdUnits.Texts[0].pe_merge_ahead, Is.EqualTo(4));
        }

        [Test]
        public void WhenSortedMDUnitThenDontSortButAddDe()
        {
            // MD3, MD2, MD1
            var sentenceWithDescendingMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "NN", pe_text = "something"},
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithDescendingMdUnits =
                strategy.ShuffleSentence(sentenceWithDescendingMdUnits);

            // MD3, MD2, MD1, PY
            Assert.That(sentenceWithDescendingMdUnits.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithDescendingMdUnits.Texts[1].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithDescendingMdUnits.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithDescendingMdUnits.Texts[3].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithDescendingMdUnits.Texts[4].pe_tag_revised, Is.EqualTo("PY"));
            Assert.That(sentenceWithDescendingMdUnits.Texts[5].pe_tag_revised, Is.EqualTo("BKP"));

            Assert.That(sentenceWithDescendingMdUnits.Texts[0].pe_merge_ahead, Is.EqualTo(4));
        }


          [Test]
          public void WhenManyMDUnitsShuffleDescendingAndAddDe()
          {
            var sentence = new Sentence()
            {
              Texts = new List<Text>()
              {
                new Text() {pe_tag = "", pe_text = "the"},
                new Text() {pe_tag = "", pe_text = "house"},
                new Text() {pe_tag = "MD1", pe_text = "on"},
                new Text() {pe_tag = "", pe_text = "the"},
                new Text() {pe_tag = "", pe_text = "corner"},
                new Text() {pe_tag = "MD2", pe_text = "of"},
                new Text() {pe_tag = "", pe_text = "River"},
                new Text() {pe_tag = "", pe_text = "Street"},
                new Text() {pe_tag = "MD3", pe_text = "in"},
                new Text() {pe_tag = "", pe_text = "city"},
                new Text() {pe_tag = "", pe_text = "centre"},
                new Text() {pe_tag = "BKP", pe_text = "."}
              }
            };

          var strategy = new MdUnitStrategy();
          sentence =
              strategy.ShuffleSentence(sentence);

          Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("the"));
          Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("house"));
          Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("in"));
          Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("city"));
          Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("centre"));
          Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("of"));
          Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("River"));
          Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("Street"));
          Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("on"));
          Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("the"));
          Assert.That(sentence.Texts[10].pe_text, Is.EqualTo("corner"));
          Assert.That(sentence.Texts[11].pe_text, Is.EqualTo(" de "));
          Assert.That(sentence.Texts[12].pe_text, Is.EqualTo("."));

          Assert.That(sentence.Texts[2].pe_merge_ahead, Is.EqualTo(9));
        }


        [Test]
        public void WhenVBUnitThenUseAsBreaker()
        {
            // MD3, MD2, MD1
            var sentenceWithVbUnit = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "NN", pe_text = "something"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "VB", pe_text = "going"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithVbUnit =
                strategy.ShuffleSentence(sentenceWithVbUnit);

            // MD3, MD2, MD1, PY
            Assert.That(sentenceWithVbUnit.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithVbUnit.Texts[1].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithVbUnit.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithVbUnit.Texts[3].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithVbUnit.Texts[4].pe_tag_revised, Is.EqualTo("PY")); // de particle
            Assert.That(sentenceWithVbUnit.Texts[5].pe_tag_revised, Is.EqualTo("VB"));
            Assert.That(sentenceWithVbUnit.Texts[6].pe_tag_revised, Is.EqualTo("BKP"));

            Assert.That(sentenceWithVbUnit.Texts[0].pe_merge_ahead, Is.EqualTo(4));
        }

        [Test]
        public void WhenAllMDUnitsHaveManyTexts()
        {
            // MD3, MD2, MD1
            var sentenceWithLargeMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "NN", pe_text = "the"},
                    new Text() { pe_tag_revised = "NN", pe_text = "house"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "NN", pe_text = "somewhere"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "NN", pe_text = "the"},
                    new Text() { pe_tag_revised = "NN", pe_text = "room"},
                    new Text() { pe_tag_revised = "VB", pe_text = "going"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithLargeMdUnits =
                strategy.ShuffleSentence(sentenceWithLargeMdUnits);

            // MD3, MD2, MD1, PY
            Assert.That(sentenceWithLargeMdUnits.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithLargeMdUnits.Texts[1].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMdUnits.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMdUnits.Texts[3].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithLargeMdUnits.Texts[4].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMdUnits.Texts[5].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithLargeMdUnits.Texts[6].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMdUnits.Texts[7].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMdUnits.Texts[8].pe_tag_revised, Is.EqualTo("PY"));
            Assert.That(sentenceWithLargeMdUnits.Texts[9].pe_tag_revised, Is.EqualTo("VB"));
            Assert.That(sentenceWithLargeMdUnits.Texts[10].pe_tag_revised, Is.EqualTo("BKP"));

            Assert.That(sentenceWithLargeMdUnits.Texts[0].pe_merge_ahead, Is.EqualTo(8));
        }

        [Test]
        public void WhenMDIsNotAtTheStart()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "NA", pe_text = "And"},
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "NN", pe_text = "something"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "VB", pe_text = "going"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_tag_revised, Is.EqualTo("NA"));
            Assert.That(sentenceWithMdUnits.Texts[1].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithMdUnits.Texts[2].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithMdUnits.Texts[3].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithMdUnits.Texts[4].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_tag_revised, Is.EqualTo("PY"));
            Assert.That(sentenceWithMdUnits.Texts[6].pe_tag_revised, Is.EqualTo("VB"));
            Assert.That(sentenceWithMdUnits.Texts[7].pe_tag_revised, Is.EqualTo("BKP"));

            Assert.That(sentenceWithMdUnits.Texts[1].pe_merge_ahead, Is.EqualTo(4));
        }

        [Test]
        public void VbPrenMdUnit()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "NA", pe_text = "They", pe_order = 10},
                    new Text() { pe_tag_revised = "NA", pe_text = "bombed", pe_order = 20},
                    new Text() { pe_tag_revised = "PREN", pe_text = "the", pe_order = 30},
                    new Text() { pe_tag_revised = "NN", pe_text = "place", pe_order = 40},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in", pe_order = 50},
                    new Text() { pe_tag_revised = "NN", pe_text = "city", pe_order = 60},
                    new Text() { pe_tag_revised = "NN", pe_text = "centre", pe_order = 70},
                    new Text() { pe_tag_revised = "MD2", pe_text = " of ", pe_order = 80},
                    new Text() { pe_tag_revised = "NN", pe_text = "river", pe_order = 90},
                    new Text() { pe_tag_revised = "NN", pe_text = "street", pe_order = 100},
                    new Text() { pe_tag_revised = "TM", pe_text = "at", pe_order = 110},
                    new Text() { pe_tag_revised = "TM", pe_text = "5pm", pe_order = 120},
                    new Text() { pe_tag_revised = "MD1", pe_text = "on", pe_order = 130},
                    new Text() { pe_tag_revised = "NN", pe_text = "corner", pe_order = 140},
                    new Text() { pe_tag_revised = "NN", pe_text = "house", pe_order = 150},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order = 160},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("They"));
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("bombed"));

            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("in")); // MD unit
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("city"));
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("centre"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo(" of "));
            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("river"));
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("street"));
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo("at"));
            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo("5pm"));
            Assert.That(sentenceWithMdUnits.Texts[10].pe_text, Is.EqualTo("on"));
            Assert.That(sentenceWithMdUnits.Texts[11].pe_text, Is.EqualTo("corner"));
            Assert.That(sentenceWithMdUnits.Texts[12].pe_text, Is.EqualTo("house"));
            Assert.That(sentenceWithMdUnits.Texts[13].pe_text, Is.EqualTo(" de "));

            Assert.That(sentenceWithMdUnits.Texts[14].pe_text, Is.EqualTo("the"));  // PREN
            Assert.That(sentenceWithMdUnits.Texts[15].pe_text, Is.EqualTo("place"));

            Assert.That(sentenceWithMdUnits.Texts[16].pe_text, Is.EqualTo(" . "));

            Assert.That(sentenceWithMdUnits.Texts[2].pe_merge_ahead, Is.EqualTo(11));
        }

        [Test]
        public void WhenNNThenPRENThenMDUnit_MoveMDunitBeforePREN()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "PREN", pe_text = "A"},
                    new Text() { pe_tag_revised = "", pe_text = "short"},
                    new Text() { pe_tag_revised = "NN", pe_text = "leaflet"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "", pe_text = "family"},
                    new Text() { pe_tag_revised = "MD1", pe_text = "on"},
                    new Text() { pe_tag_revised = "", pe_text = "the"},
                    new Text() { pe_tag_revised = "", pe_text = "importance"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
    
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("of")); //MD2
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("family"));
            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("on")); //MD1
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("importance"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo(" de "));  // de added
            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("A")); //PREN
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("short"));
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo("leaflet")); //NN
            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo(" . "));

            Assert.That(sentenceWithMdUnits.Texts[0].pe_merge_ahead, Is.EqualTo(5));

        }


        [Test]
        public void WhenBKPThenPRENThenNNThenMDUnit_MoveMDunitBeforePREN()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "", pe_text = "However", pe_order = 10},
                    new Text() { pe_tag_revised = "BKP", pe_text = " , ", pe_order = 20},
                    new Text() { pe_tag_revised = "PREN", pe_text = "A", pe_order = 30},
                    new Text() { pe_tag_revised = "", pe_text = "short", pe_order = 40},
                    new Text() { pe_tag_revised = "NN", pe_text = "leaflet", pe_order = 50},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of", pe_order = 60},
                    new Text() { pe_tag_revised = "", pe_text = "family", pe_order = 70},
                    new Text() { pe_tag_revised = "MD1", pe_text = "on", pe_order = 80},
                    new Text() { pe_tag_revised = "", pe_text = "the", pe_order = 90},
                    new Text() { pe_tag_revised = "", pe_text = "importance", pe_order = 100},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order = 110},

                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);
            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("However")); 
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo(" , ")); //BKP 
            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("of")); //MD2
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("family"));
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("on")); //MD1
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("importance"));
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo(" de "));  // de added
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo("A")); //PREN
            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo("short"));
            Assert.That(sentenceWithMdUnits.Texts[10].pe_text, Is.EqualTo("leaflet")); //NN
            Assert.That(sentenceWithMdUnits.Texts[11].pe_text, Is.EqualTo(" . "));

            Assert.That(sentenceWithMdUnits.Texts[2].pe_merge_ahead, Is.EqualTo(5));
        }

        [Test]
        public void WhenADJThenNNThenMDUnit_MoveMDunitBeforeADJ()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "ADJ", pe_text = "Nice", pe_order = 10},
                    new Text() { pe_tag_revised = "NN", pe_text = "folk", pe_order = 20 },
                    new Text() { pe_tag_revised = "MD1", pe_text = "of", pe_order = 30},
                    new Text() { pe_tag_revised = null, pe_text = "integrity", pe_order = 40},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order = 50},

                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("of")); //MD1
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("integrity"));
            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("Nice")); //ADJ
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("folk")); //NN
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo(" . "));

            Assert.That(sentenceWithMdUnits.Texts[0].pe_merge_ahead, Is.EqualTo(1));
        }

        [Test]
        public void WhenNNThenMD_MoveMDBeforeNN()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "NN", pe_text = "People", pe_order = 10},
                    new Text() { pe_tag_revised = "MD1", pe_text = "of", pe_order = 20 },
                    new Text() { pe_tag_revised = "", pe_text = "the", pe_order = 30},
                    new Text() { pe_tag_revised = "", pe_text = "highest", pe_order = 40},
                    new Text() { pe_tag_revised = "", pe_text = "degree", pe_order = 50},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of", pe_order = 60},
                    new Text() { pe_tag_revised = null, pe_text = "integrity", pe_order = 70},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order = 80},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("of")); //MD2
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("integrity"));
            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("of")); //MD1
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("the")); 
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("highest"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo("degree")); 
            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo(" de "));
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("People")); //NN
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo(" . "));

            Assert.That(sentenceWithMdUnits.Texts[0].pe_merge_ahead, Is.EqualTo(6));
        }

        [Test]
        public void WhenNNThenMDwithNoPrenOrAdj_MoveMDBeforeNN()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "", pe_text = "They", pe_order = 10},
                    new Text() { pe_tag_revised = "PAST", pe_text = "met", pe_order = 20 },
                    new Text() { pe_tag_revised = "NN", pe_text = "him", pe_order = 30},
                    new Text() { pe_tag_revised = "MD1", pe_text = "at", pe_order = 40},
                    new Text() { pe_tag_revised = "", pe_text = "the", pe_order = 50},
                    new Text() { pe_tag_revised = "", pe_text = "centre", pe_order = 60},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of", pe_order = 70},
                    new Text() { pe_tag_revised = null, pe_text = "the", pe_order = 80},
                    new Text() { pe_tag_revised = null, pe_text = "city", pe_order = 90},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order = 100},
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("They"));
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("met")); // PAST

            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("of")); //MD2
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("the")); 
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("city"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo("at")); //MD1
            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("centre"));
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo(" de "));

            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo("him"));
            Assert.That(sentenceWithMdUnits.Texts[10].pe_text, Is.EqualTo(" . "));

            Assert.That(sentenceWithMdUnits.Texts[2].pe_merge_ahead, Is.EqualTo(6));
        }

        [Test]
        public void WhenVbPastPresThenNN_MoveMdAfterVbPastPres()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                     new Text() { pe_tag_revised = null, pe_text = "We", pe_order =  10},
                     new Text() { pe_tag_revised = "VBA", pe_text = "will", pe_order =  20},

                     new Text() { pe_tag_revised = "VB", pe_text = "meet", pe_order =  30},
                     new Text() { pe_tag_revised = "NN", pe_text = "him", pe_order =  40},

                     new Text() { pe_tag_revised = "MD2", pe_text = "of", pe_order =  50},
                     new Text() { pe_tag_revised = null, pe_text = "the", pe_order =  60},
                     new Text() { pe_tag_revised = null, pe_text = "city", pe_order =  70},
                     new Text() { pe_tag_revised = "MD1", pe_text = "at", pe_order =  80},
                     new Text() { pe_tag_revised = null, pe_text = "the", pe_order =  90},
                     new Text() { pe_tag_revised = null, pe_text = "centre", pe_order =  100},

                     new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order =  110}
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("We"));
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("will")); //VBA

            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("meet")); //VB
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("of"));   //MD2
            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo("city"));

            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("at"));   //MD1
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo("centre"));
            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo(" de "));
            Assert.That(sentenceWithMdUnits.Texts[10].pe_text, Is.EqualTo("him"));
        }


        [Test]
        public void WhenMdNulThatBeforeVbPastPresBeforeNN_MoveMdBeforeVbPastPres()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                     new Text() { pe_tag_revised = null, pe_text = "And", pe_order =  10},
                     new Text() { pe_tag_revised = "MDNUL", pe_text = "that", pe_order =  20},
                     new Text() { pe_tag_revised = null, pe_text = "is", pe_order =  30},
                     new Text() { pe_tag_revised = null, pe_text = "Why", pe_order =  40},

                     new Text() { pe_tag_revised = null, pe_text = "We", pe_order =  50},
                     new Text() { pe_tag_revised = "VBA", pe_text = "will", pe_order =  60},

                     new Text() { pe_tag_revised = "VB", pe_text = "meet", pe_order =  70},
                     new Text() { pe_tag_revised = "NN", pe_text = "him", pe_order =  80},

                     new Text() { pe_tag_revised = "MD2", pe_text = "of", pe_order =  90},
                     new Text() { pe_tag_revised = null, pe_text = "the", pe_order =  100},
                     new Text() { pe_tag_revised = null, pe_text = "city", pe_order =  170},
                     new Text() { pe_tag_revised = "MD1", pe_text = "at", pe_order =  180},
                     new Text() { pe_tag_revised = null, pe_text = "the", pe_order =  190},
                     new Text() { pe_tag_revised = null, pe_text = "centre", pe_order =  100},

                     new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order =  110}
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("And"));
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("that")); //MDNUL
            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("is"));  
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("Why"));

            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("We"));
            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo("will")); //VBA

            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("meet")); //VB

            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("of"));   //MD2
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo("city"));

            Assert.That(sentenceWithMdUnits.Texts[10].pe_text, Is.EqualTo("at"));   //MD1
            Assert.That(sentenceWithMdUnits.Texts[11].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMdUnits.Texts[12].pe_text, Is.EqualTo("centre"));
            Assert.That(sentenceWithMdUnits.Texts[13].pe_text, Is.EqualTo(" de "));
            Assert.That(sentenceWithMdUnits.Texts[14].pe_text, Is.EqualTo("him"));
        }

        [Test]
        public void WhenVBAPastNNMD_MoveMDAfterVba()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "ADJ", pe_text = "Economic", pe_order =  10},
                    new Text() { pe_tag_revised = "NN", pe_text = "growth", pe_order =  20},
                    new Text() { pe_tag_revised = "VBA", pe_text = "has", pe_order =  30},
                    new Text() { pe_tag_revised = "PAST", pe_text = "continued", pe_order =  40},
                    new Text() { pe_tag_revised = "MD1", pe_text = "at", pe_order =  50},
                    new Text() { pe_tag_revised = "PREN", pe_text = "a"},
                    new Text() { pe_tag_revised = "ADJ", pe_text = "moderate"},
                    new Text() { pe_tag_revised = "NN", pe_text = "rate"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MdUnitStrategy();
            sentence =
                strategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Economic"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("growth"));

            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("has")); //VBA
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("at"));  //MD1
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("a"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("moderate"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("rate")); //PAST

            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("continued"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenEconomicGrowth_Move_Md_before_VbaHas()
        {
            var sentenceWithMdUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                     new Text() { pe_tag_revised = "ADJ", pe_text = "Economic", pe_order =  10},
                     new Text() { pe_tag_revised = "NN", pe_text = "growth", pe_order =  20},
                     new Text() { pe_tag_revised = "TM3", pe_text = "this year", pe_order =  40},
                     new Text() { pe_tag_revised = "TM2", pe_text = "so far", pe_order =  60},
                     new Text() { pe_tag_revised = "VBA", pe_text = "has", pe_order =  30},
                     new Text() { pe_tag_revised = "PAST", pe_text = "continued", pe_order =  170},
                     new Text() { pe_tag_revised = "MD1", pe_text = "at", pe_order =  80},
                     new Text() { pe_tag_revised = "PREN", pe_text = "a", pe_order =  90},
                     new Text() { pe_tag_revised = "ADJ", pe_text = "moderate", pe_order =  100},
                     new Text() { pe_tag_revised = "NN", pe_text = "rate", pe_order =  100},
                     new Text() { pe_tag_revised = "BKP", pe_text = " . ", pe_order =  180}
                }
            };

            var strategy = new MdUnitStrategy();
            sentenceWithMdUnits =
                strategy.ShuffleSentence(sentenceWithMdUnits);

            Assert.That(sentenceWithMdUnits.Texts[0].pe_text, Is.EqualTo("Economic"));
            Assert.That(sentenceWithMdUnits.Texts[1].pe_text, Is.EqualTo("growth"));

            Assert.That(sentenceWithMdUnits.Texts[2].pe_text, Is.EqualTo("this year"));
            Assert.That(sentenceWithMdUnits.Texts[3].pe_text, Is.EqualTo("so far"));

            Assert.That(sentenceWithMdUnits.Texts[4].pe_text, Is.EqualTo("has")); //Vba       


            Assert.That(sentenceWithMdUnits.Texts[5].pe_text, Is.EqualTo("at"));
            Assert.That(sentenceWithMdUnits.Texts[6].pe_text, Is.EqualTo("a"));
            Assert.That(sentenceWithMdUnits.Texts[7].pe_text, Is.EqualTo("moderate"));
            Assert.That(sentenceWithMdUnits.Texts[8].pe_text, Is.EqualTo("rate"));

            Assert.That(sentenceWithMdUnits.Texts[9].pe_text, Is.EqualTo("continued"));
        }
    }
}
