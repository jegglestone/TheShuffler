namespace Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Model;

    public class TimerUnitStrategy
    {
        public Paragraph ShuffleTimerUnits(Paragraph xmlSentenceElement)
        {
            var sentence = new Sentence(xmlSentenceElement);
            Text[] sentenceArray = sentence.SentenceArray;

            if (NoTimerFoundInSentence(sentenceArray))
                return xmlSentenceElement;

            int timerUnitCount;
            var timerUnits = GetTimerUnits(
                sentenceArray, out timerUnitCount).ToArray<IMoveableUnit>();

            sentence.UnderlineJoinedSentenceUnit(
                sentenceArray, 
                timerUnits[0].StartPosition, 
                timerUnits[timerUnitCount - 1].EndPosition);

            var reversedTimerUnits = 
                sentence.GetMoveableUnitsInReverseOrder(
                    timerUnitCount, timerUnits, sentenceArray);


            // move shuffled timer unit before the VB/VBA/PAST/DG they modify

            // If a VB/ VBA / PAST is found, move the TM unit to before the VB/ VBA / PAST

            // If a DG is found, move the TM unit to before the digit unit

            RemoveAnyBlankSpaceFromEndOfUnit(reversedTimerUnits);

            return new Paragraph(
                OpenXmlHelper.BuildWordsIntoOpenXmlElement(
                    reversedTimerUnits
                    .Concat(
                        sentence.GetSentenceBreaker(sentenceArray)).ToArray()));
        }

        private static void RemoveAnyBlankSpaceFromEndOfUnit(Text[] reversedTimerUnits)
        {
            var lastText = reversedTimerUnits[reversedTimerUnits.Length - 1].Text;
            if (lastText == string.Empty)
                reversedTimerUnits[reversedTimerUnits.Length - 1].Remove();

            reversedTimerUnits[reversedTimerUnits.Length - 1].Text = lastText.TrimEnd(Convert.ToChar(" "));
        }

        private static IEnumerable<TimerUnit> GetTimerUnits(IList<Text> sentenceArray, out int timerUnitCount)
        {
            TimerUnit[] timerUnits =
                new TimerUnit[sentenceArray.Count(x => x.InnerText.IsTimer())];

            timerUnitCount = 0;

            for (int index = 0; index < sentenceArray.Count; index++)
            {
                if (sentenceArray[index].IsTimer())
                {
                    timerUnits[timerUnitCount] = new TimerUnit
                    {
                        StartPosition = index
                    };

                    timerUnitCount++;

                    if (timerUnitCount <= 1) continue;

                    timerUnits[timerUnitCount - 2].EndPosition = index;
                }
                else if (sentenceArray[index].InnerText.IsBreakerPunctuation())
                {
                    timerUnits[timerUnitCount - 1].EndPosition = index;
                    break;
                }
            }
            return timerUnits;
        }

        private static bool NoTimerFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsTimer());
        }
    }
}
