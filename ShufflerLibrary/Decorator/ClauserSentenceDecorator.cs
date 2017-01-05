namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class ClauserSentenceDecorator
    {
        public Sentence Sentence { get; }

        public ClauserSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }

        public int ClauserIndexPosition
        {
            get
            {
                if (!Sentence.HasClauser()) return -1;

                if (Sentence.Texts.FindIndex(
                    text => text.pe_tag_revised == UnitTypes.CS_ClauserUnit) > -1)
                {
                    return Sentence.Texts.FindIndex(
                        text => text.pe_tag_revised == UnitTypes.CS_ClauserUnit);
                }

                return Sentence.Texts.FindIndex(
                    text => text.pe_tag == UnitTypes.CS_ClauserUnit &&
                            text.pe_tag_revised == "NULL");
            }
        }

        public bool ClauserProceededByComma
        {
            get
            {
                return Sentence.Texts.Skip(ClauserIndexPosition).Any(
                            text =>
                                ((text.pe_tag_revised.IsNull() && text.pe_tag == UnitTypes.BKP_BreakerPunctuation)
                                || (text.pe_tag_revised == UnitTypes.BKP_BreakerPunctuation))
                                && text.pe_text==" , ");
            }
        }

        public List<Text> GetClauserUnit(
            int clauserPosition, int lastIndexPosition)
        {
            var clauserTexts = new List<Text>();
            for (int i = clauserPosition; i <= lastIndexPosition; i++)
            {
                clauserTexts.Add(Sentence.Texts[i]);
            }

            return clauserTexts;
        }
    }
}