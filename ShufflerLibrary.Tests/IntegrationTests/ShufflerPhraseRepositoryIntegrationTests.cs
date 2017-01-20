namespace ShufflerLibrary.Tests.IntegrationTests
{
    using DataAccess;
    using NUnit.Framework;
    using Repository;

    [TestFixture]
    public class ShufflerPhraseRepositoryIntegrationTests
    {
        [Test]
        public void GetsMultipleSentencesFromDataSource()
        {
            var shufflerPhraseRepository = new ShufflerPhraseRepository(
                new ShufflerDataAccess());

            var document = shufflerPhraseRepository.GetShufflerDocument(2021);

            Assert.That(document.Paragraphs.Count, Is.EqualTo(15));
            Assert.That(document.Paragraphs[0].Sentences.Count, Is.EqualTo(2));
        }
    }
}