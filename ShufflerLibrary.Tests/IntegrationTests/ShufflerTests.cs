namespace ShufflerLibrary.Tests.IntegrationTests
{
    using NUnit.Framework;

    [TestFixture]
    public class ShufflerTests
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
        public void Document_Can_Be_Retrieved_Shuffled_and_Saved(int documentId)
        {
            var shuffler = new Shuffler();
            
            Assert.That(shuffler.ShuffleParagraph(documentId), Is.EqualTo(true));
        }
    }
}
