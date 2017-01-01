using NUnit.Framework;

namespace ShufflerLibrary.Tests.IntegrationTests
{
    using DataAccess;
    using Repository;
    using Strategy;

    [TestFixture]
    public class ClauserUnitStrategyTests
    {
        [Test]
        public void When_Clauser_And_NBKP_MoveToBeginningOfSentence()
        {
            // TMIn April and May NBKP, CShowever NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.
            
            var shufflerPhraseRepository 
                = new ShufflerPhraseRepository(new ShufflerDataAccess());

            var document = shufflerPhraseRepository.GetShufflerDocument(2016);
            
            var clauserUnitStrategy = 
                new ClauserUnitStrategy();
            
            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                document.Paragraphs[0].Sentences[0]);

            // CShowever NBKP, TMIn April and May NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.
            Assert.That(sentenceReturned.Texts.Count==12);
            Assert.That(sentenceReturned.Texts[0].pe_text, Is.EqualTo(" however "));
            Assert.That(sentenceReturned.Texts[1].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[2].pe_text, Is.EqualTo(" In "));
            Assert.That(sentenceReturned.Texts[3].pe_text, Is.EqualTo(" April "));
            Assert.That(sentenceReturned.Texts[4].pe_text, Is.EqualTo(" and "));
            Assert.That(sentenceReturned.Texts[5].pe_text, Is.EqualTo(" May "));
            Assert.That(sentenceReturned.Texts[6].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[7].pe_text, Is.EqualTo(" the "));
            Assert.That(sentenceReturned.Texts[8].pe_text, Is.EqualTo(" report "));
            Assert.That(sentenceReturned.Texts[9].pe_text, Is.EqualTo(" wasn’t "));
            Assert.That(sentenceReturned.Texts[10].pe_text, Is.EqualTo(" good "));
            Assert.That(sentenceReturned.Texts[11].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_Clauser_And_NulThat_WithoutNBKP_MoveToAfterNulThat()
        {
            // We were for PRENthe NNplan NULthat VBwas ADVwell PASTstructured CShowever long BKP.

            var shufflerPhraseRepository
                = new ShufflerPhraseRepository(new ShufflerDataAccess());

            var document = shufflerPhraseRepository.GetShufflerDocument(2016);

            var clauserUnitStrategy =
                new ClauserUnitStrategy();

            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                document.Paragraphs[1].Sentences[0]);

            // We were for PRENthe NNplan NULthat CShowever long VBwas ADVwell PASTstructured BKP.
            Assert.That(sentenceReturned.Texts.Count == 12);
            Assert.That(sentenceReturned.Texts[0].pe_text, Is.EqualTo(" We "));
            Assert.That(sentenceReturned.Texts[1].pe_text, Is.EqualTo(" were "));
            Assert.That(sentenceReturned.Texts[2].pe_text, Is.EqualTo(" for "));
            Assert.That(sentenceReturned.Texts[3].pe_text, Is.EqualTo(" the "));
            Assert.That(sentenceReturned.Texts[4].pe_text, Is.EqualTo(" plan "));
            Assert.That(sentenceReturned.Texts[5].pe_text, Is.EqualTo(" that "));
            Assert.That(sentenceReturned.Texts[6].pe_text, Is.EqualTo(" however "));
            Assert.That(sentenceReturned.Texts[7].pe_text, Is.EqualTo(" long "));
            Assert.That(sentenceReturned.Texts[8].pe_text, Is.EqualTo(" was "));
            Assert.That(sentenceReturned.Texts[9].pe_text, Is.EqualTo(" well "));
            Assert.That(sentenceReturned.Texts[10].pe_text, Is.EqualTo(" structured "));
            Assert.That(sentenceReturned.Texts[11].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_Clauser_WithoutNBKP_ShuffleClauserAndRestOfSentence_To_BeginningOfSentence()
        {
            
        }
    }
}
