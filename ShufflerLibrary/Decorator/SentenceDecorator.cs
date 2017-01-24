namespace ShufflerLibrary.Decorator
{
    using Model;
    using System.Collections.Generic;
    using System.Linq;

    public class SentenceDecorator
    {
        protected Sentence Sentence { get; set; }

        public List<Text> Texts
        {
            get { return Sentence.Texts; }
            set { Sentence.Texts = value; }
        }

        public Text SentenceBreaker => Sentence.SentenceBreaker;

        public int Pe_para_no => Sentence.pe_para_no;

        public int TextCount => Sentence.TextCount;

        public bool HasVBVBAPAST => Sentence.HasVBVBAPAST;

        public bool HasDG => Sentence.HasDG;

        //TODO - these need moving into a ModifierSentenceDecorator
        // or use composition here
        public int ModifierCount
        {
            get { return Texts.Count(text => text.IsModifier); }
        }

        public int FirstModifierPosition
        {
            get { return Texts.FindIndex(text => text.IsModifier); }
        }

        public bool HasMoreThanOneModifier()
        {
            return ModifierCount > 1;
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

        public List<Text> GetModifierUnitUpToVBorBK(
            int firstModifierPosition)
        {
            List<Text> modifiersUpToVBorBK = new List<Text>();

            for (int i = firstModifierPosition;
                i < FirstVBorBKPPositionAfterFirstModifier; i++)
            {
                modifiersUpToVBorBK.Add(Texts[i]);
            }

            return modifiersUpToVBorBK;
        }

        public bool ReversableUnitsAreSortedAscending(
            IReadOnlyCollection<Text> timerUnit,
            System.Func<Text, bool> p)
        {
            var orderedByAsc =
                timerUnit
                    .Where(p)
                    .OrderBy(d => d.actual_tag_used);

            if (timerUnit
                .Where(p)
                .SequenceEqual(orderedByAsc))
            {
                return true;
            }
            return false;
        }
    }
}
