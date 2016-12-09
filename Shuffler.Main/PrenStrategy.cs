namespace Main
{
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Extensions;
    using Helper;
    using Interfaces;
    using Model;

    public class PrenStrategy : IShuffleStrategy
    {
        private Sentence _sentence;

        public Paragraph ShuffleSentenceUnit(Paragraph xmlSentenceElement)
        {
            _sentence = new Sentence(xmlSentenceElement);
            Text[] sentenceArray = _sentence.SentenceArray;

            if (_sentence.UnitNotFoundInSentence(
                sentenceArray,
                element => element.InnerText.IsPren()))
                return xmlSentenceElement;

            _sentence.PrenUnitCount = 0;
            var prenUnits = GetPrenUnits(
                sentenceArray).ToArray<IMoveableUnit>();

            Text[] prenUnitsInSerialNumberDescendingOrder =
                _sentence.GetMoveableUnitsInSerialNumberDescendingOrder(
                    _sentence.PrenUnitCount, prenUnits, sentenceArray)
                    .RemoveTags("NN"); //TagMarks.Noun;

            Text[] beforePrenUnits = 
                sentenceArray.Take(prenUnits[0].StartPosition).ToArray();

            Text[] newSentence = _sentence.RemoveAnyBlankSpaceFromEndOfUnit(
                beforePrenUnits
                    .Concat(prenUnitsInSerialNumberDescendingOrder)
                    .Concat(_sentence.GetSentenceBreaker(sentenceArray))
                    .ToArray()
                    .RemoveAnyDoubleSpaces());

            return new Paragraph(
                OpenXmlHelper.BuildWordsIntoOpenXmlElement(
                    newSentence));
        }

        private IEnumerable<MoveableUnit> GetPrenUnits(
            IList<Text> sentenceArray)
        {
            var prenUnits =
                new MoveableUnit[sentenceArray.Count(
                    x => x.InnerText.IsPren())];

            int prenUnitCount = 0;

            _sentence.PopulateMoveablePrenUnits(
                sentenceArray, ref prenUnitCount, prenUnits);

            _sentence.PrenUnitCount = prenUnitCount;
            return prenUnits;
        }
    }
}
