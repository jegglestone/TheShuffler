using System;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    using System.Xml;
    using Constants;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;
    using Interfaces;
    using Shuffler.Helper;

    public class ClauserUnitStrategy : IShuffleStrategy
    {
        private readonly IUnitChecker _clauserUnitChecker;
        private const string _clauserTag = "CS";

        public ClauserUnitStrategy(IUnitChecker clauserUnitChecker)
        {
            _clauserUnitChecker = clauserUnitChecker;
        }

        public Paragraph ShuffleSentenceUnit(Paragraph xmlSentenceElement)
        {
            Text[] sentenceArray = xmlSentenceElement.Descendants<Text>().ToArray();

            if (Array.Exists(
                sentenceArray, element => element.InnerText == _clauserTag))
            {
                int clauserIndexPosition =
                    Array.FindIndex(sentenceArray, i => i.InnerText == _clauserTag);

                if (IsClauserUnit(sentenceArray[clauserIndexPosition].Parent)) // what if not? - test for more 2 cs with one false -Sony EricCSon
                {
                    if (clauserIndexPosition == 0)
                        return xmlSentenceElement; // no need to shuffle if clauser is already at beginning
                    
                    if (!CommaFollowingTheClauserUnit(sentenceArray, clauserIndexPosition))
                    {
                        Text[] beforeClauser;
                        Text[] afterClauser;

                        ArrayUtility.SplitArrayAtPosition(sentenceArray, clauserIndexPosition, out beforeClauser, out afterClauser);

                        int nextBKPPosition =
                            GetPositionOfNextBreakerUnit(afterClauser);

                        if (nextBKPPosition == -1)
                            throw new XmlException("Full stop not found in this sentence");

                        if (NextBreakerIsAFullStop(afterClauser, nextBKPPosition))  // Future could be ! or ?
                        {
                            afterClauser[nextBKPPosition + 1] = new Text(",");

                            Array.Resize(ref beforeClauser, beforeClauser.Length + 2);
                            beforeClauser[beforeClauser.Length - 2] = new Text(TagMarks.BreakerPunctuation);
                            beforeClauser[beforeClauser.Length - 1] = new Text(".");

                            var arr = afterClauser.Concat(beforeClauser).ToArray();
                            List<OpenXmlElement> wordElements = OpenXmlHelper.BuildWordsIntoOpenXmlElement(arr);

                            xmlSentenceElement = new Paragraph(wordElements);
                            return xmlSentenceElement;
                        }
                    }
                    else
                    {
                        MoveClauserAndCommaToBeginningOfSentence(sentenceArray, clauserIndexPosition);
                    }
                }
            }

            return xmlSentenceElement;
        }

        private static void MoveClauserAndCommaToBeginningOfSentence(Text[] sentenceArray, int clauserIndexPosition)
        {
            bool hasAdditionalSpaceElementBeforeBreaker = 
                SentenceHasSpaceBeforeBKP(sentenceArray, clauserIndexPosition);

            sentenceArray[0].Parent.InsertBeforeSelf(
               sentenceArray[clauserIndexPosition].Parent.CloneNode(true));
            sentenceArray[0].Parent.InsertBeforeSelf(
                sentenceArray[clauserIndexPosition + 1].Parent.CloneNode(true));
            sentenceArray[0].Parent.InsertBeforeSelf(
                sentenceArray[clauserIndexPosition + 2].Parent.CloneNode(true));
            sentenceArray[0].Parent.InsertBeforeSelf(
                sentenceArray[clauserIndexPosition + 3].Parent.CloneNode(true));
            if (hasAdditionalSpaceElementBeforeBreaker)
            {
                sentenceArray[0].Parent.InsertBeforeSelf(
                    sentenceArray[clauserIndexPosition + 4].Parent.CloneNode(true));
                sentenceArray[0].Parent.InsertBeforeSelf(
                    sentenceArray[clauserIndexPosition + 5].Parent.CloneNode(true));
            }

            sentenceArray[clauserIndexPosition].Parent.Remove();
            sentenceArray[clauserIndexPosition + 1].Parent.Remove();
            sentenceArray[clauserIndexPosition + 2].Parent.Remove();
            sentenceArray[clauserIndexPosition + 3].Parent.Remove();
            if (!hasAdditionalSpaceElementBeforeBreaker) return;
            sentenceArray[clauserIndexPosition + 4].Parent.Remove();
            sentenceArray[clauserIndexPosition + 5].Parent.Remove();
        }

        private static bool NextBreakerIsAFullStop(Text[] afterClauser, int nextBKPPosition)
        {
            return afterClauser[nextBKPPosition + 1].Text == ".";
        }

        private static int GetPositionOfNextBreakerUnit(Text[] arrayOfUnits)
        {
            int nextBKPPosition = -1;
            for (int i = 0; i < arrayOfUnits.Length; i++)
            {
                if (arrayOfUnits[i].Text.Contains(TagMarks.BreakerPunctuation))
                {
                    nextBKPPosition = i;
                    break;
                }
            }

            return nextBKPPosition;
        }

        private static bool CommaFollowingTheClauserUnit(Text[] sentenceArray, int clauserIndexPosition)
        {
            if (SentenceHasSpaceBeforeBKP(sentenceArray, clauserIndexPosition))
            {
                sentenceArray = sentenceArray.RemoveAt(clauserIndexPosition + 2);
            }

            return sentenceArray[clauserIndexPosition + 2].InnerText.RemoveWhiteSpaces().IsBreakerPunctuation() &&
                   sentenceArray[clauserIndexPosition + 3].InnerText.RemoveWhiteSpaces() == ",";
        }

        private static bool SentenceHasSpaceBeforeBKP(Text[] sentenceArray, int clauserIndexPosition)
        {
            return sentenceArray[clauserIndexPosition + 2].InnerText.RemoveWhiteSpaces() == ""
                            && sentenceArray[clauserIndexPosition + 3].InnerText.RemoveWhiteSpaces().IsBreakerPunctuation();
        }


        private bool IsClauserUnit(OpenXmlElement node)
        {
            //TODO: Move this to make it reusable
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
