using System.Collections.Generic;

namespace ShufflerLibrary.Tests.Unit
{
    using Helper;
    using Model;
    using NUnit.Framework;
    using Text = Model.Text;

    [TestFixture]
    public class SentenceOrderReSetterUnitTests
    {
        [Test]
        public void GivenSentence_ReApplies_Pe_Order()
        {
            const int lowestNumber = 1214400;
            const int secondLowestNumber = 1214410;
            const int thirdLowestNumber = 1214420;
            const int secondHighestNumber = 1214430;
            const int highestNumber = 1214450;

            var sentenceUnordered = 
                new Sentence
                {
                    Texts = new List<Text>()
                };

            sentenceUnordered.Texts.AddRange(new List<Text>
            {
                new Text { pe_text = "Should be first", pe_order = highestNumber },
                new Text { pe_text = "Should be second", pe_order = secondHighestNumber },
                new Text { pe_text = "Should be third", pe_order = thirdLowestNumber },
                new Text { pe_text = "Should be fourth", pe_order = lowestNumber },
                new Text { pe_text = "Should be fifth", pe_order = secondLowestNumber },
            });

            var sentenceOrdered = 
                SentenceOrderReSetter.SetPeOrderAsc(sentenceUnordered);

            Assert.That(sentenceOrdered.Texts[0].pe_order==lowestNumber
                && sentenceOrdered.Texts[0].pe_text == "Should be first");
            Assert.That(sentenceOrdered.Texts[1].pe_order == secondLowestNumber
                && sentenceOrdered.Texts[1].pe_text == "Should be second");
            Assert.That(sentenceOrdered.Texts[2].pe_order == thirdLowestNumber
                && sentenceOrdered.Texts[2].pe_text == "Should be third");
            Assert.That(sentenceOrdered.Texts[3].pe_order == secondHighestNumber
                && sentenceOrdered.Texts[3].pe_text == "Should be fourth");
            Assert.That(sentenceOrdered.Texts[4].pe_order == highestNumber
                && sentenceOrdered.Texts[4].pe_text == "Should be fifth");
            
        }
    }
}
