namespace Shuffler.Tests
{
    using NUnit.Framework;
    using System.Text;
    using System.Xml.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Helper;

    [TestFixture]
    public class DocumentFormatterTests
    {    

        [Test]
        public void ShuffleClauserUnits_ToTheBeginningOfTheSentenceAndAddComma()
        {

            Paragraph paragraph = 
                new Paragraph {InnerXml = GetUnShuffledParagraphWithClauserUnit()};

            var documentFormatter = new DocumentFormatter(new ClauserUnitChecker());

            // act
            var shufflerParagraph = 
                documentFormatter.ShuffleClauserUnits(paragraph);

            // assert
            Assert.That(shufflerParagraph.InnerText, Is.EqualTo(
                "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        }

        //[Test]
        //public void ShuffleClauserUnits_when_clauserAtStartOfSentence_DontMoveIt()
        //{
        //    var mockRange = new Mock<Range>();
        //    mockRange.Setup(f => f.Text).Returns(
        //        "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP.");
        //    var sentence = mockRange.Object;

        //    var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

        //    // act
        //    documentFormatter.ShuffleClauserUnits(sentence);

        //    // assert
        //    Assert.That(sentence.Text, Is.EqualTo(
        //        "CSbefore he VBhad a chance VBto speak BKP, PRENThe meeting VBwas over BKP."));
        //}


        //[Test]
        //public void ShuffleClauserUnits_when_clauserHasComma_MoveTheClauserAndCommaToStartOfSentence()
        //{
        //    var mockRange = new Mock<Range>();
        //    mockRange.Setup(f => f.Text).Returns(
        //        "TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP.");
        //    var sentence = mockRange.Object;

        //    var documentFormatter = new DocumentFormatter(GetClauserUnitChecker());

        //    // act
        //    documentFormatter.ShuffleClauserUnits(sentence);

        //    // assert
        //    Assert.That(sentence.Text, Is.EqualTo(
        //        "CShowever BKP, TMIn April and May BKP, PRENthe NNreport VBwasn’t ADJgood BKP."));
        //}

        // multiple clauser units?
        // Does a clauser unit only end with BKP?

        /// <summary>
        /// Returns word XML for the following string
        /// TMIn April and May BKP, CShowever BKP, PRENthe NNreport VBwasn’t ADJgood BKP.
        /// </summary>
        /// <returns></returns>
        private static string GetUnShuffledParagraphWithClauserUnit()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml version='1.0' encoding='utf-8' xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\"><w:p w:rsidR=\"002D50D3\" w:rsidRDefault=\"002D50D3\">");
            sb.Append("   <w:pPr>");
            sb.Append("     <w:spacing w:after=\"0\" w:line=\"240\" w:lineRule=\"auto\" />");
            sb.Append("     <w:rPr>");
            sb.Append("       <w:vertAlign w:val=\"subscript\" />");
            sb.Append("     </w:rPr>");
            sb.Append("   </w:pPr>");
            sb.Append("   <w:proofErr w:type=\"spellStart\" />");
            sb.Append("   <w:r>");
            sb.Append("   <w:rPr>");
            sb.Append("       <w:vertAlign w:val=\"superscript\" />");
            sb.Append("   </w:rPr>");
            sb.Append("   <w:t>TM</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("   <w:rPr>");
            sb.Append("   <w:lang w:val=\"en-US\" />");
            sb.Append("   </w:rPr>");
            sb.Append("   <w:t>In</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellEnd\" ");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"> April and</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:u w:val=\"single\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>May</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>BKP</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>,</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellStart\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>CS</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("        </w:rPr>");
            sb.Append("        <w:t>however</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellEnd\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("        <w:t>BKP</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>,</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellStart\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>PREN</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>the</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellEnd\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:rFonts w:ascii=\"Arial\" w:hAnsi=\"Arial\" w:eastAsia=\"SimSun\" w:cs=\"Arial\" />");
            sb.Append("   	</w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellStart\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>NN</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>report</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellEnd\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellStart\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>VB</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>wasn’t</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellEnd\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellStart\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>ADJ</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>good</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:proofErr w:type=\"spellEnd\" />");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:lang w:val=\"en-US\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t xml:space=\"preserve\"></w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:vertAlign w:val=\"superscript\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>BKP</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   <w:r>");
            sb.Append("       <w:rPr>");
            sb.Append("           <w:u w:val=\"single\" />");
            sb.Append("       </w:rPr>");
            sb.Append("       <w:t>.</w:t>");
            sb.Append("   </w:r>");
            sb.Append("   </w:p></xml>");

            return sb.ToString();
        }
    }
}
