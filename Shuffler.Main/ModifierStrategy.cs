namespace Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Interfaces;
    using Model;

    public class ModifierStrategy : IShuffleStrategy
    {
        private Sentence _sentence;

        public Paragraph ShuffleSentenceUnit(Paragraph xmlSentenceElement)
        {
            _sentence = new Sentence(xmlSentenceElement);
            Text[] sentenceArray = _sentence.SentenceArray;

            if (_sentence.UnitNotFoundInSentence(
                sentenceArray,
                element => element.InnerText.IsModifier()))
                return xmlSentenceElement;

            _sentence.TimerUnitCount = 0;
            var modifierUnits = GetModifierUnits(
                sentenceArray).ToArray<IMoveableUnit>();

            throw new NotImplementedException();

            
            // reverse them
            
        }

        private IEnumerable<MoveableUnit> GetModifierUnits(
            IList<Text> sentenceArray)
        {
            var modifierUnits =
                new MoveableUnit[sentenceArray.Count(
                    x => x.InnerText.IsModifier())];

            int modifierUnitCount = 0;

            _sentence.PopulateMoveableUnits(
                sentenceArray, ref modifierUnitCount, modifierUnits);

            _sentence.ModifierCount = modifierUnitCount;
            return modifierUnits;
        }
    }
}
