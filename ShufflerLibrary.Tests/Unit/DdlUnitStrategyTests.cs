using System.Collections.Generic;
using NUnit.Framework;
using ShufflerLibrary.Model;
using ShufflerLibrary.Strategy;
using Text = ShufflerLibrary.Model.Text;

namespace ShufflerLibrary.Tests.Unit
{
    [TestFixture]
    public class DdlUnitStrategyTests
    {
        [Test]
        public void When_PyYo_Nn_Md1_MoveMdAfterYo()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "PY", pe_text = "Yo"},
                    new Text() {pe_tag = "NN", pe_text = "order"},
                    new Text() {pe_tag = "MD1", pe_text = "of", pe_merge_ahead = 0},
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "queen"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            }; 

            var ddlUnitStrategy = new DdlUnitStrategy();
            sentence = ddlUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_tag, Is.EqualTo("PY"));
            Assert.That(sentence.Texts[1].pe_tag, Is.EqualTo("MD1"));
            Assert.That(sentence.Texts[2].pe_tag, Is.EqualTo("NN"));
        }

        [Test]
        public void When_PyYo_Nn_Md1_LongerMdUnit_MoveMdUnitAfterYo()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "PY", pe_text = "Yo"},
                    new Text() {pe_tag = "NN", pe_text = "order"},
                    new Text() {pe_tag = "MD1", pe_text = "of", pe_merge_ahead = 2},
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "queen"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            var ddlUnitStrategy = new DdlUnitStrategy();
            sentence = ddlUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("of"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("queen"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("order"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_PyYo_Nn_Mdbk_ChangeNothing()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "PY", pe_text = "Yo"},
                    new Text() {pe_tag = "NN", pe_text = "order"},
                    new Text() {pe_tag = "MDBK", pe_text = "by"},
                    new Text() {pe_tag = "", pe_text = "tonight"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            var ddlUnitStrategy = new DdlUnitStrategy();
            sentence = ddlUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("order"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("by"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("tonight"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_PyYo_Nn_Md1_Md2_MoveUnitAfterPyYo()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "PY", pe_text = "Yo"},
                    new Text() {pe_tag = "NN", pe_text = "order"},
                    new Text() {pe_tag = "MD2", pe_text = "of", pe_merge_ahead = 5},
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "queen"},
                    new Text() {pe_tag = "MD1", pe_text = "of", },
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "country"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            var ddlUnitStrategy = new DdlUnitStrategy();
            sentence = ddlUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("of"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("queen"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("of"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("country"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("order"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_PyYo_Nn_Mdbk_Md1_MoveMdUnitAfterMdbk()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "PY", pe_text = "Yo"},
                    new Text() {pe_tag = "NN", pe_text = "order"},
                    new Text() {pe_tag = "MDBK", pe_text = "by"},
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "twelfth"},
                    new Text() {pe_tag = "MD1", pe_text = "of", pe_merge_ahead=2 },
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "month"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            var ddlUnitStrategy = new DdlUnitStrategy();
            sentence = ddlUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("order"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("by"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("of"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("month"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("twelfth"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo(" . "));
        }

        //5.	If it’s - PYyo + NN + MD1 + MDBK, do 1
        [Test]
        public void WhenPyYoFollowedByNnAndMd1AndMdbk_MoveMdAfterYo()
        {
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "PY", pe_text = "Yo"},
                    new Text() {pe_tag = "NN", pe_text = "order"},

                    new Text() {pe_tag = "MD1", pe_text = "of", pe_merge_ahead=2 },
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "month"},

                    new Text() {pe_tag = "MDBK", pe_text = "by"},
                    new Text() {pe_tag = "", pe_text = "the"},
                    new Text() {pe_tag = "", pe_text = "twelfth"},
  
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            var ddlUnitStrategy = new DdlUnitStrategy();
            sentence = ddlUnitStrategy.ShuffleSentence(sentence);

            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("Yo"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("of"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("month"));

            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("order"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("by"));
            
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("twelfth"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo(" . "));
        }


    }
}
