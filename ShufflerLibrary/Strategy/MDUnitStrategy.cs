namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class MdUnitStrategy : IStrategy
    {
        private MdSentenceDecorator _mdSentenceDecorator;

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasModifier())
                return sentence;

            _mdSentenceDecorator = 
                new MdSentenceDecorator(sentence);

            List<Text> modifiersUpToVbPastPresOrBkp;

            int firstModifierPosition = _mdSentenceDecorator.FirstModifierPosition;

            if (_mdSentenceDecorator.HasMoreThanOneModifier())
            {
                modifiersUpToVbPastPresOrBkp =
                    _mdSentenceDecorator.GetModifierUnitUpToVbPastPresBkp(firstModifierPosition);

                if (modifiersUpToVbPastPresOrBkp.Count > 1)
                {
                    var mdPositions = ModifierPositionHelper.GetMDUnitPositions(
                        modifiersUpToVbPastPresOrBkp);

                    ReverseModifierUnit(modifiersUpToVbPastPresOrBkp, firstModifierPosition, mdPositions);

                    AddDeParticleToMDUnit(sentence, mdPositions);

                    UnderlineMdUnit(
                        firstModifierPosition, 
                        _mdSentenceDecorator.Texts.Skip(firstModifierPosition).ToList().FindIndex(text => text.IsDe()));
                }
            }
            else  // one modifier unit
            {
                modifiersUpToVbPastPresOrBkp = 
                  _mdSentenceDecorator.GetModifierUnitUpToVbPastPresBkp(firstModifierPosition);

                UnderlineMdUnit(firstModifierPosition,
                    modifiersUpToVbPastPresOrBkp.Count-1);
            }

            var textsbeforeMdUnit = 
                TextsBefore(firstModifierPosition).ToList();

            if (textsbeforeMdUnit.Any())
            {
                ApplySearchLeftRules(
                  sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            }

            return sentence;
        }

        private void UnderlineMdUnit(int firstModifierPosition, int endPosition)
        {
            _mdSentenceDecorator
                .Texts[firstModifierPosition].pe_merge_ahead = endPosition;
        }

        private void ReverseModifierUnit(List<Text> modifiersUpToVbPastPresOrBkp, int firstModifierPosition,
        MoveableUnit[] mdPositions)
      {
        if (_mdSentenceDecorator.ReversableUnitsAreSortedAscending(
          modifiersUpToVbPastPresOrBkp, text => text.IsModifier))
        {
          SortModifiersInDescendingNumericOrder(
            modifiersUpToVbPastPresOrBkp, firstModifierPosition, mdPositions);
        }
      }

      private static void ApplySearchLeftRules(Sentence sentence, List<Text> textsbeforeMdUnit, int firstModifierPosition,
            List<Text> modifiersUpToVbPastPresOrBkp)
        {  
            var textToImmediateLeftOfMd = textsbeforeMdUnit.Last();

            //if (!textToImmediateLeftOfMd.IsType(UnitTypes.VB_Verb)  //2.
            //               && !textToImmediateLeftOfMd.IsType(UnitTypes.PAST_Participle)
            //               && !textToImmediateLeftOfMd.IsType(UnitTypes.PRES_Participle)
            //               && !textToImmediateLeftOfMd.IsNN
            //               && !textToImmediateLeftOfMd.IsBKBy
            //               && !textToImmediateLeftOfMd.IsBkp
            //               && !textToImmediateLeftOfMd.IsType(UnitTypes.NbkpNonBreakerPunctuation)))
            //    return;

            //var firstTextToImmediateLeftOfMdUnit = textsbeforeMdUnit.Last(
            //    text => text.IsType(UnitTypes.VB_Verb)
            //               || text.IsType(UnitTypes.PAST_Participle)
            //               || text.IsType(UnitTypes.PRES_Participle)
            //               || text.IsNN
            //               || text.IsBKBy
            //               || text.IsBkp
            //               || text.IsType(UnitTypes.NbkpNonBreakerPunctuation));

            if (textToImmediateLeftOfMd.IsNN)      // 2.1
                SearchForPrenAndMoveMDBeforeIt(             // 2.1.1 - 2.1.2
                    sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            if (textToImmediateLeftOfMd.IsBkp)     // 2.2
                return;
            if (textToImmediateLeftOfMd.IsBKBy)     // 2.3
                return;
            if (textToImmediateLeftOfMd.IsVbPastPres)
            {
                MoveMDBeforeVbPastPres(                     // 2.4
                    sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            }
        }

        private static void SearchForPrenAndMoveMDBeforeIt(Sentence sentence, List<Text> textsbeforeMdUnit, int firstModifierPosition,
            List<Text> modifiersUpToVbPastPresOrBkp)
        {
            bool hasVbPastPresOrBkpBeforeMd = textsbeforeMdUnit.Any(
                PredicateTextIsVbPastPresBkp()); 

            if (hasVbPastPresOrBkpBeforeMd)
                textsbeforeMdUnit =
                    textsbeforeMdUnit.Skip(
                        textsbeforeMdUnit.FindLastIndex(
                            TextIsVbPastPresBkp())).ToList();

            if (textsbeforeMdUnit.Any(text => text.IsPren))
            {
                MoveMDBeforePren(
                    sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            }
            else if (textsbeforeMdUnit.Any(text => text.IsType(UnitTypes.ADJ_Adjective)))
            {
                MoveMDBeforeADJ(
                    sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            }
            else if (textsbeforeMdUnit.Any(text => text.IsNN))
            {
                MoveMDBeforeNN(
                    sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            }
        }

        private static void MoveMDBeforePren(
            Sentence sentence, 
            List<Text> textsbeforeMdUnit, 
            int firstModifierPosition,
            List<Text> modifiersUpToVbPastPresOrBkp)
        {
            var prenUnit =
                textsbeforeMdUnit.Last(text => text.IsPren);

            int prenPositionInSentence =
                sentence.Texts.FindIndex(text => text.pe_order == prenUnit.pe_order);

            int mdUnitSize = GetModifierUnitSize(modifiersUpToVbPastPresOrBkp);

            var mdUnitPlusDe = sentence.Texts.GetRange(
                firstModifierPosition, mdUnitSize);

            sentence.Texts.RemoveRange(
                firstModifierPosition, mdUnitSize);

            sentence.Texts.InsertRange(prenPositionInSentence, mdUnitPlusDe);
        }

        private static void MoveMDBeforeADJ(
            Sentence sentence,
            List<Text> textsbeforeMdUnit,
            int firstModifierPosition,
            List<Text> modifiersUpToVbPastPresOrBkp)
        {
            var adjUnit =
                textsbeforeMdUnit.Last(text => text.IsType(UnitTypes.ADJ_Adjective));

            int adjnPositionInSentence =
                sentence.Texts.FindIndex(text => text.pe_order == adjUnit.pe_order);

            int mdUnitSize = GetModifierUnitSize(modifiersUpToVbPastPresOrBkp);

            var mdUnitPlusDe = sentence.Texts.GetRange(
                firstModifierPosition, mdUnitSize);

            sentence.Texts.RemoveRange(
                firstModifierPosition, mdUnitSize);

            sentence.Texts.InsertRange(adjnPositionInSentence, mdUnitPlusDe);
        }

        private static void MoveMDBeforeNN(
            Sentence sentence,
            List<Text> textsbeforeMdUnit,
            int firstModifierPosition,
            List<Text> modifiersUpToVbPastPresOrBkp)
        {
            var nnUnit =
                textsbeforeMdUnit.Last(text => text.IsNN);

            int nnPositionInSentence =
                sentence.Texts.FindIndex(text => text.pe_order == nnUnit.pe_order);

            int mdUnitSize = GetModifierUnitSize(modifiersUpToVbPastPresOrBkp);

            var mdUnitPlusDe = sentence.Texts.GetRange(
                firstModifierPosition, mdUnitSize);

            sentence.Texts.RemoveRange(
                firstModifierPosition, mdUnitSize); 

            sentence.Texts.InsertRange(nnPositionInSentence, mdUnitPlusDe);
        }
        
       private static void MoveMDBeforeVbPastPres(
            Sentence sentence,
            List<Text> textsbeforeMdUnit,
            int firstModifierPosition,
            List<Text> modifiersUpToVbPastPresOrBkp)
        {
            var vbPastPresUnit =
                textsbeforeMdUnit.Last(text => text.IsVbPastPres);

            int vbPastPresPositionInSentence =
                sentence.Texts.FindIndex(text => text.pe_order == vbPastPresUnit.pe_order);

            int mdUnitSize = GetModifierUnitSize(modifiersUpToVbPastPresOrBkp);

            var mdUnitPlusDe = sentence.Texts.GetRange(
                firstModifierPosition, mdUnitSize);

            sentence.Texts.RemoveRange(
                firstModifierPosition, mdUnitSize);
         
            sentence.Texts.InsertRange(vbPastPresPositionInSentence, mdUnitPlusDe);
        }

        private static int GetModifierUnitSize(List<Text> modifiersUpToVbPastPresOrBkp)
        {
            int mdUnitSize = modifiersUpToVbPastPresOrBkp.Count;
            if (modifiersUpToVbPastPresOrBkp.Count(text => text.IsModifier) > 1)
                mdUnitSize++; // there will be a de unit after it
            return mdUnitSize;
        }

        private static Predicate<Text> TextIsVbPastPresBkp()
        {
            return text => text.IsType(UnitTypes.VB_Verb) 
                           || text.IsType(UnitTypes.BKP_BreakerPunctuation)
                           || text.IsType(UnitTypes.PAST_Participle)
                           || text.IsType(UnitTypes.PRES_Participle);
        }

        private static Func<Text, bool> PredicateTextIsVbPastPresBkp()
        {
            return text => text.IsType(UnitTypes.VB_Verb)
                           ||text.IsType(UnitTypes.BKP_BreakerPunctuation)
                           ||text.IsType(UnitTypes.PAST_Participle)
                           ||text.IsType(UnitTypes.PRES_Participle);
        }

        private void AddDeParticleToMDUnit(Sentence sentence, MoveableUnit[] mdPositions)
        {
            sentence.Texts.Insert(
                _mdSentenceDecorator.FirstVbPastPresBkpPositionAfterFirstModifier,
                DeParticleHelper.CreateNewDeParticle(
                  sentence.Texts[mdPositions.Last().EndPosition].pe_order, 
                  0));
        }

        private void SortModifiersInDescendingNumericOrder(
            List<Text> modifiers,
            int firstModifierPosition,
            MoveableUnit[] mdPositions)
        {
            ModifierPositionHelper.RemoveCurrentMDUnit(
                _mdSentenceDecorator,
                mdPositions,
                firstModifierPosition);

            Array.Reverse(mdPositions);

            List<Text> reversedMdUnit =
                MoveableUnitHelper.GetTextsFromMoveablePositionsList(
                    modifiers, mdPositions);

            ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
                _mdSentenceDecorator,
                reversedMdUnit,
                firstModifierPosition);
        }

        private IEnumerable<Text> TextsBefore(int position)
        {
            return _mdSentenceDecorator.Texts.Take(position);
        }
    }
}
