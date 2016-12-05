namespace Shuffler.Tests
{
    using NUnit.Framework;
    using Main;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Helper;

    [TestFixture]
    public class ClauserUnitStrategyTests
    {
        [Test]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentenceAndAddComma()
        {
            const string unShuffledSentence =
                "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new ClauserUnitStrategy(new ClauserUnitChecker());

            //  act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleSentenceUnit(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP."));
        }

        [Test]
        public void ShuffleClauserUnits_when_NoComma_MoveEntireClauserUnitToStartOfSentence_AndAddComma()
        {
            const string unShuffledSentence =
                "PRENThe meeting VBwas over CSbefore he VBhad a chance VBto speak BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new ClauserUnitStrategy(new ClauserUnitChecker());

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleSentenceUnit(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }

        [Test]
        public void ShuffleClauserUnits_when_clauserAtStartOfSentence_DontMoveIt()
        {
            const string unShuffledSentence =
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP.";

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new ClauserUnitStrategy(new ClauserUnitChecker());

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleSentenceUnit(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }

        [TestCase(
            "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP._Different",
            "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP.")]
        [TestCase(
            "MD1In TMMApril DYN2and TMMMay BKP, CShowever BKP, ADJthe reported NNpace MD1of NNjob NNgains PASTslowed BKP.",
            "CShowever BKP, MD1In TMMApril DYN2and TMMMay BKP, ADJthe reported NNpace MD1of NNjob NNgains PASTslowed BKP.")]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentenceAndAddsComma_WhenSpaceElementBeforeComma(
            string sentenceFile, string expectation)
        {
            string unShuffledSentence = sentenceFile;

            Paragraph paragraph =
                DocumentContentHelper.GetParagraphFromWordDocument(unShuffledSentence);

            var clauserUnitStrategy = new ClauserUnitStrategy(new ClauserUnitChecker());

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleSentenceUnit(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                expectation));

            // multiple clauser units?

            // what if the bkp is a ? or !
        }
    }
}
