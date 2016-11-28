namespace Shuffler.Tests
{
    using Main.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class StringExtensionTests
    {
        [TestCase("A short sentenceBKP.", "BKP.", 1)]
        [TestCase("Two adjectives make you ADJsmart and ADJcorrect", "ADJ", 2)]
        [TestCase("How many times do we get a full stop and breakerBKP. I dont knowBKP. Maybe threeBKP.", "BKP.", 3)]
        public void CountTimesThisStringAppearsInThatString_CountsOccurencesOfAGivenSubString(
            string sentence, string subString, int expectedCount)
        {
            Assert.That(
                sentence.CountTimesThisStringAppearsInThatString(subString), 
                Is.EqualTo(expectedCount));
        }
    }
}
