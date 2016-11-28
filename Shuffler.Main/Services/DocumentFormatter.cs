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
        private readonly IClauserUnitStrategy _clauserUnitStrategy;
        private readonly IAdverbStrategy _adverbStrategy;

        public DocumentFormatter(
            IClauserUnitStrategy clauserUnitStrategy
            , AdverbStrategy adverbStrategy)
        {
            _clauserUnitStrategy = clauserUnitStrategy;
            _adverbStrategy = adverbStrategy;
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
                IEnumerable<OpenXmlElement> ShuffledSentences = ShuffleSentences(element);
                shuffledElements.AddRange(new Paragraph(ShuffledSentences));
            }
            else
            {
                Paragraph shuffledElement = ShuffleParagraph(element);
                shuffledElements.Add(shuffledElement);
            }
        }

        private IEnumerable<OpenXmlElement> ShuffleSentences(OpenXmlElement element)
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

                    sentenceElement.Append(words.ToList());

                    Paragraph shuffledSentence = ShuffleParagraph(sentenceElement);
                    ShuffledSentences.Add(shuffledSentence);
                }
            }

            return ShuffledSentences;
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
            shuffledElement = _clauserUnitStrategy.ShuffleClauserUnits(shuffledElement);
            shuffledElement = _adverbStrategy.ShuffleAdverbUnits(shuffledElement);
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
