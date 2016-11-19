using System;
using System.Collections.Generic;
using System.Linq;

namespace CSWordRemoveBlankPage
{
    using System.Xml;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Shuffler.Helper;

    public class ClauserUnitStrategy : IClauserUnitStrategy
    {
        private readonly IUnitChecker _clauserUnitChecker;
        private const string _clauserTag = "CS";

        public ClauserUnitStrategy(IUnitChecker clauserUnitChecker)
        {
            _clauserUnitChecker = clauserUnitChecker;
        }

        public Paragraph ShuffleClauserUnits(Paragraph xmlSentenceElement)
        {
            Text[] sentenceArray = xmlSentenceElement.Descendants<Text>().ToArray();

            if (Array.Exists(
                sentenceArray, element => element.InnerText == _clauserTag))
            {
                int clauserIndexPosition =
                    Array.FindIndex(sentenceArray, i => i.InnerText == _clauserTag);

                if (IsClauserUnit(sentenceArray[clauserIndexPosition].Parent)) // what if not?
                {
                    if (clauserIndexPosition == 0)
                        return xmlSentenceElement; // no need to shuffle if clauser is already at beginning

                    if (NoCommaFollowingTheClauserUnit(sentenceArray, clauserIndexPosition))
                    {
                        Text[] beforeClauser;
                        Text[] afterClauser;

                        SplitArrayAtPosition(sentenceArray, clauserIndexPosition, out beforeClauser, out afterClauser);

                        int nextBKPPosition =
                            GetPositionOfNextBreakerUnit(afterClauser);

                        if (nextBKPPosition == -1)
                            throw new XmlException("Full stop not found in this sentence");

                        if (afterClauser[nextBKPPosition + 1].Text == ".")  // Future could be ! or ?
                        {
                            afterClauser[nextBKPPosition + 1] = new Text(",");

                            Array.Resize(ref beforeClauser, beforeClauser.Length + 2);
                            beforeClauser[beforeClauser.Length - 2] = new Text("BKP");
                            beforeClauser[beforeClauser.Length - 1] = new Text(".");

                            var arr = afterClauser.Concat(beforeClauser).ToArray();
                            List<OpenXmlElement> wordElements = BuildWordsIntoOpenXmlElement(arr);

                            var p = new Paragraph(wordElements);
                            return p;
                        }
                    }
                    else
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
                        sentenceArray[clauserIndexPosition + 1].Parent.Remove();
                        sentenceArray[clauserIndexPosition + 2].Parent.Remove();
                        sentenceArray[clauserIndexPosition + 3].Parent.Remove();
                    }
                }
            }

            return xmlSentenceElement;
        }



        private static int GetPositionOfNextBreakerUnit(Text[] arrayOfUnits)
        {
            int nextBKPPosition = -1;
            for (int i = 0; i < arrayOfUnits.Length; i++)
            {
                if (arrayOfUnits[i].Text.Contains("BKP"))
                {
                    nextBKPPosition = i;
                    break;
                }
            }

            return nextBKPPosition;
        }

        private static List<OpenXmlElement> BuildWordsIntoOpenXmlElement(Text[] textUnits)
        {
            var wordElements =
                new List<OpenXmlElement>();

            foreach (var text in textUnits)
            {
                if (text.Parent != null)
                    wordElements.Add(
                        text.Parent.CloneNode(true));
                else
                {
                    var wordRun = new Run();
                    wordRun.AppendChild(text);
                    wordElements.Add(wordRun);
                }
            }

            return wordElements;
        }

        private static bool NoCommaFollowingTheClauserUnit(Text[] sentenceArray, int clauserIndexPosition)
        {
            return sentenceArray[clauserIndexPosition + 2].InnerText.Replace(" ", "") != "BKP" ||
                                    sentenceArray[clauserIndexPosition + 3].InnerText.Replace(" ", "") != ",";
        }

        public void SplitArrayAtPosition<T>(T[] array, int index, out T[] first, out T[] second)
        {
            first = array.Take(index).ToArray();
            second = array.Skip(index).ToArray();
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
