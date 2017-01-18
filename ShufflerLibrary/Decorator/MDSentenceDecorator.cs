namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class MDSentenceDecorator : SentenceDecorator
    {
        public MDSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }

        public int ModifierCount
        {
            get { return Texts.Count(text => text.IsModifier); }
        }

        public int FirstModifierPosition
        {
            get { return Texts.FindIndex(text => text.IsModifier); }
        }

        public int FirstVBorBKPPositionAfterFirstModifier
        {
            get
            {
                return Texts
                    .Skip(FirstModifierPosition)
                    .ToList()
                    .FindIndex(text => text.IsType(UnitTypes.VB_Verb)
                                       || text.IsType(UnitTypes.BKP_BreakerPunctuation)
                                       || text.IsType(UnitTypes.NBKP_NonBreakerPunctuation))
                       + FirstModifierPosition;
            }
        }

        public int FirstBKPPositionAfterFirstModifier
        {
            get
            {
                return Texts
                    .Skip(FirstModifierPosition)
                    .ToList()
                    .FindIndex(text => text.IsType(UnitTypes.BKP_BreakerPunctuation))
                       + FirstModifierPosition;
            }
        }

        public bool ModifiersAreSortedAscending(
            IReadOnlyCollection<Text> modifiersUpToVBorBk)
        {
            var orderedByAsc =
                modifiersUpToVBorBk
                    .Where(text => text.IsModifier)
                    .OrderBy(d => d.actual_tag_used);

            if (modifiersUpToVBorBk
                .Where(text => text.IsModifier)
                .SequenceEqual(orderedByAsc))
            {
                return true;
            }
            return false;
        }

        public bool HasMoreThanOneModifier()
        {
            return ModifierCount > 1;
        }

        public IEnumerable<Text> TextsBeforePyXuyao(int PyXuyaoPosition)
        {
            return Texts.Take(PyXuyaoPosition);
        }

        public bool SentenceHasSingleModifierAndPyXuyaoUnit()
        {
            return Texts.Any(text => text.IsPyXuyao)
                   && Texts.Count(text => text.IsModifier) == 1;
        }

        public bool PyXuyaoIsWithinMDandPreceededByNN(int PyXuyaoPosition)
        {
            return TextsBeforePyXuyao(PyXuyaoPosition).Any(text => text.IsNN)
                   && Texts[PyXuyaoPosition - 1].IsModifier;

        }

        public bool ModifierUnitHasTimerUnit(List<Text> modifiersUpToVBorBK)
        {
            return modifiersUpToVBorBK.Any(text => text.IsTimer
                                                   || text.IsType(UnitTypes.TMY_TimerYear));
        }

        public bool PrenAdjOrDigBeforeNN(int nnPosition)
        {
            return Texts[nnPosition - 1].IsType(UnitTypes.ADJ_Adjective)
                   || Texts[nnPosition - 1].IsType(UnitTypes.DIG_Digit)
                   || Texts[nnPosition - 1].IsType(UnitTypes.PREN_Pronoun);
        }

        public List<Text> GetModifierUnitUpToVBorBK(int firstModifierPosition)
        {
            List<Text> modifiersUpToVBorBK = new List<Text>();

            for (int i = firstModifierPosition;
                i < FirstVBorBKPPositionAfterFirstModifier; i++)
            {
                modifiersUpToVBorBK.Add(Texts[i]);
            }

            return modifiersUpToVBorBK;
        }
    }
}
