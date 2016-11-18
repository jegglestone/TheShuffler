namespace Shuffler.Helper
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class DocumentFormatter
    {
        private IUnitChecker _clauserUnitChecker;
        private const string _clauserTag = "CS";

        public DocumentFormatter(IUnitChecker clauserUnitChecker)
        {
            _clauserUnitChecker = clauserUnitChecker;
        }

        public bool ProcessDocument(MainDocumentPart docPart)
        {
            OpenXmlElement documentBodyXml = docPart.Document.Body;

            foreach (var element in documentBodyXml.Elements())
            {
                if (element.LocalName == "p")
                {
                    //ShuffleClauserUnits(element as Paragraph);
                }
            }

            return false;
        }

        public Paragraph ShuffleClauserUnits(Paragraph xmlSentenceElement)
        {
            Text[] sentenceArray = xmlSentenceElement.Descendants<Text>().ToArray();
            
            if (Array.Exists(
                sentenceArray, element => element.InnerText == _clauserTag))
            {
                int clauserIndexPosition = 
                    Array.FindIndex(sentenceArray, i => i.InnerText == _clauserTag);

                if (IsClauserUnit(sentenceArray[clauserIndexPosition].Parent))
                {
                    // if theres a comma afterward get the position of it
                    if (sentenceArray[clauserIndexPosition + 2].InnerText.Replace(" ", "") == "BKP")
                    {
                        //   move the clauser and comma to the beginning
                        OpenXmlElement tmpClauserVar = sentenceArray[clauserIndexPosition].Parent;
                        OpenXmlElement tmpClauserWord = sentenceArray[clauserIndexPosition + 1].Parent;
                        OpenXmlElement tmpCommaVar = sentenceArray[clauserIndexPosition + 2].Parent;
                        OpenXmlElement tmpComma = sentenceArray[clauserIndexPosition + 3].Parent;

                        sentenceArray[0].Parent.InsertBeforeSelf(tmpClauserVar.CloneNode(true));
                        sentenceArray[0].Parent.InsertBeforeSelf(tmpClauserWord.CloneNode(true));
                        sentenceArray[0].Parent.InsertBeforeSelf(tmpCommaVar.CloneNode(true));
                        sentenceArray[0].Parent.InsertBeforeSelf(tmpComma.CloneNode(true));
                      
                        sentenceArray[clauserIndexPosition].Parent.Remove();
                        sentenceArray[clauserIndexPosition+1].Parent.Remove();
                        sentenceArray[clauserIndexPosition + 2].Parent.Remove();
                        sentenceArray[clauserIndexPosition + 3].Parent.Remove();
                        
                        //sentenceArray[0].Parent.InsertBefore(tmpClauserVar, sentenceArray[3].Parent);
                        //sentenceArray[1].Parent.InsertAfter(tmpClauserWord, sentenceArray[4]);
                        //sentenceArray[2].Parent.InsertAfter(tmpCommaVar, sentenceArray[5]);
                    }
                    // else
                    //  get the ending point (bkp)
                    //  move the remainder to the start of sentence
                    //  add a comma 

                    // move the block to the beginning of the sentence

                }
            }
            
            return xmlSentenceElement;
        }

        private bool IsClauserUnit(OpenXmlElement node)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<xml xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">"
                + node.OuterXml + "</xml>");

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            XmlNode xmlNode = doc.SelectSingleNode("//w:r", nsMgr);
            return _clauserUnitChecker.IsValidUnit(xmlNode);
        }
    }
}
