namespace ShufflerLibrary.Tests.Unit
{
    using NUnit.Framework;
    using Text = Model.Text;

    [TestFixture]
    public class TextTests
    {
        [TestCase("TM")]
        [TestCase("TM1")]
        [TestCase("TM2")]
        public void When_Timer_Returns_true(string timerUnitTag)
        {
            var text = new Text {pe_tag = timerUnitTag, pe_tag_revised = "null"};

            var text2 = new Text {pe_tag_revised = timerUnitTag, pe_tag = "null"};

            Assert.That(text.IsTimer);
        }

        [TestCase("TMM")]
        [TestCase("DYN")]
        [TestCase("NOTTM")]
        public void When_NotTimer_Returns_false(string nonTimerTag)
        {
            var text = new Text { pe_tag = nonTimerTag, pe_tag_revised = "null"};

            var text2 = new Text { pe_tag_revised = nonTimerTag, pe_tag = "null"};

            Assert.That(text.IsTimer, Is.EqualTo(false));
        }

    }
}
