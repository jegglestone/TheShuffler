namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Text = Model.Text;

    [TestFixture]
    public class SentenceUnitTests
    {
        private List<Text> _additionalTexts => new List<Text>()
        {
            new Text
            {
                pe_tag="NUL",
                pe_tag_revised = "PREN"
            },
            new Text
            {
                pe_tag="DYN",
                pe_tag_revised = "DYN1"
            },
            new Text
            {
                pe_tag = "VB",
                pe_tag_revised = "NULL"
            }
        };

        [Test]
        public void When_peTagRevised_Is_CS_HasClauser_Returns_True()
        {
            var sentence = new Sentence();
            sentence.Texts.Add(new Text
            {
                pe_tag_revised = "CS"
            });
            sentence.Texts.AddRange(_additionalTexts);

            Assert.That(
                sentence.HasClauser(), Is.EqualTo(true));
        }

        [Test]
        public void When_peTag_Is_CS_And_peTagRevised_Is_DYN_Returns_False()
        {
            var sentence = new Sentence();
            sentence.Texts.AddRange(_additionalTexts);
            sentence.Texts.Add(new Text
            {
                pe_tag="CS",
                pe_tag_revised = "DYN"
            });
            Assert.That(
                sentence.HasClauser(), Is.EqualTo(false));
        }

        [Test]
        public void When_peTag_Is_CS_And_PeTagRevised_IsNUll_Returns_true()
        {
            var sentence = new Sentence();
            sentence.Texts.AddRange(_additionalTexts);
            sentence.Texts.Add(new Text
            {
                pe_tag = "CS",
                pe_tag_revised = "NULL"
            });
            sentence.Texts.AddRange(_additionalTexts);

            Assert.That(
                sentence.HasClauser(), Is.EqualTo(true));
        }
    }
}
