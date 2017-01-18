namespace ShufflerLibrary.Tests.Unit
{
    using Model;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class ThatUnitStrategyTests
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
            var thatUnitStrategy = new ThatUnitStrategy();
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
    }
}
