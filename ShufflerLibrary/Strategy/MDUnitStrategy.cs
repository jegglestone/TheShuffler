namespace ShufflerLibrary.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Helper;
    using Model;

    public class MDUnitStrategy : IStrategy
    {
        private MDSentenceDecorator _mdSentenceDecorator;

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsModifier))
                return sentence;

            _mdSentenceDecorator = 
                new MDSentenceDecorator(sentence);

            List<Text> modifiersUpToVbPastPresOrBkp; //VB/PAST/PRES/BKP

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
                }
            }
            else  // one modifier unit
            {
                modifiersUpToVbPastPresOrBkp = 
                  _mdSentenceDecorator.GetModifierUnitUpToVbPastPresBkp(firstModifierPosition);
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
            if(!textsbeforeMdUnit.Any(text => text.IsType(UnitTypes.VB_Verb)
                           || text.IsType(UnitTypes.PAST_Participle)
                           || text.IsType(UnitTypes.PRES_Participle)
                           || text.IsNN))
                return;

            var firstTextToImmediateLeftOfMdUnit = textsbeforeMdUnit.Last(
                text => text.IsType(UnitTypes.VB_Verb)
                           || text.IsType(UnitTypes.PAST_Participle)
                           || text.IsType(UnitTypes.PRES_Participle)
                           || text.IsNN);

            if (firstTextToImmediateLeftOfMdUnit.IsNN)
            {
                SearchForPrenAndMoveMDBeforeIt(
                    sentence, textsbeforeMdUnit, firstModifierPosition, modifiersUpToVbPastPresOrBkp);
            }
            else if (firstTextToImmediateLeftOfMdUnit.IsVbPastPres)
            {
                MoveMDBeforeVbPastPres(
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
                DeParticleHelper.CreateNewDeParticle(mdPositions.Last().EndPosition, 0));
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

        #region obsolete code
        //if (_mdSentenceDecorator.SentenceHasSingleModifierAndPyXuyaoUnit())
        //{
        //int PyXuyaoPosition =
        //    _mdSentenceDecorator.Texts.FindIndex(
        //        text => text.IsPyXuyao);

        //if (_mdSentenceDecorator.PyXuyaoIsWithinMDandPreceededByNN(
        //    PyXuyaoPosition))
        //{
        //    ApplyMDPlusPYXuyaoRules(
        //        PyXuyaoPosition);
        //}
        //}

        //private void ApplyMDPlusPYXuyaoRules(int PyXuyaoPosition)
        //{
        //    int nnPosition = _mdSentenceDecorator
        //        .TextsBeforePyXuyao(PyXuyaoPosition)
        //        .ToList()
        //        .FindLastIndex(text => text.IsNN);

        //    int modifierStartPosition = PyXuyaoPosition - 1;

        //    int modifierEndPosition =
        //        _mdSentenceDecorator.FirstBKPPositionAfterFirstModifier;

        //    var mdPyXuyaoUnit = _mdSentenceDecorator
        //        .Texts
        //        .Skip(modifierStartPosition)
        //        .ToList()
        //        .Take(modifierEndPosition - modifierStartPosition);

        //    _mdSentenceDecorator.Texts.RemoveRange(
        //        modifierStartPosition,
        //        modifierEndPosition - modifierStartPosition);

        //    _mdSentenceDecorator.Texts.InsertRange(
        //        nnPosition,
        //        mdPyXuyaoUnit);

        //    DeleteModifiers();
        //}

        //private void ApplyPrenDigAdjPlusNNRules(
        //    int firstModifierPosition, 
        //    MoveableUnit[] mdPositions, 
        //    int nnPosition)
        //{
        //    int lastPrenDigAdjPosition = nnPosition - 1;

        //    int firstModifierCurrentPosition =
        //        _mdSentenceDecorator.FirstModifierPosition;

        //    int modifierEndPosition =
        //        mdPositions.Last().EndPosition + 1;

        //    modifierEndPosition =
        //        RemovePrensFromUnitAndDecrementEndPosition(
        //            firstModifierCurrentPosition, modifierEndPosition);

        //    MoveModifierUnitAfter_PrenDigAdj_AndAddDeParticle(
        //        firstModifierPosition,
        //        modifierEndPosition,
        //        firstModifierCurrentPosition,
        //        lastPrenDigAdjPosition);

        //    DeleteModifiers();
        //}

        //private void MoveModifierUnitAfter_PrenDigAdj_AndAddDeParticle(
        //    int firstModifierPosition, 
        //    int modifierEndPosition,
        //    int firstModifierCurrentPosition, 
        //    int lastPrenDigAdjPosition)
        //{
        //    var modifierUnit =
        //        _mdSentenceDecorator
        //            .Texts
        //            .Skip(firstModifierPosition)
        //            .Take(modifierEndPosition).ToList();

        //    _mdSentenceDecorator.Texts.RemoveRange(
        //        firstModifierCurrentPosition,
        //        modifierEndPosition);

        //    modifierUnit.Add(
        //        DeParticleHelper.CreateNewDeParticle(
        //            modifierUnit.Last().pe_order, 0));

        //    _mdSentenceDecorator.Texts.InsertRange(
        //        lastPrenDigAdjPosition + 1,
        //        modifierUnit);
        //}

        //private int RemovePrensFromUnitAndDecrementEndPosition(
        //    int firstModifierCurrentPosition, int modifierEndPosition)
        //{
        //    for (int i = firstModifierCurrentPosition;
        //         i < firstModifierCurrentPosition + modifierEndPosition; i++)
        //    {
        //        if (_mdSentenceDecorator.Texts[i].IsPren)
        //        {
        //            _mdSentenceDecorator.Texts.RemoveAt(i);
        //            modifierEndPosition--;
        //        }
        //    }

        //    return modifierEndPosition;
        //}

        //private IEnumerable<Text> TextsBefore(int position)
        //{
        //    return _mdSentenceDecorator.Texts.Take(position);
        //}

        //private void DeleteModifiers()
        //{
        //    _mdSentenceDecorator.Texts.RemoveAll(text => text.IsModifier);
        //}

        //private void MoveModifiersBeforeVbVbaPast(
        //    MoveableUnit[] mdPositions, 
        //    int firstModifierPosition,
        //    List<Text> modifiersUpToVBorBK)
        //{
        //    ModifierPositionHelper.RemoveCurrentMDUnit(
        //        _mdSentenceDecorator,
        //        mdPositions,
        //        _mdSentenceDecorator.FirstModifierPosition);

        //    int VBVBAPASTPosition = _mdSentenceDecorator
        //        .Texts
        //        .Take(firstModifierPosition)
        //        .ToList()
        //        .FindIndex(text => text.IsVbVbaPast);

        //    ModifierPositionHelper.InsertReversedMDUnitBeforePosition(
        //        _mdSentenceDecorator,
        //        modifiersUpToVBorBK,
        //        VBVBAPASTPosition);
        //}
        #endregion

    }
}
