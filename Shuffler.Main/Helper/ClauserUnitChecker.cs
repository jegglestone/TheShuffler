namespace Main.Helper
{
    using System.Xml;
    using Shuffler.Helper;

    // Example

    //<w:r w:rsidRPr="00B83388">
    //  <w:rPr>
    //    <w:rFonts w:ascii="Arial"  />
    //    <w:color w:val="000000" />
    //    <w:sz w:val="24" />
    //    <w:szCs w:val="24" />
    //    <w:u w:val="single" />
    //    <w:vertAlign w:val="superscript" />
    //  </w:rPr>
    //  <w:t>NN</w:t>
    //</w:r>


    public class ClauserUnitChecker : IUnitChecker
    {
        private readonly XmlNamespaceManager _xmlNameSpaceManager;

        public ClauserUnitChecker()
        {
            var xmlNameSpaceManager 
                = new XmlNamespaceManager(new NameTable());

            xmlNameSpaceManager.AddNamespace(
                "w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            _xmlNameSpaceManager = xmlNameSpaceManager;
        }

        public bool IsValidUnit(XmlNode xmlNode)
        {
            if (wordRunIsNotSuperScript(xmlNode))
                return false;

            return 
                xmlNode
                .InnerXml
                .Replace(" ", string.Empty)
                .Contains(">CS</w:t>");
        }

        private bool wordRunIsNotSuperScript(XmlNode xmlNode)
        {
            return xmlNode.SelectSingleNode(
                            "//w:vertAlign[@w:val='superscript']",
                            _xmlNameSpaceManager) == null;
        }
    }
}

