namespace Main
{
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;
    using Interfaces;
    using Model;

    public class ModifierStrategy : IShuffleStrategy
    {
        private Sentence _sentence;
        private readonly IModifierFormatter _modifierFormatter;

        public ModifierStrategy(IModifierFormatter modifierFormatter)
        {
            _modifierFormatter = modifierFormatter;
        }

        public Paragraph ShuffleSentenceUnit(Paragraph xmlSentenceElement)
        {
            _sentence = new Sentence(xmlSentenceElement);
            Text[] sentenceArray = _sentence.SentenceArray;

            if (_sentence.UnitNotFoundInSentence(
                sentenceArray,
                element => element.InnerText.IsModifier()))
                return xmlSentenceElement;

            _sentence.ModifierCount = 0;
            var modifierUnits = GetModifierUnits(
                sentenceArray).ToArray<IMoveableUnit>();

            _sentence.UnderlineJoinedSentenceUnit(
                sentenceArray,
                modifierUnits[0].StartPosition,
                modifierUnits[_sentence.ModifierCount - 1].EndPosition);

            Text[] modifierUnitsInSerialNumberOrder =
                _sentence.GetMoveableUnitsInSerialNumberDescendingOrder(
                    _sentence.ModifierCount, modifierUnits, sentenceArray);

            Text[] newSentence;

            if (_sentence.HasPrenToTheLeft(
                sentenceArray, modifierUnits[0].StartPosition))
                newSentence = MoveModifierUnitBeforePren(
                    sentenceArray,
                    modifierUnitsInSerialNumberOrder,
                    modifierUnits[0].StartPosition);
            else
            {
                var beforeModifier =
                    sentenceArray.Take(modifierUnits[0].StartPosition);

                newSentence =
                    beforeModifier
                    .Concat(_modifierFormatter.ApplyFormattingRules(
                        modifierUnitsInSerialNumberOrder)).ToArray();
            }

            newSentence = _sentence.RemoveAnyBlankSpaceFromEndOfUnit
                (newSentence
                    .Concat(
                        _sentence.GetSentenceBreaker(sentenceArray))
                        .ToArray().RemoveAnyDoubleSpaces());

            return new Paragraph(
                OpenXmlHelper.BuildWordsIntoOpenXmlElement(
                    newSentence));
        }

        private Text[] MoveModifierUnitBeforePren(
            Text[] sentenceArray, 
            Text[] modifierUnitsInSerialNumberOrder, 
            int modiferUnitStartPosition)
        {
            int prenUnitPosition
                = _sentence.GetPositionOfClosestSpecifiedUnit(
                    sentenceArray,
                    modiferUnitStartPosition,
                    t => t.Text.IsPren());

            var modifierTagAndUnit =
                sentenceArray
                .Skip(prenUnitPosition)
                .Take(modiferUnitStartPosition-prenUnitPosition).ToArray();

            var beforePrenUnitPosition =
                sentenceArray
                    .Take(prenUnitPosition).ToArray();

            modifierUnitsInSerialNumberOrder =
                _modifierFormatter.ApplyFormattingRules(modifierUnitsInSerialNumberOrder);

            return
                beforePrenUnitPosition
                    .Concat(modifierUnitsInSerialNumberOrder)
                    .Concat(modifierTagAndUnit).ToArray();
        }

        private IEnumerable<MoveableUnit> GetModifierUnits(
            IList<Text> sentenceArray)
        {
            var modifierUnits =
                new MoveableUnit[sentenceArray.Count(
                    x => x.InnerText.IsModifier())];

            int modifierUnitCount = 0;

            _sentence.PopulateMoveableModifierUnits(
                sentenceArray, ref modifierUnitCount, modifierUnits);

            _sentence.ModifierCount = modifierUnitCount;
            return modifierUnits;
        } 
    }
}
