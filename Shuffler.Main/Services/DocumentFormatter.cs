namespace Main.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;
    using Interfaces;

    public class DocumentFormatter
    {
        private readonly IShuffleStrategy _clauserUnitStrategy;
        private readonly IShuffleStrategy _adverbUnitStrategy;
        private readonly IShuffleStrategy _timerUnitStrategy;
        private readonly IShuffleStrategy _modifierStrategy;
        private readonly IShuffleStrategy _prenStrategy;

        public DocumentFormatter(
            IShuffleStrategy clauserUnitStrategy
            , IShuffleStrategy adverbUnitStrategy
            , IShuffleStrategy timerUnitStrategy
            , IShuffleStrategy modifierStrategy
            , IShuffleStrategy prenStrategy)
        {
            _clauserUnitStrategy = clauserUnitStrategy;
            _adverbUnitStrategy = adverbUnitStrategy;
            _timerUnitStrategy = timerUnitStrategy;
            _modifierStrategy = modifierStrategy;
            _prenStrategy = prenStrategy;
        }

        public List<OpenXmlElement> ProcessDocument(MainDocumentPart docPart)
        {
            var shuffledElements = new List<OpenXmlElement>();

            foreach (var element in docPart.Document.Body.Elements().ToList())
            {
                if (IsParagraph(element))
                {
                    ShuffleParagraph(element, shuffledElements);
                }
                else
                {
                    shuffledElements.Add(element);
                }
            }

            return shuffledElements;
        }

        private void ShuffleParagraph(OpenXmlElement element, List<OpenXmlElement> shuffledElements)
        {
            if (ParagraphHasMultipleSentences(element))
            {
                Paragraph ShuffledSentences = ShuffleSentences(element);
                shuffledElements.Add(ShuffledSentences);
            }
            else
            {
                shuffledElements.Add(ShuffleParagraph(element));
            }
        }

        private Paragraph ShuffleSentences(OpenXmlElement element)
        {
            var strArray = element.Descendants<Text>().ToArray();
            int prevBKP = 0;
            int prevFullStopPosition = 0;
            
            var ShuffledSentences = new List<OpenXmlElement>();
            for (int i = 0; i < strArray.Length; i++)
            {
                var currentWord = strArray[i].InnerText;
                if (currentWord.IsBreakerPunctuation())
                {
                    prevBKP = i;
                }
                else if (IsFullStop(prevBKP, i, currentWord))
                {
                    var beforeFullStop = ExtractSentenceFromParagraph(strArray, i, prevFullStopPosition);

                    prevFullStopPosition = i;
                    OpenXmlElement sentenceElement = new Paragraph();

                    List<Run> words = beforeFullStop.Select(word => word.Parent.CloneNode(true) as Run).ToList();

                    sentenceElement.Append(words);

                    var shuffledSentence = ShuffleParagraph(sentenceElement);

                    ShuffledSentences.Add(shuffledSentence);
                }
            }

            var paragraph = new Paragraph();
            foreach (var sentence in ShuffledSentences)
            {
                foreach (var openXmlWordRun in sentence)
                {
                    paragraph.AppendChild(
                        openXmlWordRun.CloneNode(true));
                }
            }
            
            return paragraph;
        }

        private static bool IsFullStop(int prevBKP, int i, string currentWord)
        {
            return currentWord == "." && prevBKP == (i - 1);
        }

        private static Text[] ExtractSentenceFromParagraph(Text[] strArray, int i, int prevFullStopPosition)
        {
            Text[] beforeFullStop;
            Text[] afterFullStop;

            ArrayUtility.SplitArrayAtPosition(
                strArray, i + 1, out beforeFullStop, out afterFullStop);

            if (prevFullStopPosition > 0 && prevFullStopPosition < i)
            {
                ArrayUtility.SplitArrayAtPosition(beforeFullStop, prevFullStopPosition + 1, out beforeFullStop,
                    out afterFullStop);
                beforeFullStop = afterFullStop;
            }
            return beforeFullStop;
        }

        private Paragraph ShuffleParagraph(OpenXmlElement element)
        {
            var shuffledElement = element as Paragraph;
            shuffledElement = _clauserUnitStrategy.ShuffleSentenceUnit(shuffledElement);
            shuffledElement = _adverbUnitStrategy.ShuffleSentenceUnit(shuffledElement);
            shuffledElement = _timerUnitStrategy.ShuffleSentenceUnit(shuffledElement);
            shuffledElement = _modifierStrategy.ShuffleSentenceUnit(shuffledElement);
            shuffledElement = _prenStrategy.ShuffleSentenceUnit(shuffledElement);
            return shuffledElement;
        }

        private static bool ParagraphHasMultipleSentences(OpenXmlElement element)
        {
            return element.InnerText.CountTimesThisStringAppearsInThatString("BKP.") > 1;
        }

        private static bool IsParagraph(OpenXmlElement element)
        {
            return element.LocalName == "p";
        }
    }
}
