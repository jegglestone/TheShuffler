namespace ShufflerLibrary.Tests.Unit
{
    using Model;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class MdNulThatUnitStrategyTests
    {
        [Test]
        public void When_PrenAndMDNulThat_Move_MDNul_To_AfterPren()
        {
            var sentence = new Sentence()
            {
                //Bef: Some of PRENthe factors MDNULthat have restrained the recovery PYde persist.
                //Aft: Some of PRENthe MDNULthat have restrained the recovery PYde factors persist.

                Texts = new List<Text>()
                {
                    new Text() { pe_text = "Some", pe_tag = ""},
                    new Text() { pe_text = "of", pe_tag = ""},
                    new Text() { pe_text = "the", pe_tag = "PREN"},
                    new Text() { pe_text = "factors", pe_tag = "NN"},
                    new Text() { pe_text = " that ", pe_tag = "MDNUL"},
                    new Text() { pe_text = "have restrained", pe_tag = ""},
                    new Text() { pe_text = "the", pe_tag = ""},
                    new Text() { pe_text = "recovery", pe_tag = ""},
                    new Text() { pe_text = " de ", pe_tag = "PY"},
                    new Text() { pe_text = "persist", pe_tag = ""},
                    new Text() { pe_text = " . ", pe_tag = "BKP"}
                }
            };
            MdNulThatUnitStrategy mdNulThatUnitStrategy = new MdNulThatUnitStrategy();
            sentence = mdNulThatUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Some"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("of"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("the")); //PREN
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo(" that ")); //MDNUL
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("have restrained"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("recovery"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo(" de "));    //PY
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("factors")); //NN
            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("persist"));
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo(" . "));
        }
    }
}
