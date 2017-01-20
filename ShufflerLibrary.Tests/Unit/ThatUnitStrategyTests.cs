namespace ShufflerLibrary.Tests.Unit
{
    using Model;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class NulThatUnitStrategyTests
    {
        [Test]
        public void ShufflesNulThatUnitBeforeNN()
        {
            /*
             They VBlike PRENa NNhouse NULthat VBis ADJbig BKP.
            
            1.Search for NULthat
            2.If found, search immediate left for NN and continue until no more NN is found  
                (there may be several NN one after another as in ‘organisation strategy meeting’ or ‘UN Climate Change Committee’)
                we want firstIndex of NN before NULthat
            3.Then search right for commas until reaching BKP
            4.If there is only one comma found, underline from ‘that’ all the way up to but not including the comma
            5.If there are two commas found, underline from ‘that’ all the way to the last word before BKP
            6.Now move the newly formed large unit to immediately before the last NN to the left of ‘that’ (see the word in bold here – organisation strategy meeting)
            

             They VBlike PRENa NNhouse || NULthat VBis ADJbig || BKP.
            */

            var sentenceWithNulThat = new Sentence
            {
                Texts = new List<Text>
                {
                    new Text() { pe_tag="", pe_text = "They"},
                    new Text() { pe_tag="VB", pe_text = "like"},
                    new Text() { pe_tag="PREN", pe_text = "a"},
                    new Text() { pe_tag="NN", pe_text = "house"},
                    new Text() { pe_tag="NUL", pe_text = "that"},
                    new Text() { pe_tag="VB", pe_text = "is"},
                    new Text() { pe_tag="ADJ", pe_text = "big"},
                    new Text() { pe_tag="BKP", pe_text = " . "},
                }
            };
            var thatUnitStrategy = new NulThatUnitStrategy();
            sentenceWithNulThat = thatUnitStrategy.ShuffleSentence(sentenceWithNulThat);

            /*
             They VBlike PRENa  NULthat VBis ADJbig NNhouse BKP.
             */
             Assert.That(sentenceWithNulThat.Texts[0].pe_text, Is.EqualTo("They"));
             Assert.That(sentenceWithNulThat.Texts[1].pe_text, Is.EqualTo("like"));
             Assert.That(sentenceWithNulThat.Texts[2].pe_text, Is.EqualTo("a"));
             Assert.That(sentenceWithNulThat.Texts[3].pe_text, Is.EqualTo("that"));
             Assert.That(sentenceWithNulThat.Texts[4].pe_text, Is.EqualTo("is"));
             Assert.That(sentenceWithNulThat.Texts[5].pe_text, Is.EqualTo("big"));
             Assert.That(sentenceWithNulThat.Texts[6].pe_text, Is.EqualTo("house"));
             Assert.That(sentenceWithNulThat.Texts[7].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenMultipleNNUnitsBeforeNuLThatChooseTheEarliestOne()
        {
            /*
               PRENThe NNorganization NNstrategy NNmeeting NULthat PASTtook place.
             */

            var sentenceWithNulThat = new Sentence
            {
                Texts = new List<Text>
                {
                    new Text() { pe_tag="PREN", pe_text = "The"},
                    new Text() { pe_tag="NN", pe_text = "organisation"},
                    new Text() { pe_tag="NN", pe_text = "strategy"},
                    new Text() { pe_tag="NN", pe_text = "meeting"},
                    new Text() { pe_tag="NUL", pe_text = "that"},
                    new Text() { pe_tag="PAST", pe_text = "took"},
                    new Text() { pe_tag="", pe_text = "place"},
                    new Text() { pe_tag="BKP", pe_text = " . "}
                }
            };
            var thatUnitStrategy = new NulThatUnitStrategy();
            sentenceWithNulThat = thatUnitStrategy.ShuffleSentence(sentenceWithNulThat);

            /*
             PRENThe NULthat PASTtook place NNorganization NNstrategy 
             */
            Assert.That(sentenceWithNulThat.Texts[0].pe_text, Is.EqualTo("The"));
            Assert.That(sentenceWithNulThat.Texts[1].pe_text, Is.EqualTo("that"));
            Assert.That(sentenceWithNulThat.Texts[2].pe_text, Is.EqualTo("took"));
            Assert.That(sentenceWithNulThat.Texts[3].pe_text, Is.EqualTo("place"));
            Assert.That(sentenceWithNulThat.Texts[4].pe_text, Is.EqualTo("organisation"));
            Assert.That(sentenceWithNulThat.Texts[5].pe_text, Is.EqualTo("strategy"));
            Assert.That(sentenceWithNulThat.Texts[6].pe_text, Is.EqualTo("meeting"));
            Assert.That(sentenceWithNulThat.Texts[7].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenCommaUsesThatAsTheEndOfTheNulTHat()
        {
            /*
               PRENThe NNorganization NNstrategy NNmeeting NULthat PASTtook place.
             */

            var sentenceWithNulThat = new Sentence
            {
                Texts = new List<Text>
                {
                    new Text() { pe_tag="PREN", pe_text = "The"},
                    new Text() { pe_tag="NN", pe_text = "organisation"},
                    new Text() { pe_tag="NN", pe_text = "strategy"},
                    new Text() { pe_tag="NN", pe_text = "meeting"},
                    new Text() { pe_tag="NUL", pe_text = "that"},
                    new Text() { pe_tag="PAST", pe_text = "took"},
                    new Text() { pe_tag="", pe_text = "place"},
                    new Text() { pe_tag="BKP", pe_text = " , "},
                    new Text() { pe_tag="", pe_text = "extra"},
                    new Text() { pe_tag="", pe_text = "text"},
                    new Text() { pe_tag="BKP", pe_text = " . "},
                }
            };
            var thatUnitStrategy = new NulThatUnitStrategy();
            sentenceWithNulThat = thatUnitStrategy.ShuffleSentence(sentenceWithNulThat);

            /*
             PRENThe NULthat PASTtook place NNorganization NNstrategy 
             */
            Assert.That(sentenceWithNulThat.Texts[0].pe_text, Is.EqualTo("The"));
            Assert.That(sentenceWithNulThat.Texts[1].pe_text, Is.EqualTo("that"));
            Assert.That(sentenceWithNulThat.Texts[2].pe_text, Is.EqualTo("took"));
            Assert.That(sentenceWithNulThat.Texts[3].pe_text, Is.EqualTo("place"));
            Assert.That(sentenceWithNulThat.Texts[4].pe_text, Is.EqualTo("organisation"));
            Assert.That(sentenceWithNulThat.Texts[5].pe_text, Is.EqualTo("strategy"));
            Assert.That(sentenceWithNulThat.Texts[6].pe_text, Is.EqualTo("meeting"));
            Assert.That(sentenceWithNulThat.Texts[7].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceWithNulThat.Texts[8].pe_text, Is.EqualTo("extra"));
            Assert.That(sentenceWithNulThat.Texts[9].pe_text, Is.EqualTo("text"));
            Assert.That(sentenceWithNulThat.Texts[10].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenMultipleCommasGoesToTheEndOfSentence()
        {
            /*
             I believe in the NNman NULthat NBKP, without any hesitation NBKP,  PASTgave up his life BKP.
             */
            var sentenceWithNulThat = new Sentence
            {
                Texts = new List<Text>
                {
                    new Text() { pe_tag="", pe_text = "I"},
                    new Text() { pe_tag="", pe_text = "believe"},
                    new Text() { pe_tag="", pe_text = "in"},
                    new Text() { pe_tag="", pe_text = "the"},
                    new Text() { pe_tag="NN", pe_text = "man"},
                    new Text() { pe_tag="NUL", pe_text = "that"},
                    new Text() { pe_tag="BKP", pe_text = " , "},
                    new Text() { pe_tag="", pe_text = "without"},
                    new Text() { pe_tag="", pe_text = "any"},
                    new Text() { pe_tag="", pe_text = "hesitation"},
                    new Text() { pe_tag="BKP", pe_text = " , "},
                    new Text() { pe_tag="PAST", pe_text = "gave"},
                    new Text() { pe_tag="", pe_text = "up"},
                    new Text() { pe_tag="", pe_text = "his"},
                    new Text() { pe_tag="", pe_text = "life"},
                    new Text() { pe_tag="BKP", pe_text = " . "}

                }
            };
            var thatUnitStrategy = new NulThatUnitStrategy();
            sentenceWithNulThat = thatUnitStrategy.ShuffleSentence(sentenceWithNulThat);

            /*
             I believe in the NULthat NBKP, without any hesitation NBKP,  PASTgave up his life NNman BKP.
             */
            Assert.That(sentenceWithNulThat.Texts[0].pe_text, Is.EqualTo("I"));
            Assert.That(sentenceWithNulThat.Texts[1].pe_text, Is.EqualTo("believe"));
            Assert.That(sentenceWithNulThat.Texts[2].pe_text, Is.EqualTo("in"));
            Assert.That(sentenceWithNulThat.Texts[3].pe_text, Is.EqualTo("the"));
            Assert.That(sentenceWithNulThat.Texts[4].pe_text, Is.EqualTo("that"));
            Assert.That(sentenceWithNulThat.Texts[5].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceWithNulThat.Texts[6].pe_text, Is.EqualTo("without"));
            Assert.That(sentenceWithNulThat.Texts[7].pe_text, Is.EqualTo("any"));
            Assert.That(sentenceWithNulThat.Texts[8].pe_text, Is.EqualTo("hesitation"));
            Assert.That(sentenceWithNulThat.Texts[9].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceWithNulThat.Texts[10].pe_text, Is.EqualTo("gave"));
            Assert.That(sentenceWithNulThat.Texts[11].pe_text, Is.EqualTo("up"));
            Assert.That(sentenceWithNulThat.Texts[12].pe_text, Is.EqualTo("his"));
            Assert.That(sentenceWithNulThat.Texts[13].pe_text, Is.EqualTo("life"));
            Assert.That(sentenceWithNulThat.Texts[14].pe_text, Is.EqualTo("man"));
            Assert.That(sentenceWithNulThat.Texts[15].pe_text, Is.EqualTo(" . "));
        }
    }
}
