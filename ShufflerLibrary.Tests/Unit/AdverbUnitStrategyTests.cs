namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;
    using System.Linq;

    [TestFixture]
    public class AdverbUnitStrategyTests
    {
        [Test]
        public void When_Adverb_Preceeded_By_VB_PAST_PRES_Move_Adverb_Before_It()
        {
            // He PASTshouted ADVloudlyNBKP, ADVemotionally BKand ADVnon - stop BKP.

            var sentence = new Sentence()
            {
                Texts = new List<Model.Text>
                {
                    new Model.Text(){ pe_text = " He ",          pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663970 },
                    new Model.Text(){ pe_text = " shouted ",     pe_tag="PAST", pe_tag_revised="NULL", pe_order=1663980 },
                    new Model.Text(){ pe_text = " loudly ",      pe_tag="ADV", pe_tag_revised="ADV1", pe_order=1663990 },
                    new Model.Text(){ pe_text = " , ",           pe_tag="NBKP", pe_tag_revised="NULL", pe_order=1664000 },
                    new Model.Text(){ pe_text = " emotionally ", pe_tag="ADV", pe_tag_revised="NULL", pe_order=1664010 },
                    new Model.Text(){ pe_text = " and ",         pe_tag="DYN2", pe_tag_revised="BK", pe_order=1664020 },
                    new Model.Text(){ pe_text = " non-stop ",    pe_tag="ADV", pe_tag_revised="NULL", pe_order=1664030 },
                    new Model.Text(){ pe_text = " . ",           pe_tag="BKP",  pe_tag_revised="NULL", pe_order=1664040 },
                },
                pe_para_no = 123
            };
            
            var adverbUnitStrategy = new AdverbUnitStrategy();

            var returnedSentence = adverbUnitStrategy.ShuffleSentence(sentence);

            // He ADVloudlyNBKP, ADVemotionally BKand ADVnon-stop PASTshouted BKP
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" He "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" loudly "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" , "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" emotionally "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" and "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" non-stop "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" shouted "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.pe_para_no == 123);
        }

        [Test]
        public void When_Adverb_Preceeded_By_PRES_Move_Adverb_Before_PRES()
        {
            // He is PRESdoing it ADVconsistently and ADVcarefully BKP.

            var sentence = new Sentence()
            {
                Texts = new List<Model.Text>
                {
                    new Text(){ pe_text = " He ",          pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " is ",     pe_tag="DYN7", pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " doing ",      pe_tag="PRES", pe_tag_revised="NULL", pe_order=1663990 },
                    new Text(){ pe_text = " it ",           pe_tag="NN", pe_tag_revised="NULL", pe_order=1664000 },
                    new Text(){ pe_text = " consistently ", pe_tag="ADV", pe_tag_revised="ADV1", pe_order=1664010 },
                    new Text(){ pe_text = " and ",         pe_tag="", pe_tag_revised="NN", pe_order=1664020 },
                    new Text(){ pe_text = " carefully ",    pe_tag="ADV", pe_tag_revised="ADV2", pe_order=1664030 },
                    new Text(){ pe_text = " . ",           pe_tag="BKP",  pe_tag_revised="NULL", pe_order=1664040 },
                }
            };

            var adverbUnitStrategy = new AdverbUnitStrategy();

            var returnedSentence = adverbUnitStrategy.ShuffleSentence(sentence);

            // He is ADVconsistently and ADVcarefully PRESdoing it BKP.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" He "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" is "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" consistently "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" and "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" carefully "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" doing "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" it "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_No_Adverb_Does_Not_Shuffle()
        {
            // He PASTshouted loudlyNBKP, emotionally BKand non-stop BKP.
            var sentence = new Sentence()
            {
                Texts = new List<Model.Text>
                {
                    new Text(){ pe_text = " He ",          pe_tag="NN",   pe_tag_revised="NULL", pe_order=1663970 },
                    new Text(){ pe_text = " shouted ",     pe_tag="PAST", pe_tag_revised="NULL", pe_order=1663980 },
                    new Text(){ pe_text = " loudly ",      pe_tag="NULL", pe_tag_revised="VB", pe_order=1663990 },
                    new Text(){ pe_text = " , ",           pe_tag="NBKP", pe_tag_revised="NULL", pe_order=1664000 },
                    new Text(){ pe_text = " emotionally ", pe_tag="NULL", pe_tag_revised="VB", pe_order=1664010 },
                    new Text(){ pe_text = " and ",         pe_tag="DYN2", pe_tag_revised="BK", pe_order=1664020 },
                    new Text(){ pe_text = " non-stop ",    pe_tag="ADV", pe_tag_revised="VB", pe_order=1664030 },
                    new Text(){ pe_text = " . ",           pe_tag="BKP",  pe_tag_revised="NULL", pe_order=1664040 },
                },
                pe_para_no = 123
            };

            var adverbUnitStrategy = new AdverbUnitStrategy();

            var returnedSentence = adverbUnitStrategy.ShuffleSentence(sentence);

            // He PASTshouted loudlyNBKP, emotionally BKand non-stop BKP.
            Assert.That(returnedSentence.Texts[0].pe_text, Is.EqualTo(" He "));
            Assert.That(returnedSentence.Texts[1].pe_text, Is.EqualTo(" shouted "));
            Assert.That(returnedSentence.Texts[2].pe_text, Is.EqualTo(" loudly "));
            Assert.That(returnedSentence.Texts[3].pe_text, Is.EqualTo(" , "));
            Assert.That(returnedSentence.Texts[4].pe_text, Is.EqualTo(" emotionally "));
            Assert.That(returnedSentence.Texts[5].pe_text, Is.EqualTo(" and "));
            Assert.That(returnedSentence.Texts[6].pe_text, Is.EqualTo(" non-stop "));
            Assert.That(returnedSentence.Texts[7].pe_text, Is.EqualTo(" . "));

            Assert.That(returnedSentence.pe_para_no == 123);
        }

        [Test]
        public void When_Adverb_And_Two_Timers_Does_Not_Duplicate()
        {
            // this is a bug fix

            var sentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text { pe_tag="NN", pe_text ="Economic growth"},
                    new Text { pe_tag="VB", pe_text ="has continued"},
                    new Text { pe_tag="ADV1", pe_text ="at a moderate rate"},
                    new Text { pe_tag="TM1", pe_text ="so far"},
                    new Text { pe_tag="TM2", pe_text ="this year"},
                    new Text { pe_tag="BKP", pe_text =" . "}
                }
            };

            var adverbUnitStrategy = new AdverbUnitStrategy();
            var returnedSentence = adverbUnitStrategy.ShuffleSentence(sentence);

            Assert.That(
                returnedSentence.Texts.Count(text => text.pe_text == "at a moderate rate"), Is.EqualTo(1));
        }
    }
}
