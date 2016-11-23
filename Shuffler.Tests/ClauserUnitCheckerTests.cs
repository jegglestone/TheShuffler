namespace Shuffler.Tests
{
    using System.Text;
    using System.Xml;
    using NUnit.Framework;
    using Helper;

    [TestFixture]
    public class ClauserUnitCheckerTests
    {
        private ClauserUnitChecker _clauserUnitChecker;

        [SetUp]
        public void Setup()
        {
            _clauserUnitChecker = new ClauserUnitChecker();
        }

        [Test]
        public void IsValidUnit_When_Superscript_And_CS_ReturnsTrue()
        {
            // arrange
            var plainXmlBuilder = new StringBuilder();
            plainXmlBuilder.Append("<xml xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
            plainXmlBuilder.Append("<w:r w:rsidRPr=\"00B83388\">");
            plainXmlBuilder.Append(" <w:rPr>");
            plainXmlBuilder.Append("   <w:rFonts />");
            plainXmlBuilder.Append("   <w:vertAlign w:val=\"superscript\" />");
            plainXmlBuilder.Append(" </w:rPr>");
            plainXmlBuilder.Append(" <w:t>CS</w:t>");
            plainXmlBuilder.Append("</w:r></xml>");

            var doc = new
                XmlDocument();
            doc.LoadXml(plainXmlBuilder.ToString());

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            XmlNode xmlNode = doc.SelectSingleNode("//w:r", nsMgr);
           
            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(
                xmlNode), Is.EqualTo(true));
        }

        [Test]
        public void IsValidUnit_Ignores_WhiteSpace_when_evalating_WordRun_Value()
        {
            // arrange
            var plainXmlBuilder = new StringBuilder();
            plainXmlBuilder.Append("<xml xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
            plainXmlBuilder.Append("<w:r w:rsidRPr=\"00B83388\">");
            plainXmlBuilder.Append(" <w:rPr>");
            plainXmlBuilder.Append("   <w:rFonts />");
            plainXmlBuilder.Append("   <w:vertAlign w:val=\"superscript\" />");
            plainXmlBuilder.Append(" </w:rPr>");
            plainXmlBuilder.Append(" <w:t> CS </w:t>");
            plainXmlBuilder.Append("</w:r></xml>");

            var doc = new
                XmlDocument();
            doc.LoadXml(plainXmlBuilder.ToString());

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            XmlNode xmlNode = doc.SelectSingleNode("//w:r", nsMgr);

            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(
                xmlNode), Is.EqualTo(true));
        }

        [Test]
        public void IsValidUnit_When_NormalFont_ReturnsFalse()
        {
            // arrange
            var plainXmlBuilder = new StringBuilder();
            plainXmlBuilder.Append("<xml xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
            plainXmlBuilder.Append("<w:r w:rsidRPr=\"00B83388\">");
            plainXmlBuilder.Append(" <w:rPr>");
            plainXmlBuilder.Append("   <w:rFonts />");
            plainXmlBuilder.Append("   <w:vertAlign w:val=\"normal\" />");
            plainXmlBuilder.Append(" </w:rPr>");
            plainXmlBuilder.Append(" <w:t>CS</w:t>");
            plainXmlBuilder.Append("</w:r></xml>");

            var doc = new
                XmlDocument();
            doc.LoadXml(plainXmlBuilder.ToString());

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            XmlNode xmlNode = doc.SelectSingleNode("//w:r", nsMgr);


            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(
                xmlNode), Is.EqualTo(false));
        }

        [Test]
        public void IsValidUnit_When_Not_CS_ReturnsFalse()
        {
            // arrange
            var plainXmlBuilder = new StringBuilder();
            plainXmlBuilder.Append("<xml xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
            plainXmlBuilder.Append("<w:r w:rsidRPr=\"00B83388\">");
            plainXmlBuilder.Append(" <w:rPr>");
            plainXmlBuilder.Append("   <w:rFonts />");
            plainXmlBuilder.Append("   <w:vertAlign w:val=\"superscript\" />");
            plainXmlBuilder.Append(" </w:rPr>");
            plainXmlBuilder.Append(" <w:t>YY</w:t>");
            plainXmlBuilder.Append("</w:r></xml>");

            var doc = new
                XmlDocument();
            doc.LoadXml(plainXmlBuilder.ToString());

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            XmlNode xmlNode = doc.SelectSingleNode("//w:r", nsMgr);

            // AA
            Assert.That(_clauserUnitChecker.IsValidUnit(
                xmlNode), Is.EqualTo(false));
        }
    }
}
