namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class MDUnitStrategyTests
    {
        [Test]
        public void WhenUnsortedMDUnitThenSortDescending()
        {
            // MD1, MD2, MD3
            var sentenceWithAscendingMDUnits = new Sentence()
            {
                Texts=new List<Text>()
                {
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "NN", pe_text = "something"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MDUnitStrategy();
            sentenceWithAscendingMDUnits = 
                strategy.ShuffleSentence(sentenceWithAscendingMDUnits);

            Assert.That(sentenceWithAscendingMDUnits.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithAscendingMDUnits.Texts[1].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithAscendingMDUnits.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithAscendingMDUnits.Texts[3].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithAscendingMDUnits.Texts[4].pe_tag_revised, Is.EqualTo("BKP"));
        }

        [Test]
        public void WhenSortedMDUnitThenDontSort()
        {
            // MD3, MD2, MD1
            var sentenceWithDescendingMDUnits = new Sentence()
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

            var strategy = new MDUnitStrategy();
            sentenceWithDescendingMDUnits =
                strategy.ShuffleSentence(sentenceWithDescendingMDUnits);

            Assert.That(sentenceWithDescendingMDUnits.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithDescendingMDUnits.Texts[1].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithDescendingMDUnits.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithDescendingMDUnits.Texts[3].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithDescendingMDUnits.Texts[4].pe_tag_revised, Is.EqualTo("BKP"));
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

      var strategy = new MDUnitStrategy();
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


      //Aft:  …the house MD3in city centre MD2of River Street MD1on the corner de.

    }


    [Test]
        public void WhenVBUnitThenUseAsBreaker()
        {
            // MD3, MD2, MD1
            var sentenceWithVBUnit = new Sentence()
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

            var strategy = new MDUnitStrategy();
            sentenceWithVBUnit =
                strategy.ShuffleSentence(sentenceWithVBUnit);

            Assert.That(sentenceWithVBUnit.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithVBUnit.Texts[1].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithVBUnit.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithVBUnit.Texts[3].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithVBUnit.Texts[4].pe_tag_revised, Is.EqualTo("VB"));
            Assert.That(sentenceWithVBUnit.Texts[5].pe_tag_revised, Is.EqualTo("BKP"));
        }

        [Test]
        public void WhenAllMDUnitsHaveManyTexts()
        {
            // MD3, MD2, MD1
            var sentenceWithLargeMDUnits = new Sentence()
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

            var strategy = new MDUnitStrategy();
            sentenceWithLargeMDUnits =
                strategy.ShuffleSentence(sentenceWithLargeMDUnits);

            Assert.That(sentenceWithLargeMDUnits.Texts[0].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithLargeMDUnits.Texts[1].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMDUnits.Texts[2].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMDUnits.Texts[3].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithLargeMDUnits.Texts[4].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMDUnits.Texts[5].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithLargeMDUnits.Texts[6].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMDUnits.Texts[7].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithLargeMDUnits.Texts[8].pe_tag_revised, Is.EqualTo("VB"));
            Assert.That(sentenceWithLargeMDUnits.Texts[9].pe_tag_revised, Is.EqualTo("BKP"));
        }

        [Test]
        public void WhenMDIsNotAtTheStart()
        {
            var sentenceWithMDUnits = new Sentence()
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

            var strategy = new MDUnitStrategy();
            sentenceWithMDUnits =
                strategy.ShuffleSentence(sentenceWithMDUnits);

            Assert.That(sentenceWithMDUnits.Texts[0].pe_tag_revised, Is.EqualTo("NA"));
            Assert.That(sentenceWithMDUnits.Texts[1].pe_tag_revised, Is.EqualTo("MD3"));
            Assert.That(sentenceWithMDUnits.Texts[2].pe_tag_revised, Is.EqualTo("MD2"));
            Assert.That(sentenceWithMDUnits.Texts[3].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithMDUnits.Texts[4].pe_tag_revised, Is.EqualTo("MD1"));
            Assert.That(sentenceWithMDUnits.Texts[5].pe_tag_revised, Is.EqualTo("VB"));
            Assert.That(sentenceWithMDUnits.Texts[6].pe_tag_revised, Is.EqualTo("BKP"));
        }

        [Test]
        public void WhenPrenDigAdjPlusNNUnitBeforeModifier()
        {
            var sentenceWithMDUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "ADJ", pe_text = "Real"},
                    new Text() { pe_tag_revised = "NN", pe_text = "GDP"},
                    new Text() { pe_tag_revised = "NA", pe_text = "looks"},
                    new Text() { pe_tag_revised = "PREN1", pe_text = "about"},
                    new Text() { pe_tag_revised = "DIG", pe_text = "2"},
                    new Text() { pe_tag_revised = "NN", pe_text = "percent"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "PREN", pe_text = "this"},
                    new Text() { pe_tag_revised = "TMY", pe_text = "year"},
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "TM1", pe_text = "first quarter"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "}
                }
            };

            var strategy = new MDUnitStrategy();
            sentenceWithMDUnits =
                strategy.ShuffleSentence(sentenceWithMDUnits);

            Assert.That(sentenceWithMDUnits.Texts[0].pe_tag_revised, Is.EqualTo("ADJ"));
            Assert.That(sentenceWithMDUnits.Texts[1].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithMDUnits.Texts[2].pe_tag_revised, Is.EqualTo("NA"));
            Assert.That(sentenceWithMDUnits.Texts[3].pe_tag_revised, Is.EqualTo("PREN1"));
            Assert.That(sentenceWithMDUnits.Texts[4].pe_tag_revised, Is.EqualTo("DIG"));

            Assert.That(sentenceWithMDUnits.Texts[5].pe_tag_revised, Is.EqualTo("TMY"));
            Assert.That(sentenceWithMDUnits.Texts[6].pe_tag_revised, Is.EqualTo("TM1"));
            Assert.That(sentenceWithMDUnits.Texts[7].pe_text_revised, Is.EqualTo(" de "));

            Assert.That(sentenceWithMDUnits.Texts[8].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithMDUnits.Texts[9].pe_tag_revised, Is.EqualTo("BKP"));
        }

        [Test]
        public void WhenPrenDigAdjPlusNNUnitBeforeShorterModifier()
        {
            var sentenceWithMDUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "ADJ", pe_text = "Real"},
                    new Text() { pe_tag_revised = "NN", pe_text = "GDP"},
                    new Text() { pe_tag_revised = "NA", pe_text = "looks"},
                    new Text() { pe_tag_revised = "PREN1", pe_text = "about"},
                    new Text() { pe_tag_revised = "DIG", pe_text = "2"},
                    new Text() { pe_tag_revised = "NN", pe_text = "percent"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of"},
                    new Text() { pe_tag_revised = "PREN", pe_text = "this"},
                    new Text() { pe_tag_revised = "TMY", pe_text = "year"},
                    new Text() { pe_tag_revised = "MD1", pe_text = "in"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "}
                }
            };

            var strategy = new MDUnitStrategy();
            sentenceWithMDUnits =
                strategy.ShuffleSentence(sentenceWithMDUnits);

            Assert.That(sentenceWithMDUnits.Texts[0].pe_tag_revised, Is.EqualTo("ADJ"));
            Assert.That(sentenceWithMDUnits.Texts[1].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithMDUnits.Texts[2].pe_tag_revised, Is.EqualTo("NA"));
            Assert.That(sentenceWithMDUnits.Texts[3].pe_tag_revised, Is.EqualTo("PREN1"));
            Assert.That(sentenceWithMDUnits.Texts[4].pe_tag_revised, Is.EqualTo("DIG"));

            Assert.That(sentenceWithMDUnits.Texts[5].pe_tag_revised, Is.EqualTo("TMY"));
            Assert.That(sentenceWithMDUnits.Texts[6].pe_text_revised, Is.EqualTo(" de "));

            Assert.That(sentenceWithMDUnits.Texts[7].pe_tag_revised, Is.EqualTo("NN"));
            Assert.That(sentenceWithMDUnits.Texts[8].pe_tag_revised, Is.EqualTo("BKP"));
        }

        [Test]
        public void VbPrenMdUnit()
        {
            var sentenceWithMDUnits = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "NA", pe_text = "They"},
                    new Text() { pe_tag_revised = "NA", pe_text = "bombed"},
                    new Text() { pe_tag_revised = "PREN", pe_text = "the"},
                    new Text() { pe_tag_revised = "NN", pe_text = "place"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "NN", pe_text = "city"},
                    new Text() { pe_tag_revised = "NN", pe_text = "centre"},
                    new Text() { pe_tag_revised = "MD2", pe_text = " of "},
                    new Text() { pe_tag_revised = "NN", pe_text = "river"},
                    new Text() { pe_tag_revised = "NN", pe_text = "street"},
                    new Text() { pe_tag_revised = "TM", pe_text = "at"},
                    new Text() { pe_tag_revised = "TM", pe_text = "5pm"},
                    new Text() { pe_tag_revised = "MD1", pe_text = "on"},
                    new Text() { pe_tag_revised = "NN", pe_text = "corner"},
                    new Text() { pe_tag_revised = "NN", pe_text = "house"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "},
                }
            };

            var strategy = new MDUnitStrategy();
            sentenceWithMDUnits =
                strategy.ShuffleSentence(sentenceWithMDUnits);

            Assert.That(sentenceWithMDUnits.Texts[0].pe_text, Is.EqualTo("They"));
            Assert.That(sentenceWithMDUnits.Texts[1].pe_text, Is.EqualTo("bombed"));
            Assert.That(sentenceWithMDUnits.Texts[2].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithMDUnits.Texts[3].pe_text, Is.EqualTo("place"));
            Assert.That(sentenceWithMDUnits.Texts[4].pe_text, Is.EqualTo("in"));
            Assert.That(sentenceWithMDUnits.Texts[5].pe_text, Is.EqualTo("city"));
            Assert.That(sentenceWithMDUnits.Texts[6].pe_text, Is.EqualTo("centre"));
            Assert.That(sentenceWithMDUnits.Texts[7].pe_text, Is.EqualTo(" of "));

            Assert.That(sentenceWithMDUnits.Texts[8].pe_text, Is.EqualTo("river"));
            Assert.That(sentenceWithMDUnits.Texts[9].pe_text, Is.EqualTo("street"));
            Assert.That(sentenceWithMDUnits.Texts[10].pe_text, Is.EqualTo("at"));
            Assert.That(sentenceWithMDUnits.Texts[11].pe_text, Is.EqualTo("5pm"));
            Assert.That(sentenceWithMDUnits.Texts[12].pe_text, Is.EqualTo("on"));
            Assert.That(sentenceWithMDUnits.Texts[13].pe_text, Is.EqualTo("corner"));

            Assert.That(sentenceWithMDUnits.Texts[14].pe_text, Is.EqualTo("house"));
            Assert.That(sentenceWithMDUnits.Texts[15].pe_text, Is.EqualTo(" de "));
            
            Assert.That(sentenceWithMDUnits.Texts[16].pe_text, Is.EqualTo(" . "));
        }

        //[Test]
        //public void WhenMDfollowedByPyXuyao()
        //{
        //    var sentenceWithMDandPYXuyao = new Sentence()
        //    {
        //        Texts = new List<Text>()
        //        {
        //            new Text() { pe_tag_revised = "NA", pe_text = "This"},
        //            new Text() { pe_tag_revised = "VB", pe_text = "is"},
        //            new Text() { pe_tag_revised = "PREN", pe_text = "a"},
        //            new Text() { pe_tag_revised = "NN", pe_text = "book"},
        //            new Text() { pe_tag_revised = "MD", pe_text = "to"},
        //            new Text() { pe_tag_revised = "PY", pe_text = " xuyao "},
        //            new Text() { pe_tag_revised = "VB", pe_text = "read"},
        //            new Text() { pe_tag_revised = "PY", pe_text = " de "},
        //            new Text() { pe_tag_revised = "BKP", pe_text = " . "},
        //        }
        //    };

        //    var strategy = new MDUnitStrategy();
        //    sentenceWithMDandPYXuyao =
        //        strategy.ShuffleSentence(sentenceWithMDandPYXuyao);

        //    Assert.That(sentenceWithMDandPYXuyao.Texts[0].pe_text, Is.EqualTo("This"));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[1].pe_text, Is.EqualTo("is"));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[2].pe_text, Is.EqualTo("a"));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[3].pe_text, Is.EqualTo(" xuyao "));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[4].pe_text, Is.EqualTo("read"));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[5].pe_text, Is.EqualTo(" de "));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[6].pe_text, Is.EqualTo("book"));
        //    Assert.That(sentenceWithMDandPYXuyao.Texts[7].pe_text, Is.EqualTo(" . "));
        //}
    }
}
