using System.Collections.Generic;
using NUnit.Framework;
using ShufflerLibrary.Model;
using ShufflerLibrary.Strategy;
using Text = ShufflerLibrary.Model.Text;

namespace ShufflerLibrary.Tests.Unit
{
    [TestFixture]
    public class PyYoUnitStrategyTests
    {
        [Test]
        public void WhenPyYoNotFound_returnsSentence()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag = "", pe_text = "Hi"},
                    new Text() { pe_tag = "VB", pe_text = "there"},
                    new Text() { pe_tag = "BKP", pe_text = " . "}
                }
            };

            PyYouUnitStrategy pyYouUnitStrategy = new PyYouUnitStrategy();
            sentence = pyYouUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Hi"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("there"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenPyJinxindePrenNNYo_AndNoMdBk_Move_PyJinxingdePrenNnAfterPyYo()
        {
            var sentence = new Sentence()
            {
                //Bef: PYjinxingde  PRENA NNsurvey PYyo PRENthe NNuniversity PASTfound that…
                Texts = new List<Text>()
                {
                    new Text() { pe_tag = "PY", pe_text = " jinxingde "},
                    new Text() { pe_tag = "PREN", pe_text = " A "},
                    new Text() { pe_tag = "NN", pe_text = " survey "},

                    new Text() { pe_tag = "PY", pe_text = " yo "},
                    new Text() { pe_tag = "PREN", pe_text = " the "},
                    new Text() { pe_tag = "NN", pe_text = " university "},

                    new Text() { pe_tag = "PAST", pe_text = " found "},
                    new Text() { pe_tag = "", pe_text = "that"},
                    new Text() { pe_tag = "", pe_text = "grades"},
                    new Text() { pe_tag = "", pe_text = "were"},
                    new Text() { pe_tag = "", pe_text = "good"},
                    new Text() { pe_tag = "BKP", pe_text = " . "}
                }
            };

            PyYouUnitStrategy pyYouUnitStrategy = new PyYouUnitStrategy();
            sentence = pyYouUnitStrategy.ShuffleSentence(sentence);

            //Aft: PYyo PRENthe NNuniversity PYjinxingde  PRENA NNsurvey PASTfound that…
            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo(" yo ")); //PyYo
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo(" the ")); //PREN
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo(" university ")); //NN

            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo(" jinxingde ")); //PY
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo(" A "));      // PREN
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo(" survey ")); // NN

            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo(" found ")); //PAST
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("that"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("grades"));
            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("were"));
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo("good"));
            Assert.That(sentence.Texts[11].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void  WhenPyJinxindePrenNNYo_AndMdBk_Move_PyJinxingdeAfterPyYoAndWhenPyJinxindePrenNNYo_AndNoMdBk_Move_PyJinxingdePrenNnAfterPyYoAfterMdbk()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    //Bef: PYjinxingde  PRENA NNsurvey PYyo PRENthe NNuniversity MDBKon students PASTfound that…

                    new Text() { pe_tag = "PY", pe_text = "jinxingde"}, //jxd pren nn
                    new Text() { pe_tag = "PREN", pe_text = " A "},
                    new Text() { pe_tag = "NN", pe_text = "survey"},
                    new Text() { pe_tag = "PY", pe_text = " yo "}, //PyYo
                    new Text() { pe_tag = "PREN", pe_text = "the"},
                    new Text() { pe_tag = "NN", pe_text = "university"},
                    new Text() { pe_tag = "MDBK", pe_text = "on", pe_merge_ahead = 1},  //Mdbk
                    new Text() { pe_tag = "", pe_text = "students"},
                    new Text() { pe_tag = "PAST", pe_text = "found"}, //end PyYo
                    new Text() { pe_tag = "", pe_text = "that"},
                    new Text() { pe_tag = "", pe_text = "its"},
                    new Text() { pe_tag = "", pe_text = "fine"},
                    new Text() { pe_tag = "BKP", pe_text = " . "}
                }
            };

            PyYouUnitStrategy pyYouUnitStrategy = new PyYouUnitStrategy();
            sentence = pyYouUnitStrategy.ShuffleSentence(sentence);

            //Aft: PYyo PRENthe NNuniversity PYjinxingde  MDBKon students PRENA NNsurvey PASTfound that…
            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo(" yo "));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("university"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("jinxingde"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("on"));// mdbk
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("students"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo(" A "));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("survey"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("found"));
            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("that"));
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo("its"));
        }

        [Test]
        public void WhenPydeNNPPastPyde_AndNoMdbk_MovePastAfterYo()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    //Bef: PYde NNPTests PASTconducted PYde PYyo PRENthe NNFederal Reserve PASTfound that…

                    new Text() { pe_tag = "PY", pe_text = "de"},
                    new Text() { pe_tag = "NNP", pe_text = "Tests"},
                    new Text() { pe_tag = "PAST", pe_text = "conducted"},
                    new Text() { pe_tag = "PY", pe_text = "de"},

                    new Text() { pe_tag = "PY", pe_text = "yo", pe_merge_ahead = 3},
                    new Text() { pe_tag = "PREN", pe_text = "the"},
                    new Text() { pe_tag = "NN", pe_text = "Federal"},
                    new Text() { pe_tag = "", pe_text = "Reserve"},

                    new Text() { pe_tag = "PAST", pe_text = "found"},
                    new Text() { pe_tag = "", pe_text = "that"},
                    new Text() { pe_tag = "BKP", pe_text = " . "}
                }
            };

            PyYouUnitStrategy pyYouUnitStrategy = new PyYouUnitStrategy();
            sentence = pyYouUnitStrategy.ShuffleSentence(sentence);

            //Aft: PYyo PRENthe NNFederal Reserve PASTconducted PYde PYde NNPTests PASTfound that…
            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("Federal"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("Reserve"));

            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("conducted"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("de"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("de"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("Tests"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("found"));
            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("that"));
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenPydeNNPPastPyde_AndMdbk_MovePastAfterYoAndNNpAfterMdbk()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    //PYde NNPTests PASTconducted PYde PYyo PRENthe NNFederal Reserve MDBKof US banks PASTfound that…
                    new Text() { pe_tag = "PY", pe_text = "de"},
                    new Text() { pe_tag = "NNP", pe_text = "Tests"},
                    new Text() { pe_tag = "PAST", pe_text = "conducted"},
                    new Text() { pe_tag = "PY", pe_text = "de"},

                    new Text() { pe_tag = "PY", pe_text = "yo", pe_merge_ahead = 3},
                    new Text() { pe_tag = "PREN", pe_text = "the"},
                    new Text() { pe_tag = "NN", pe_text = "Federal"},
                    new Text() { pe_tag = "", pe_text = "Reserve"},

                    new Text() { pe_tag = "MDBK", pe_text = "of", pe_merge_ahead = 2},
                    new Text() { pe_tag = "", pe_text = "US"},
                    new Text() { pe_tag = "", pe_text = "banks"},
                    new Text() { pe_tag = "PAST", pe_text = "found"},
                    new Text() { pe_tag = "", pe_text = "that"},
                    new Text() { pe_tag = "BKP", pe_text = " . "}
                }
            };

            PyYouUnitStrategy pyYouUnitStrategy = new PyYouUnitStrategy();
            sentence = pyYouUnitStrategy.ShuffleSentence(sentence);

            //PYyo PRENthe NNFederal Reserve PASTconducted PYde MDBKof US banks PYde NNPTests PASTfound that
            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("Federal"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("Reserve"));

            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("conducted")); //PAST
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("de"));        // PY

            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("of"));  //MDBK
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("US"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("banks"));

            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("de")); //PY
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo("Tests"));

            Assert.That(sentence.Texts[11].pe_text, Is.EqualTo("found")); //PAST
            Assert.That(sentence.Texts[12].pe_text, Is.EqualTo("that"));
            Assert.That(sentence.Texts[13].pe_text, Is.EqualTo(" . "));
        }
    }
}
