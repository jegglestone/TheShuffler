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
            Assert.That(text2.IsTimer);
        }

        [TestCase("TMM")]
        [TestCase("DYN")]
        [TestCase("NOTTM")]
        public void When_NotTimer_Returns_false(string nonTimerTag)
        {
            var text = new Text { pe_tag = nonTimerTag, pe_tag_revised = "null"};

            var text2 = new Text { pe_tag_revised = nonTimerTag, pe_tag = "null"};

            Assert.That(text.IsTimer, Is.EqualTo(false));
            Assert.That(text2.IsTimer, Is.EqualTo(false));
        }

        [Test]
        public void When_tag_IsNull_Returns_false()
        {
            var text = new Text() {pe_tag="BKP", pe_tag_revised=null};

            var text2 = new Text() { pe_tag=null, pe_tag_revised = "BKP"};

            Assert.That(text.IsTimer, Is.EqualTo(false));
            Assert.That(text2.IsTimer, Is.EqualTo(false));
        }

        [Test]
        public void When_tag_IsNull_but_other_Is_TM_Returns_true()
        {
            var text = new Text() { pe_tag = "TM2", pe_tag_revised = null };

            var text2 = new Text() { pe_tag = null, pe_tag_revised = "TM" };

            Assert.That(text.IsTimer, Is.EqualTo(true));
            Assert.That(text2.IsTimer, Is.EqualTo(true));
        }

        [Test]
        public void When_Both_Tags_Null_return_false()
        {
            var text = new Text {pe_tag = null, pe_tag_revised = null};

            Assert.That(text.IsModifier, Is.EqualTo(false));
        }
    }
}
