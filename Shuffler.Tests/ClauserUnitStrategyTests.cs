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

            // act
            var shufflerParagraph =
                clauserUnitStrategy.ShuffleClauserUnits(paragraph);

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
                clauserUnitStrategy.ShuffleClauserUnits(paragraph);
            
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
                clauserUnitStrategy.ShuffleClauserUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }


        // multiple clauser units?

        // what if the bkp is a ? or !
    }
}
