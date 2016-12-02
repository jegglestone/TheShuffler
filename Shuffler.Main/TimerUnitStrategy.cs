namespace Main
{
    using System;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;
    using Model;

    public class TimerUnitStrategy
    {
        public Paragraph ShuffleTimerUnits(Paragraph xmlSentenceElement)
        {
            Text[] sentenceArray = xmlSentenceElement.Descendants<Text>().ToArray();

            if (NoTimerFoundInSentence(sentenceArray))
                return xmlSentenceElement;

            // If an ADV is found, continue to search for the next ADV until reaching any of VB / PAST / PRES / Full - Stop.
            var totalNumberOfTimers = sentenceArray.Count(x => x.InnerText.IsTimer());

            var timerUnits = new TimerUnit[totalNumberOfTimers];
            int timerUnitCount = 0;

            for (int index = 0; index < sentenceArray.Length; index++)
            {
                var text = sentenceArray[index];
                if (text.IsTimer())
                {
                    timerUnits[timerUnitCount] = 
                        new TimerUnit {StartPosition = index};

                    if (timerUnitCount <= 0) continue;
                    timerUnits[timerUnitCount - 1].EndPosition = index - 1;

                    timerUnitCount++;
                }
                else if (text.ReachedSentenceBreaker())
                {
                    break;
                }
            }

            //Real GDP VBrose TM1this time TM2last year BKP.
            //Real GDP VBrose TM2last year TM1this time BKP.
            int firstTimerUnitPosition = timerUnits[0].StartPosition;
            int lastTimerUnitPosition = timerUnits[totalNumberOfTimers].EndPosition;
            UnderlineEntireTMUnit(sentenceArray, firstTimerUnitPosition, lastTimerUnitPosition);

            /*
             Split the Array at each timer.StartPosition
             * */
             // move to ReverseShuffle() method
            foreach (var timerUnit in timerUnits)
            {
                Text[] beforeTimerUnit;
                Text[] afterTimerUnit;
                ArrayUtility.SplitArrayAtPosition(
                    sentenceArray, timerUnit.StartPosition, out beforeTimerUnit, out afterTimerUnit);
            }

            // move shuffled timer unit before the VB/VBA/PAST/DG they modify

            return null;
        }

        private void UnderlineEntireTMUnit(Text[] sentenceArray, int firstTimerUnitPosition, int lastTimerUnitPosition)
        {
            for (int i = firstTimerUnitPosition; i < lastTimerUnitPosition; i ++)
            {
                OpenXmlTextHelper.UnderlineWordRun(
                    OpenXmlTextHelper.GetParentRunProperties(sentenceArray, i));
            }
        }


        private static bool NoTimerFoundInSentence(Text[] sentenceArray)
        {
            return !Array.Exists(
                sentenceArray, element => element.InnerText.IsTimer());
        }
    }
}
