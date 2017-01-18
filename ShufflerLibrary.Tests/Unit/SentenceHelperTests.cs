using System.Collections.Generic;

namespace ShufflerLibrary.Tests.Unit
{
    using Helper;
    using Model;
    using NUnit.Framework;
    using Text = Model.Text;

    public class SentenceHelperTests
    {
        [Test]
        public void SplitsMultipleSentencesBasedOnFullStops()
        {
            var sentence = new Sentence
            {
                pe_para_no = 123,
                Sentence_No=22,
                SentenceHasMultipleOptions = true,
                Texts = new List<Text>
                {
                    new Text { pe_text = "hi", pe_tag = ""},
                    new Text { pe_text_revised = "there", pe_tag_revised = ""},
                    new Text { pe_text = "this", pe_tag = ""},
                    new Text { pe_text_revised = "is", pe_tag_revised = ""},
                    new Text { pe_text = "a", pe_tag = ""},
                    new Text { pe_text_revised = "sentence", pe_tag_revised = ""},
                    new Text { pe_text = " . ", pe_tag = "BKP"},

                    new Text { pe_text_revised = "Hope", pe_tag_revised = ""},
                    new Text { pe_text = "you", pe_tag = ""},
                    new Text { pe_text_revised = "like", pe_tag_revised = ""},
                    new Text { pe_text = "it", pe_tag = ""},
                    new Text { pe_text_revised = " . ", pe_tag_revised = "BKP"},

                    new Text { pe_text = "I've", pe_tag = ""},
                    new Text { pe_text_revised = "mixed", pe_tag_revised = ""},
                    new Text { pe_text = "texts", pe_tag = ""},
                    new Text { pe_text_revised = "and", pe_tag_revised = ""},
                    new Text { pe_text = "revised texts", pe_tag = ""},
                    new Text { pe_text_revised = ",", pe_tag_revised = "BKP"},
                    new Text { pe_text = "for", pe_tag = ""},
                    new Text { pe_text_revised = "you", pe_tag_revised = ""},
                    new Text { pe_text_revised = " . ", pe_tag_revised = "BKP"}
                }
            };

            List<Sentence> sentences = SentenceHelper.SplitSentenceOptions(sentence);

            Assert.That(sentences[0].Texts[0].pe_text=="hi");
            Assert.That(sentences[0].Texts[6].actual_text_used == " . ");
            Assert.That(sentences[0].TextCount == 7);
            Assert.That(sentences[0].pe_para_no == 123);

            Assert.That(sentences[1].Texts[0].actual_text_used == "Hope");
            Assert.That(sentences[1].Texts[4].actual_tag_used == "BKP");
            Assert.That(sentences[1].TextCount == 5);
            Assert.That(sentences[1].pe_para_no == 123);
            Assert.That(sentences[1].Sentence_No == 22);

            Assert.That(sentences[2].Texts[0].actual_text_used =="I've");
            Assert.That(sentences[2].Texts[8].actual_tag_used == "BKP");
            Assert.That(sentences[2].TextCount == 9);
            Assert.That(sentences[2].pe_para_no == 123);
            Assert.That(sentences[2].SentenceHasMultipleOptions == true);

            Assert.That(sentences.Count == 3);
        }
    }
}
