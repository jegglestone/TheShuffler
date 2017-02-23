namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class TimerUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasTimer())
                return sentence;

            var timerSentenceDecorator =
                new TimerSentenceDecorator(sentence);

            List<Text> reversedTimerUnit = 
                GetTimerUnitsInReverse(timerSentenceDecorator);

            ShuffleTimerUnit(
                timerSentenceDecorator, reversedTimerUnit, timerSentenceDecorator.FirstTimerPosition);
            
            UnderlineTimerUnit(
                timerSentenceDecorator, timerSentenceDecorator.FirstTimerPosition, reversedTimerUnit.Count);
            
            return sentence;
        }

        private static void ShuffleTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator, 
            List<Text> reversedTexts, 
            int originalTimerIndexPosition)
        {
            if (TimerPreceededByDigButNotImmediately(
                timerSentenceDecorator, originalTimerIndexPosition))   // 2.  
            {
                // 2.1.If DIG is found first, move the TM unit to before the DIG unit
                MoveTimerBeforeDig(
                    timerSentenceDecorator, reversedTexts);

                return;
            }
            // 2.2 not found 

            // 3.Search to the left of TM for any of ADV or ‘ADV and ADV’ or ‘ADV, ADV and ADV.
            if (timerSentenceDecorator.Texts.Take(originalTimerIndexPosition).Any(text => text.IsAdverb))
            {
                // 3.1.If any of ADV or ‘ADV and ADV’ or ‘ADV, ADV and ADV is found move TM before first ADV:
                MoveTimerBeforeAdverb(
                    timerSentenceDecorator, reversedTexts, originalTimerIndexPosition);

            }
            //3.2.If no ADV or or ‘ADV and ADV’ or ‘ADV, ADV and ADV is found –
            else if (PastPresBeforeTimerUnit(timerSentenceDecorator))
            {  
               
               // 4.Search to the left of TM for PAST / PRES
               //  4.1.If either is found, move the TM unit to before PAST or PRES whichever is the case:
                int lastPastPresPosition =
                                timerSentenceDecorator.Texts.Take(originalTimerIndexPosition).ToList()
                                                .FindLastIndex(text => text.IsPast || text.IsPres);

                MoveTimerUnitToPosition(
                    lastPastPresPosition, reversedTexts, timerSentenceDecorator);
            }
            // 4.2.If neither PAST nor PRES is found -
            else if (NoNnUnitBeforeTimerUnit(timerSentenceDecorator))
            {
                //   5.Search to the left of TM for NNX until reaching BKP
                //   5.1.If no NNX is found, end the task.TM stays where it is

                KeepTimerUnitInSamePosition(
                    timerSentenceDecorator, reversedTexts, originalTimerIndexPosition);
            }
            else
            {
                // 5.2. NNX is found -
                ApplyBeforeNnShuffleRules(
                    timerSentenceDecorator, reversedTexts, originalTimerIndexPosition);
            } 
        }

        private static void ApplyBeforeNnShuffleRules(TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts,
            int originalTimerIndexPosition)
        {
            //   5.3.Search its left for PREN or ADJ until reaching ‘and’ or BK / NBKP / BKP.
            int lastNnPosition =
                GetClosestNnPosition(timerSentenceDecorator, originalTimerIndexPosition);
            
            int andOrBreakerPosition = GetLastAndOrBkpPositionBeforeNn(
                timerSentenceDecorator, lastNnPosition);
            if (andOrBreakerPosition == -1) andOrBreakerPosition = 0;

            //   5.3.1.If PREN is found, move the TM unit to before PREN:
            if (timerSentenceDecorator
                            .Texts
                            .Skip(andOrBreakerPosition + 1)
                            .Take(lastNnPosition - andOrBreakerPosition)
                            .Any(text => text.IsPren))
            {
                MoveTimerUnitBeforePren(
                    timerSentenceDecorator, reversedTexts, andOrBreakerPosition, lastNnPosition);
            }
            //   5.3.2.If no PREN is found but ADJ is found, move the TM unit to before ADJ:
            else if (timerSentenceDecorator
                            .Texts
                            .Skip(andOrBreakerPosition + 1)
                            .Take(lastNnPosition - andOrBreakerPosition)
                            .Any(text => text.IsNumberedType(UnitTypes.ADJ_Adjective)
                                         || text.IsType(UnitTypes.ADJ_Adjective)))
            {
                MoveTimerUnitBeforeAdj(
                    timerSentenceDecorator, reversedTexts, andOrBreakerPosition, lastNnPosition);
            }

            else
            {
                //   5.3.3.If neither PREN nor ADJ is found, move the TM unit to before NN
                MoveTimerUnitToPosition(
                    lastNnPosition,
                    reversedTexts,
                    timerSentenceDecorator);
            }

            //              Bef: …VBup from ADJabout 150, 000 jobs TMper month… 
            //Aft: … VBup from TMper month ADJabout 150, 000 jobs … 
        }

        private static bool NoNnUnitBeforeTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            int closestAndBkBkpPosition
                = GetClosestAndBkBkpPositionToTimer(
                    timerSentenceDecorator, timerSentenceDecorator.FirstTimerPosition);

            return !timerSentenceDecorator
                    .Texts
                    .Skip(closestAndBkBkpPosition)
                    .Take(timerSentenceDecorator.FirstTimerPosition - closestAndBkBkpPosition)
                    .Any(text => text.IsNN);
        }

        private static bool PastPresBeforeTimerUnit(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            int closestAndBkBkpPosition
                = GetClosestAndBkBkpPositionToTimer(
                    timerSentenceDecorator, timerSentenceDecorator.FirstTimerPosition);

            return timerSentenceDecorator
                    .Texts
                    .Skip(closestAndBkBkpPosition)
                    .Take(timerSentenceDecorator.FirstTimerPosition - closestAndBkBkpPosition)
                    .Any(text => text.IsPast || text.IsPres);
        }

        private static int GetClosestNnPosition(
            TimerSentenceDecorator timerSentenceDecorator, 
            int originalTimerIndexPosition)
        {
            // until reaching ‘and’ or BK / NBKP / BKP
            int closestAndBkBkpPosition
                = GetClosestAndBkBkpPositionToTimer(
                    timerSentenceDecorator, originalTimerIndexPosition);

            int nnPosition = timerSentenceDecorator
                            .Texts
                            .Skip(closestAndBkBkpPosition)
                            .Take(originalTimerIndexPosition - closestAndBkBkpPosition)
                            .ToList()
                            .FindLastIndex(text => text.IsNN)
                            + closestAndBkBkpPosition;

            return nnPosition;
        }

        private static int GetClosestAndBkBkpPositionToTimer(TimerSentenceDecorator timerSentenceDecorator, int originalTimerIndexPosition)
        {
            return timerSentenceDecorator
                                   .Texts
                                   .Take(originalTimerIndexPosition)
                                   .ToList()
                                   .FindLastIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation)
                                                          || text.IsType(UnitTypes.NbkpNonBreakerPunctuation)
                                                          || text.IsType(UnitTypes.BK_Breaker))
                   + 1;
        }

        private static void MoveTimerUnitBeforeAdj(TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts,
            int andOrBreakerPosition, int lastNnPosition)
        {
            int adjPos = timerSentenceDecorator.Texts
                                         .Skip(andOrBreakerPosition)
                                         .Take(lastNnPosition - andOrBreakerPosition).ToList()
                                         .FindLastIndex(text => text.IsNumberedType(UnitTypes.ADJ_Adjective)
                                                                || text.IsType(UnitTypes.ADJ_Adjective))
                         + andOrBreakerPosition;

            MoveTimerUnitToPosition(
                adjPos,
                reversedTexts,
                timerSentenceDecorator);
        }

        private static void MoveTimerUnitBeforePren(TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts,
            int andOrBreakerPosition, int lastNnPosition)
        {
            int prenPos = timerSentenceDecorator.Texts
                                          .Skip(andOrBreakerPosition)
                                          .Take(lastNnPosition - andOrBreakerPosition).ToList()
                                          .FindLastIndex(text => text.IsPren)
                          + andOrBreakerPosition;

            MoveTimerUnitToPosition(
                prenPos,
                reversedTexts,
                timerSentenceDecorator);
        }

        private static int GetLastAndOrBkpPositionBeforeNn(TimerSentenceDecorator timerSentenceDecorator, int lastNnPosition)
        {
            return timerSentenceDecorator.Texts
                            .Take(lastNnPosition).ToList()
                            .FindLastIndex(
                                text => text.actual_text_used.Replace(" ", "").ToLower() == "and"
                                        || text.IsType(UnitTypes.BKP_BreakerPunctuation)
                                        || text.IsType(UnitTypes.BK_Breaker));
        }


        private static void MoveTimerBeforeDig(TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts)
        {
            // that is not to the immediate left of TM, whichever is found first, 
            // until reaching NBKP or the beginning of a sentence 
            int breakerPosition =
                            timerSentenceDecorator
                                            .Texts
                                            .Take(timerSentenceDecorator.FirstTimerPosition)
                                            .ToList()
                                            .FindLastIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation));
            if (breakerPosition == -1) breakerPosition = 0;

            int digPosition = timerSentenceDecorator
                            .Texts
                            .Skip(breakerPosition)
                            .Take(timerSentenceDecorator.FirstTimerPosition - breakerPosition)
                            .ToList()
                            .FindLastIndex(text => text.IsType(UnitTypes.DIG_Digit))
                            + breakerPosition;

            MoveTimerUnitToPosition(
                digPosition,
                reversedTexts,
                timerSentenceDecorator);
        }

        private static void MoveTimerBeforeAdverb(TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts,
            int positionToSearchBefore)
        {
            int adverbPosition = timerSentenceDecorator.Texts.Take(positionToSearchBefore)
                            .ToList()
                            .FindLastIndex(text => text.IsAdverb);

            MoveTimerUnitToPosition(
                adverbPosition,
                reversedTexts,
                timerSentenceDecorator);
        }

        private static bool TimerPreceededByDigButNotImmediately(
            TimerSentenceDecorator timerSentenceDecorator, int originalTimerIndexPosition)
        {
            int firstBreakerPosition =
                timerSentenceDecorator.Texts.Take(originalTimerIndexPosition - 1)
                    .ToList().FindLastIndex(text => text.IsComma) + 1;

            var textsAfterBreakerBeforeTimer = timerSentenceDecorator
                            .Texts
                            .Skip(firstBreakerPosition)
                            .Take((originalTimerIndexPosition - 1) - firstBreakerPosition);

            return 
                textsAfterBreakerBeforeTimer
                .Any(
                    text => text.IsType(UnitTypes.DIG_Digit));
        }

        private static void KeepTimerUnitInSamePosition(
            TimerSentenceDecorator timerSentenceDecorator, List<Text> reversedTexts,
            int originalTimerIndexPosition)
        {
            MoveTimerUnitToPosition(
                originalTimerIndexPosition,
                reversedTexts,
                timerSentenceDecorator);
        }

        private static List<Text> GetTimerUnitsInReverse(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            MoveableUnit[] timerPositions =
                GetTimerUnitPositions(timerSentenceDecorator);

            timerPositions[timerPositions.Length - 1].EndPosition =
                timerSentenceDecorator
                    .Texts
                    .Skip(timerSentenceDecorator.FirstTimerPosition)
                    .ToList()
                    .FindIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation))
                    + timerSentenceDecorator.FirstTimerPosition - 1;

                Array.Reverse(timerPositions);

            List<Text> reversedTimerUnit =
                MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                    timerSentenceDecorator.Texts, timerPositions);

            return reversedTimerUnit;
        }

        public static void MoveTimerUnitToPosition(
            int newPosition, 
            List<Text> reversedTexts, 
            TimerSentenceDecorator timerSentenceDecorator)
        {
            RemoveCurrentTimerUnit(
                timerSentenceDecorator, reversedTexts.Count);

            if (newPosition > timerSentenceDecorator.TextCount - 1)
                timerSentenceDecorator.Texts.AddRange(reversedTexts);
            else
                timerSentenceDecorator.Texts.InsertRange(
                    newPosition,
                    reversedTexts);
        }

        private static void RemoveCurrentTimerUnit(
            SentenceDecorator timerSentenceDecorator, int timerUnitSize)
        {
            int firstTimerIndexPosition = timerSentenceDecorator.FirstTimerPosition;
            timerSentenceDecorator.Texts.RemoveRange(
                firstTimerIndexPosition,
                timerUnitSize);
        }

        private static void UnderlineTimerUnit(
            SentenceDecorator timerSentenceDecorator, int newFirstTimerIndexPosition, int timerUnitCount)
        {
            timerSentenceDecorator.Texts[newFirstTimerIndexPosition].pe_merge_ahead
                = timerUnitCount - 1;
        }
        
        private static MoveableUnit[] GetTimerUnitPositions(
            TimerSentenceDecorator timerSentenceDecorator)
        {
            return MoveableUnitHelper.GetMoveableUnitPositions(
                timerSentenceDecorator.Texts, 
                MoveableUnitHelper.NumberableUnitType.Timer, 
                timerSentenceDecorator.TimerUnitCount);
        }
    }
}
