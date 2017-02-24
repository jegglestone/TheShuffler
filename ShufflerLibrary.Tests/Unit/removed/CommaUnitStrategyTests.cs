namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class CommaUnitStrategyTests
    {
        [Test]
        public void WhenMultipleCommasRemoveAllExceptTheFirst()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag = "NN", pe_text = "Hello"},
                    new Text() { pe_tag = "", pe_text = "there"},
                    new Text() { pe_tag = "BKP", pe_text = " , "},
                    new Text() { pe_tag = "", pe_text = "yes"},
                    new Text() { pe_tag = "BKP", pe_text = " , "},
                    new Text() { pe_tag = "", pe_text = "you"},
                    new Text() { pe_tag = "BKP", pe_text = " , "},
                    new Text() { pe_tag = "", pe_text = "that's"},
                    new Text() { pe_tag = "", pe_text = "right"},
                    new Text() { pe_tag = "BKP", pe_text = " . "},
                }
            };

            CommaUnitStrategy commaUnitStrategy = new CommaUnitStrategy();
            sentence = commaUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Hello"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("there"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo(" , "));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("yes"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("you"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("that's"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("right"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo(" . "));
        }
    }
}
