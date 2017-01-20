namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class PrenNNPastUnitStrategyTests
    {
        //[Test]
        public void WhenMdBkByOrTMThenMove()
        {
            /*
             1.	Search for the structure ‘PREN + NN + PAST’ or ‘NN + PAST’
             2.	If found, search for ‘BKby’, MD or TM until reaching VB/NBKP/BKP. For example, the following sentence -
            
                   Output was VBup from PRENabout DG150,000 NNjobs PASTadded TM1per month TM2in 2011 BKP.

             * */

            var sentenceWithPrenNNPastUnit = new Sentence()
            {
                Texts=new List<Text>()
                {
                    new Text() {pe_tag = null, pe_text = "Output"},
                    new Text() {pe_tag = null, pe_text = "was"},
                    new Text() {pe_tag = "VB", pe_text = "up"},
                    new Text() {pe_tag = null, pe_text = "from"},
                    new Text() {pe_tag = "PREN", pe_text = "about"},
                    new Text() {pe_tag = "DG", pe_text = "150,000"},
                    new Text() {pe_tag = "NN", pe_text = "jobs"},
                    new Text() {pe_tag = "PAST", pe_text = "added"},
                    new Text() {pe_tag = "TM1", pe_text = "per"},
                    new Text() {pe_tag = null, pe_text = "month"},
                    new Text() {pe_tag = "TM2", pe_text = "in"},
                    new Text() {pe_tag = null, pe_text = "2011"},
                    new Text() {pe_tag = "BKP", pe_text = " . "}
                }
            };

            PrenNNPastUnitStrategy prenNnPastUnitStrategy = 
                new PrenNNPastUnitStrategy();
            sentenceWithPrenNNPastUnit = 
                prenNnPastUnitStrategy.ShuffleSentence(
                    sentenceWithPrenNNPastUnit);

            /*
                If either MD/TM is found –
                3.1.	Move MD/TM in descending order to before PAST
                3.2.	Underline from MD/TM to and including PAST
                   Output was VBup from PRENabout DG150,000 NNjobs TM2in 2011 TM1per month PASTadded BKP.
             * */

            Assert.That(sentenceWithPrenNNPastUnit.Texts[0].pe_text, Is.EqualTo("Output"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[1].pe_text, Is.EqualTo("was"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[2].pe_text, Is.EqualTo("up"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[3].pe_text, Is.EqualTo("from"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[4].pe_text, Is.EqualTo("about"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[5].pe_text, Is.EqualTo("150,000"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[6].pe_text, Is.EqualTo("jobs"));

            Assert.That(sentenceWithPrenNNPastUnit.Texts[7].pe_text, Is.EqualTo("in"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[8].pe_text, Is.EqualTo("2011"));

            Assert.That(sentenceWithPrenNNPastUnit.Texts[9].pe_text, Is.EqualTo("per"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[10].pe_text, Is.EqualTo("month"));
            Assert.That(sentenceWithPrenNNPastUnit.Texts[11].pe_text, Is.EqualTo("added"));

            Assert.That(sentenceWithPrenNNPastUnit.Texts[12].pe_text, Is.EqualTo(" . "));
        }
    }
}
