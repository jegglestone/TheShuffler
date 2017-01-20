namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class DoublePrenUnitStrategyTests
    {
        [Test]
        public void WhenTwoPrensShufflerTheirOrder()
        {
            var sentenceWithDoublePren = new Sentence
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised ="", pe_text = "This"},
                    new Text() { pe_tag_revised ="VB1", pe_text = "is"},
                    new Text() { pe_tag_revised ="PREN1", pe_text = "a"},
                    new Text() { pe_tag_revised ="NN", pe_text = "book"},
                    new Text() { pe_tag_revised ="PREN2", pe_text = "about"},
                    new Text() { pe_tag_revised ="NN", pe_text = "war"},
                    new Text() { pe_tag_revised ="BKP", pe_text = " . "},
                }
            };

            DoublePrenStrategy doublePrenStrategy = new DoublePrenStrategy();
            sentenceWithDoublePren = doublePrenStrategy.ShuffleSentence(sentenceWithDoublePren);

            Assert.That(sentenceWithDoublePren.Texts[0].pe_text, Is.EqualTo("This"));
            Assert.That(sentenceWithDoublePren.Texts[1].pe_text, Is.EqualTo("is"));
            Assert.That(sentenceWithDoublePren.Texts[2].pe_text, Is.EqualTo("about"));
            Assert.That(sentenceWithDoublePren.Texts[3].pe_text, Is.EqualTo("war"));
            Assert.That(sentenceWithDoublePren.Texts[4].pe_text, Is.EqualTo("a"));
            Assert.That(sentenceWithDoublePren.Texts[5].pe_text, Is.EqualTo("book"));
            Assert.That(sentenceWithDoublePren.Texts[6].pe_text, Is.EqualTo(" . "));
        }
    }
}

