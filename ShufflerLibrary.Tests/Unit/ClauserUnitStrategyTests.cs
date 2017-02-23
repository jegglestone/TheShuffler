using System.Collections.Generic;
using NUnit.Framework;
using ShufflerLibrary.Model;
using ShufflerLibrary.Strategy;
using Text = ShufflerLibrary.Model.Text;

namespace ShufflerLibrary.Tests.Unit
{ 

    [TestFixture]
    public class ClauserUnitStrategyTests
    {
        [Test]
        public void WhenThatFound_MoveCSandNBKPToAfterThat()
        {
            //1.1.	If ‘that’ is found, move from CS to and including NBKP to after ‘that’:
            //Bef: We VBbelieve NULthat we VBneed NNit, CShowever NNlong NNit VBtakes, to succeed BKP.
            
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "", pe_text = "We"},
                    new Text() {pe_tag = "VB", pe_text = "believe"},
                    new Text() {pe_tag = "MDNUL", pe_text = "that"},
                    new Text() {pe_tag = "", pe_text = "we"},
                    new Text() {pe_tag = "VB", pe_text = "need"},
                    new Text() {pe_tag = "NN", pe_text = "it"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "CS", pe_text = "however"},
                    new Text() {pe_tag = "NN", pe_text = "long"},
                    new Text() {pe_tag = "NN", pe_text = "it"},
                    new Text() {pe_tag = "VB", pe_text = "takes"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "", pe_text = "to"},
                    new Text() {pe_tag = "", pe_text = "succeed"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            ClauserUnitStrategy clauserUnitStrategy = new ClauserUnitStrategy();
            sentence = clauserUnitStrategy.ShuffleSentence(sentence);

            //Aft: We VBbelieve NULthat CShowever NNlong NNit VBtakesNBKP, we VBneed NNit to succeed BKP.
            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("We"));
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("believe"));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("that"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("however"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("long"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("it"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo("takes"));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo(" , "));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("we"));
            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("need"));
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo("it"));
            Assert.That(sentence.Texts[11].pe_text, Is.EqualTo(" , "));

            Assert.That(sentence.Texts[12].pe_text, Is.EqualTo("to"));
            Assert.That(sentence.Texts[13].pe_text, Is.EqualTo("succeed"));
            Assert.That(sentence.Texts[14].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void IfThatNotFound_MoveCsUpToNbkpToBeginningOfSentence()
        {
            // 1.1.	If ‘that’ is NOT found, move from CS to and including NBKP to the beginning of the sentence:

            //Bef: TMIn April and May NBKP, CShowever NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.
            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() {pe_tag = "TM", pe_text = "In"},
                    new Text() {pe_tag = "", pe_text = "April"},
                    new Text() {pe_tag = "MDNUL", pe_text = "and"},
                    new Text() {pe_tag = "", pe_text = "May"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "CS", pe_text = "however"},
                    new Text() {pe_tag = "BKP", pe_text = " , "},
                    new Text() {pe_tag = "PREN", pe_text = "the"},
                    new Text() {pe_tag = "NN", pe_text = "report"},
                    new Text() {pe_tag = "VB", pe_text = "wasn't"},
                    new Text() {pe_tag = "ADJ", pe_text = "good"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            ClauserUnitStrategy clauserUnitStrategy = new ClauserUnitStrategy();
            sentence = clauserUnitStrategy.ShuffleSentence(sentence);


            //Aft: CShowever NBKP, TMIn April and May NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.           
            Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("however")); //CS
            Assert.That(sentence.Texts[1].pe_text, Is.EqualTo(" , "));
            Assert.That(sentence.Texts[2].pe_text, Is.EqualTo("In"));
            Assert.That(sentence.Texts[3].pe_text, Is.EqualTo("April"));
            Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("and"));
            Assert.That(sentence.Texts[5].pe_text, Is.EqualTo("May"));
            Assert.That(sentence.Texts[6].pe_text, Is.EqualTo(" , "));
            Assert.That(sentence.Texts[7].pe_text, Is.EqualTo("the"));
            Assert.That(sentence.Texts[8].pe_text, Is.EqualTo("report"));
            Assert.That(sentence.Texts[9].pe_text, Is.EqualTo("wasn't"));
            Assert.That(sentence.Texts[10].pe_text, Is.EqualTo("good"));
            Assert.That(sentence.Texts[11].pe_text, Is.EqualTo(" . "));
        }

        public void NoCommaAddOneAndShuffle()
        {
            //PRENThe meeting VBwas over CSbefore he VBhad a chance VBto speak NBKP, BKP. 
        }
    }
}
