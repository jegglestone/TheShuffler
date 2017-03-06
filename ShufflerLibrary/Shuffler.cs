using System;

namespace ShufflerLibrary
{
    using Repository;
    using DataAccess;
    using Helper;
    using Model;
    using Strategy;

    public class Shuffler : IShuffler
    {
        private readonly IShufflerPhraseRepository 
            _shufflerPhraseRepository;

        private readonly IStrategy
            _clauserUnitStrategy;

        private readonly IStrategy
            _adverbUnitStrategy;

        private readonly IStrategy
            _timerUnitStrategy;

        private readonly IStrategy
            _mDUnitStrategy;

        private readonly IStrategy
            _ddlUnitStrategy;

        private readonly IStrategy
            _pyYoUnitStrategy;

        private readonly IStrategy
            _mdbkUnitStrategy;

        private readonly IStrategy
            _percentUnitStrategy;

        private readonly IStrategy
            _mdNulThatStrategy;

        public Shuffler()
        {
            _shufflerPhraseRepository = 
                new ShufflerPhraseRepository(
                    new ShufflerDataAccess());

            _clauserUnitStrategy = 
                new ClauserUnitStrategy();

            _adverbUnitStrategy =
                new AdverbUnitStrategy();

            _timerUnitStrategy = 
                new TimerUnitStrategy();
      
            _mDUnitStrategy = 
                new MdUnitStrategy();

            _ddlUnitStrategy =
                new DdlUnitStrategy();

            _pyYoUnitStrategy =
                new PyYoUnitStrategy();

            _mdbkUnitStrategy = 
                new MdbkUnitStrategy();

            _percentUnitStrategy =
                new PercentUnitStrategy();

            _mdNulThatStrategy = 
                new MdNulThatUnitStrategy();
        }

        public bool ShuffleParagraph(int pePmdId)
        {
            var document = 
                _shufflerPhraseRepository.GetShufflerDocument(pePmdId);
            
            for (int i = 0; i < document.Paragraphs.Count; i++)
            {
                var paragraph = document.Paragraphs[i];

                for (int j = 0; j < paragraph.Sentences.Count; j++)
                {
                    var sentence = paragraph.Sentences[j];
                    sentence.Sentence_No = j + 1;
                    sentence.Sentence_Identifier = Guid.NewGuid();
                    ShuffleSentence(paragraph, j, sentence);
                }
            }
            
            return _shufflerPhraseRepository
                .SaveShuffledDocument(document);
        }

        private void ShuffleSentence(
            Paragraph paragraph, int index, Sentence sentence)
        {
            ShuffledStateHelper.AddShuffledState(sentence, "Before_Shuffling");

            sentence = _adverbUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_ADV");

            sentence = _timerUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_TM");

            #region not in use
            //if (sentence.SentenceHasMultipleOptions)
            //{
            //    ApplySubsequentStrategiesToMultipleSentences(
            //        sentence);
            //}
            //else
            //{
            //    sentence = ApplySubsequentStrategiesToSentence(
            //        sentence);
            //}
            #endregion

            sentence = _mDUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_MD");
            
            sentence = _mdbkUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_MDBK");

            sentence = _mdNulThatStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_MDNUL");

            sentence = _ddlUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_DDL");

            sentence = _pyYoUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_YO");

            sentence = _percentUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_Percent");

            sentence = _clauserUnitStrategy.ShuffleSentence(sentence);
            ShuffledStateHelper.AddShuffledState(sentence, "Shuffle_CS");

            sentence = SentenceOrderReSetter.SetPeOrderAsc(sentence);

            paragraph.Sentences[index] = sentence;
        }

      #region multi sentence options
    //private void ApplySubsequentStrategiesToMultipleSentences(Sentence sentence)
    //{
    //    var sentences =
    //        SentenceHelper.SplitSentenceOptions(sentence);

    //    sentence.Texts.Clear();

    //    foreach (var optionSentence in sentences)
    //    {
    //        ApplySubsequentStrategiesToSentence(
    //            optionSentence);

    //        sentence.Texts.AddRange(optionSentence.Texts);
    //    }
    //}

    //private Sentence ApplySubsequentStrategiesToSentence(Sentence sentence)
    //{
    //    sentence = _mDUnitStrategy.ShuffleSentence(sentence);

    //    sentence = _nulThatStrategy.ShuffleSentence(sentence);

    //    sentence = _doublePrenStrategy.ShuffleSentence(sentence);

    //    sentence = _prenNnPastUnitStrategy.ShuffleSentence(sentence);

    //    sentence = _commaUnitStrategy.ShuffleSentence(sentence);

    //    return sentence;
    //}
    #endregion
  }
}
