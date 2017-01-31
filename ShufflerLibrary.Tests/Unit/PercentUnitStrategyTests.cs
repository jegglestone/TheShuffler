using System.Collections.Generic;
using NUnit.Framework;
using ShufflerLibrary.Model;
using ShufflerLibrary.Strategy;
using Text = ShufflerLibrary.Model.Text;

namespace ShufflerLibrary.Tests.Unit
{
  [TestFixture]
  public class PercentUnitStrategyTests
  {
    [Test]
    public void MovesPercentBeforeDIG()
    {
      var sentence = new Sentence
      {
        Texts = new List<Text>()
        {
          new Text() {pe_tag = "", pe_text = "unemployment"},
          new Text() {pe_tag = "PAST", pe_text = "was"},
          new Text() {pe_tag = "DIG", pe_text = " 8.2 "},
          new Text() {pe_tag = "", pe_text = " percent "},
          new Text() {pe_tag = "BKP", pe_text = "."},
        }
      };
       
      var percentUnitStrategy = new PercentUnitStrategy();
      sentence = percentUnitStrategy.ShuffleSentence(sentence);

      Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("unemployment"));
      Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("was"));
      Assert.That(sentence.Texts[2].pe_text, Is.EqualTo(" percent "));
      Assert.That(sentence.Texts[3].pe_text, Is.EqualTo(" 8.2 "));
      Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("."));
    }

    [Test]
    public void MovesPercentSymbolBeforeDIG()
    {
      var sentence = new Sentence
      {
        Texts = new List<Text>()
        {
          new Text() {pe_tag = "", pe_text = "unemployment"},
          new Text() {pe_tag = "PAST", pe_text = "was"},
          new Text() {pe_tag = "DIG", pe_text = " 8.2 "},
          new Text() {pe_tag = "", pe_text = " % "},
          new Text() {pe_tag = "BKP", pe_text = "."},
        }
      };

      var percentUnitStrategy = new PercentUnitStrategy();
      sentence = percentUnitStrategy.ShuffleSentence(sentence);

      Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("unemployment"));
      Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("was"));
      Assert.That(sentence.Texts[2].pe_text, Is.EqualTo(" % "));
      Assert.That(sentence.Texts[3].pe_text, Is.EqualTo(" 8.2 "));
      Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("."));
    }

    [Test]
    public void DoesNotMovePercentIfNoNumberInFront()
    {
      var sentence = new Sentence
      {
        Texts = new List<Text>()
        {
          new Text() {pe_tag = "", pe_text = "unemployment"},
          new Text() {pe_tag = "PAST", pe_text = "was"},
          new Text() {pe_tag = "", pe_text = " a few "},
          new Text() {pe_tag = "", pe_text = " percent "},
          new Text() {pe_tag = "BKP", pe_text = "."},
        }
      };

      var percentUnitStrategy = new PercentUnitStrategy();
      sentence = percentUnitStrategy.ShuffleSentence(sentence);

      Assert.That(sentence.Texts[0].pe_text, Is.EqualTo("unemployment"));
      Assert.That(sentence.Texts[1].pe_text, Is.EqualTo("was"));
      Assert.That(sentence.Texts[2].pe_text, Is.EqualTo(" a few "));
      Assert.That(sentence.Texts[3].pe_text, Is.EqualTo(" percent "));
      Assert.That(sentence.Texts[4].pe_text, Is.EqualTo("."));
    }
  }
}
