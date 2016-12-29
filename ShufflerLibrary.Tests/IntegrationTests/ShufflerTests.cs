namespace ShufflerLibrary.Tests.IntegrationTests
{
    using NUnit.Framework;

    [TestFixture]
    public class ShufflerTests
    {
        [Test]
        public void Document_Can_Be_Retrieved_Shuffled_and_Saved()
        {
            var shuffler = new Shuffler();
            
            Assert.That(shuffler.ShuffleParagraph(2016), Is.EqualTo(true));
        }
    }
}
