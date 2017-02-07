namespace ShufflerLibrary.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using NUnit.Framework;
    using Strategy;
    using Text = Model.Text;

    [TestFixture]
    public class ShufflerIntegrationsTests
    {
        [TestCase(2012, TestName = "General including DG units")]
        [TestCase(2015, TestName = "Short BKBy sentence")]
        [TestCase(2016, TestName = "GeneralDocument")]
        [TestCase(2017, TestName = "Short sentences")]
        [TestCase(2018, TestName = "Several PREN and ADV units")]
        [TestCase(2019, TestName = "DIG units, PAST and DYN")]
        [TestCase(2020, TestName = "BkBy and MDKB document with multiple sentence options")]
        [TestCase(2021, TestName = "Large document")]
        [TestCase(2022, TestName = "Real GDP long sentence")]
        [TestCase(2023, TestName = "Multiple timers, MDs and commas")]
        [TestCase(2024, TestName = "Multiple MDs seperated by many breakers")]
        [TestCase(2025, TestName = "Real GDP rose percent")]
        [TestCase(2027, TestName = "Long sentence with NulThat")]
        public void Document_Can_Be_Retrieved_Shuffled_and_Saved(int documentId)
        {
            var shuffler = new Shuffler();

            Assert.That(shuffler.ShuffleParagraph(documentId), Is.EqualTo(true));
        }

        [Test]
        public void Shuffler_Routines_maintain_FullStop_At_end_of_sentences()
        {
            var sentence = LargeSentence;

            var clauserUnitStrategy =
                new ClauserUnitStrategy();
            sentence = clauserUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var adverbUnitStrategy =
                new AdverbUnitStrategy();
            sentence = adverbUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var timerUnitStrategy =
                new TimerUnitStrategy();
            sentence = timerUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var mDUnitStrategy =
                new MdUnitStrategy();
            sentence = mDUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var mdbkUnitStrategy =
                new MdbkUnitStrategy();
            sentence = mdbkUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var percentUnitStrategy =
                new PercentUnitStrategy();
            sentence = percentUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);
      
            var nulThatStrategy =
                new NulThatUnitStrategy();
            sentence = nulThatStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var doublePrenStrategy =
                new DoublePrenStrategy();
            sentence = doublePrenStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var prenNnPastUnitStrategy =
                new PrenNNPastUnitStrategy();
            sentence = prenNnPastUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);

            var commaUnitStrategy =
                 new CommaUnitStrategy();
            sentence = commaUnitStrategy.ShuffleSentence(sentence);
            Assert.That(sentence.Texts.Last().IsSentenceEnd);


    }

    private Sentence LargeSentence => new Sentence()
        {
            Texts = new List<Text>()
            {
                new Text() {pe_tag = "BK", pe_text = "if"},
                new Text() {pe_tag = "ADJ", pe_text = "so"},
                new Text() {pe_tag = "BKP", pe_text = " , "},
                new Text() {pe_tag = "PREN1", pe_text = "the"},
                new Text() {pe_tag = "NN", pe_text = "deceleration"},
                new Text() {pe_tag = "MD1", pe_text = "in"},
                new Text() {pe_tag = "N", pe_text = "employment"},
                new Text() {pe_tag = "MD2", pe_text = "in"},
                new Text() {pe_tag = "TM1", pe_text = "recent months"}, // Timer start 8
                new Text() {pe_tag = "TM2", pe_text = "may"},
                new Text() {pe_tag = "VB", pe_text = "indicate"},
                new Text() {pe_tag = "NUL", pe_text = "that"},
                new Text() {pe_tag = "PREN2", pe_text = "this"},
                new Text() {pe_tag = "NN", pe_text = "catch-up"},
                new Text() {pe_tag = "VB", pe_text = "has largely been completed"}, //7 15
                new Text() {pe_tag = "BKP", pe_text = " , "}, // End - BKP
                new Text() {pe_tag = "BK", pe_text = "and"},
                new Text() {pe_tag = "BKP", pe_text = " , "},
                new Text() {pe_tag = "ADV1", pe_text = "consequently"},
                new Text() {pe_tag = "BKP", pe_text = " , "},
                new Text() {pe_tag = "NUL", pe_text = "that"},
                new Text() {pe_tag = null, pe_text = "more-rapid"},
                new Text() {pe_tag = "NN", pe_text = "gains"},
                new Text() {pe_tag = "MD1", pe_text = "in"},
                new Text() {pe_tag = "ADJ", pe_text = "economic"},
                new Text() {pe_tag = "NN", pe_text = "activity"},
                new Text() {pe_tag = "VB", pe_text = "will be required"},
                new Text() {pe_tag = "MD1", pe_text = "to"},
                new Text() {pe_tag = "VB", pe_text = "achieve"},
                new Text() {pe_tag = "ADJ", pe_text = "significant"},
                new Text() {pe_tag = "ADJ", pe_text = "further"},
                new Text() {pe_tag = "NN", pe_text = "improvement"},
                new Text() {pe_tag = "MD1", pe_text = "in"},
                new Text() {pe_tag = "NN", pe_text = "labor market"},
                new Text() {pe_tag = "NN", pe_text = "conditions"},
                new Text() {pe_tag = "BKP", pe_text = " . "}

            }
        };

        [Test]
        public void EachStrategyInTurnMaintainsTheSEntenceEnding()
        {
            var realGDPSentence = new Sentence()
            {
                Texts = new List<Text>()
                {
                    new Text() { pe_tag_revised = "ADJ", pe_text = "Real"},
                    new Text() { pe_tag_revised = "NN", pe_text = "gdp"},
                    new Text() { pe_tag_revised = "PAST", pe_text = "rose"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "at"},
                    new Text() { pe_tag_revised = "PREN1", pe_text = "an"},
                    new Text() { pe_tag_revised = "NN", pe_text = "annual rate"},
                    new Text() { pe_tag_revised = "MD2", pe_text = "of "},
                    new Text() { pe_tag_revised = "PREN2", pe_text = "about"},
                    new Text() { pe_tag_revised = "DG", pe_text = "2"},
                    new Text() { pe_tag_revised = "NN", pe_text = "percent"},
                    new Text() { pe_tag_revised = "MD3", pe_text = "in"},
                    new Text() { pe_tag_revised = "PREN3", pe_text = "the"},
                    new Text() { pe_tag_revised = "TM1", pe_text = "first qtr"},
                    new Text() { pe_tag_revised = "CS", pe_text = "after"},     //CS 13
                    new Text() { pe_tag_revised = "PRES", pe_text = "increasing"},
                    new Text() { pe_tag_revised = "MD4", pe_text = "at"},
                    new Text() { pe_tag_revised = "PREN4", pe_text = "a"},
                    new Text() { pe_tag_revised = "DG", pe_text = "3"},
                    new Text() { pe_tag_revised = "NN", pe_text = "percent"},
                    new Text() { pe_tag_revised = "NN", pe_text = "pace"},
                    new Text() { pe_tag_revised = "MD5", pe_text = "in"},
                    new Text() { pe_tag_revised = "PREN5", pe_text = "the"},
                    new Text() { pe_tag_revised = "TM2", pe_text = "4th qtr"},
                    new Text() { pe_tag_revised = "MD6", pe_text = " of "},
                    new Text() { pe_tag_revised = "TMY", pe_text = "2011"},
                    new Text() { pe_tag_revised = "PY", pe_text = "zhihou"},
                    new Text() { pe_tag_revised = "BKP", pe_text = " . "}
                }
            };

            ClauserUnitStrategy clauserUnitStrategy = new ClauserUnitStrategy();
            realGDPSentence = clauserUnitStrategy.ShuffleSentence(realGDPSentence);

            Assert.That(realGDPSentence.Texts[0].pe_tag_revised == "CS");
            Assert.That(realGDPSentence.Texts[realGDPSentence.TextCount-1].pe_text==" . ");
           
            AdverbUnitStrategy adverbUnitStrategy = new AdverbUnitStrategy();
            realGDPSentence = adverbUnitStrategy.ShuffleSentence(realGDPSentence);

            Assert.That(realGDPSentence.Texts[realGDPSentence.TextCount - 1].pe_text == " . ");

            TimerUnitStrategy timerUnitStrategy = new TimerUnitStrategy();
            realGDPSentence = timerUnitStrategy.ShuffleSentence(realGDPSentence);

            Assert.That(realGDPSentence.Texts[realGDPSentence.TextCount - 1].pe_text == " . ");
        }
    }
}
