using System;
using System.Collections.Generic;
using System.Linq;

namespace Main
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

                                    //sentenceArray[clauserIndexPosition + 2].Parent.Remove(clauserIndexPosition + 2);

                    if (!CommaFollowingTheClauserUnit(sentenceArray, clauserIndexPosition))
                    {
                        Text[] beforeClauser;
                        Text[] afterClauser;

                        SplitArrayAtPosition(sentenceArray, clauserIndexPosition, out beforeClauser, out afterClauser);

                        int nextBKPPosition =
                            GetPositionOfNextBreakerUnit(afterClauser);

                        if (nextBKPPosition == -1)
                            throw new XmlException("Full stop not found in this sentence");

                        if (NextBreakerIsAFullStop(afterClauser, nextBKPPosition))  // Future could be ! or ?
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
                        MoveClauserAndCommaToBeginningOfSentence(sentenceArray, clauserIndexPosition);
                    }
                }
            }

            return xmlSentenceElement;
        }

        private static void MoveClauserAndCommaToBeginningOfSentence(Text[] sentenceArray, int clauserIndexPosition)
        {
            bool hasAdditionalSpaceElementBeforeBreaker = SentenceHasSpaceBeforeBKP(sentenceArray,
                clauserIndexPosition);

            // move the clauser and comma to the beginning
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

        private static bool CommaFollowingTheClauserUnit(Text[] sentenceArray, int clauserIndexPosition)
        {
            if (SentenceHasSpaceBeforeBKP(sentenceArray, clauserIndexPosition))
            {
                sentenceArray = sentenceArray.RemoveAt(clauserIndexPosition + 2);
            }

            //if (sentenceArray[clauserIndexPosition + 3].InnerText.Replace(" ", "") == "")
            //    sentenceArray[clauserIndexPosition + 3].Parent.Remove();

            return sentenceArray[clauserIndexPosition + 2].InnerText.Replace(" ", "") == "BKP" &&
                   sentenceArray[clauserIndexPosition + 3].InnerText.Replace(" ", "") == ",";
        }

        private static bool SentenceHasSpaceBeforeBKP(Text[] sentenceArray, int clauserIndexPosition)
        {
            return sentenceArray[clauserIndexPosition + 2].InnerText.Replace(" ", "") == ""
                            && sentenceArray[clauserIndexPosition + 3].InnerText.Replace(" ", "") == "BKP";
        }

        public void SplitArrayAtPosition<T>(T[] array, int index, out T[] first, out T[] second)
        {
            first = array.Take(index).ToArray();
            second = array.Skip(index).ToArray();
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


    public static class ArrayExtensions
    {
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
    }

}
