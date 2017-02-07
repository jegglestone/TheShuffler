using System.Collections.Generic;
using NUnit.Framework;
using ShufflerLibrary.Model;
using ShufflerLibrary.Strategy;
using Text = ShufflerLibrary.Model.Text;

namespace ShufflerLibrary.Tests.Unit
{
  [TestFixture]
  public class MdbkStrategyTests
  {
    [Test]
    public void MdbkWithMdUnit_AddDeBeforeMdbkAndMoveMDAfterMdbk()
    {
      var sentence = new Sentence()
      {
        Texts = new List<Text>()
        {
          new Text() {pe_tag = "", pe_text = "It's"},
          new Text() {pe_tag = "", pe_text = "a"},
          new Text() {pe_tag = "", pe_text = "matter"},
          new Text() {pe_tag = "MDBK", pe_text = "related"},
          new Text() {pe_tag = "", pe_text = "to"},
          new Text() {pe_tag = "PREN", pe_text = "the"},
          new Text() {pe_tag = "NN", pe_text = "integrity"},
          new Text() {pe_tag = "MD1", pe_text = "of"},
          new Text() {pe_tag = "", pe_text = "this"},
          new Text() {pe_tag = "", pe_text = "committee"},
          new Text() {pe_tag = "BKP", pe_text = " . "}
        }
      };

      MdbkUnitStrategy mdbkUnitStrategy = new MdbkUnitStrategy();
      sentence = mdbkUnitStrategy.ShuffleSentence(sentence);

      Assert.That(sentence.Texts[0].pe_text == "It's");
      Assert.That(sentence.Texts[1].pe_text == "a");
      Assert.That(sentence.Texts[2].pe_text == "matter");

      Assert.That(sentence.Texts[3].pe_text == "related");
      Assert.That(sentence.Texts[3].pe_merge_ahead == 7);

      Assert.That(sentence.Texts[4].pe_text == "to");
      Assert.That(sentence.Texts[5].pe_text == "of"); //MD1
      Assert.That(sentence.Texts[6].pe_text == "this");
      Assert.That(sentence.Texts[7].pe_text == "committee");
      Assert.That(sentence.Texts[8].pe_text == "the"); //PREN
      Assert.That(sentence.Texts[9].pe_text == "integrity"); //NN
      Assert.That(sentence.Texts[10].pe_text == " de "); // new particle
      Assert.That(sentence.Texts[11].pe_text == " . ");

    }

    [Test]
    public void MdbkWithMultiMdUnit_AddDeBeforeMdbkAndMoveMDAfterMdbk()
    {
      var sentence = new Sentence()
      {
        Texts = new List<Text>()
        {
          new Text() {pe_tag = "", pe_text = "It's"},
          new Text() {pe_tag = "", pe_text = "a"},
          new Text() {pe_tag = "", pe_text = "matter"},
          new Text() {pe_tag = "MDBK", pe_text = "related"},
          new Text() {pe_tag = "", pe_text = "to"},
          new Text() {pe_tag = "PREN", pe_text = "the"},
          new Text() {pe_tag = "NN", pe_text = "integrity"},
          new Text() {pe_tag = "MD2", pe_text = "of"},
          new Text() {pe_tag = "", pe_text = "this"},
          new Text() {pe_tag = "", pe_text = "committee"},
          new Text() {pe_tag = "MD1", pe_text = "at"},
          new Text() {pe_tag = "", pe_text = "this"},
          new Text() {pe_tag = "", pe_text = "place"},
          new Text() {pe_tag = "BKP", pe_text = " . "}
        }
      };

      MdbkUnitStrategy mdbkUnitStrategy = new MdbkUnitStrategy();
      sentence = mdbkUnitStrategy.ShuffleSentence(sentence);

      Assert.That(sentence.Texts[0].pe_text == "It's");
      Assert.That(sentence.Texts[1].pe_text == "a");
      Assert.That(sentence.Texts[2].pe_text == "matter");

      Assert.That(sentence.Texts[3].pe_text == "related");
      Assert.That(sentence.Texts[3].pe_merge_ahead == 10);

      Assert.That(sentence.Texts[4].pe_text == "to");
      Assert.That(sentence.Texts[5].pe_text == "of"); //MD2
      Assert.That(sentence.Texts[6].pe_text == "this");
      Assert.That(sentence.Texts[7].pe_text == "committee");
      Assert.That(sentence.Texts[8].pe_text == "at"); //MD1
      Assert.That(sentence.Texts[9].pe_text == "this");
      Assert.That(sentence.Texts[10].pe_text == "place");
      Assert.That(sentence.Texts[11].pe_text == "the"); //PREN
      Assert.That(sentence.Texts[12].pe_text == "integrity"); //NN
      Assert.That(sentence.Texts[13].pe_text == " de "); // new particle
      Assert.That(sentence.Texts[14].pe_text == " . ");
    }


    [Test]
    public void MdbkWithMoMdUnit_AddDeAfterLastNnAfterMdbk()
    {
      var sentence = new Sentence()
      {
        //…exaggerated by issues MDBKrelated to ADJseasonal NNadjustment deBKP. 
        Texts = new List<Text>()
        {
           new Text() {pe_tag = "", pe_text = "It's"},
           new Text() {pe_tag = "", pe_text = "exaggerated"},
           new Text() {pe_tag = "", pe_text = "by"},
           new Text() {pe_tag = "", pe_text = "issues"},
           new Text() {pe_tag = "MDBK", pe_text = "related"},
           new Text() {pe_tag = "", pe_text = "to"},
           new Text() {pe_tag = "ADJ", pe_text = "seasonal"},
           new Text() {pe_tag = "NN", pe_text = "adjustment"},
           new Text() {pe_tag = "NN", pe_text = "things"},
           new Text() {pe_tag = "BKP", pe_text = " . "}
        }
      };

      MdbkUnitStrategy mdbkUnitStrategy = new MdbkUnitStrategy();
      sentence = mdbkUnitStrategy.ShuffleSentence(sentence);

      Assert.That(sentence.Texts[0].pe_text == "It's");
      Assert.That(sentence.Texts[1].pe_text == "exaggerated");
      Assert.That(sentence.Texts[2].pe_text == "by");

      Assert.That(sentence.Texts[3].pe_text == "issues");
     
      Assert.That(sentence.Texts[4].pe_text == "related");
      Assert.That(sentence.Texts[4].pe_merge_ahead == 5);

      Assert.That(sentence.Texts[5].pe_text == "to");
      Assert.That(sentence.Texts[6].pe_text == "seasonal");
      Assert.That(sentence.Texts[7].pe_text == "adjustment");
      Assert.That(sentence.Texts[8].pe_text == "things"); 
      Assert.That(sentence.Texts[9].pe_text == " de "); // new particle
      Assert.That(sentence.Texts[10].pe_text == " . ");
    }
  }
}
