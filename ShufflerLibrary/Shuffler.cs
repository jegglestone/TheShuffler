using System;
using System.Text;

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
            AddShuffledState(sentence, "Before_Shuffling");

            sentence = _clauserUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_CS");

            sentence = _adverbUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_ADV");

            sentence = _timerUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_TM");

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
            AddShuffledState(sentence, "Shuffle_MD");
            
            sentence = _mdbkUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_MDBK");

            sentence = _mdNulThatStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_MDNUL");

            sentence = _ddlUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_DDL");

            sentence = _pyYoUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_YO");

            sentence = _percentUnitStrategy.ShuffleSentence(sentence);
            AddShuffledState(sentence, "Shuffle_Percent");
            
            sentence = SentenceOrderReSetter.SetPeOrderAsc(sentence);

            paragraph.Sentences[index] = sentence;
        }

      private static void AddShuffledState(Sentence sentence, string ruleApplied)
      {
        StringBuilder sentenceLineStringBuilder = new StringBuilder();
        foreach (var text in sentence.Texts)
        {
          sentenceLineStringBuilder.Append(text.actual_text_used);
        }

        sentence.ShuffledStates.Add(new ShuffledState()
        {
          SentenceIdentifier = sentence.Sentence_Identifier,
          SentenceState = sentenceLineStringBuilder.ToString(),
          StrategyApplied = ruleApplied
        });
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

/*
Pe_para_no(= paragraph number, for final output purposes).
Pe_tag / pe_tag_revised – I use ISNULL(pe_tag_revised, pe_tag) to return 
the revised tag, or if null, 
the original tag. The tag is the superscript text which precedes the text.
Pe_text / pe_text_revised – I use ISNULL as above to return revised text, or,
if null, the original text.
Where pe_text_revised is blank(not null) this indicates that that word has 
been removed (ie ignore these).
Pe_merge_ahead – this defines “units” (word groups which are underlined 
together). 
If not zero, then include next NN words/phrases as part of the same unit.
Pe_order – I introduced this recently, to allow me to insert words during the 
primer stage 
(word gets added to the end of the table).
 
The output table (“Shuffled”) would have different values in pe_order column 
(numbering method to suit you),
but all other values (except, where relevant, pe_text_revised) unchanged. 
To remove a word, 
simply make pe_text_revised a blank string. Or, if you need to change a word, 
leave pe_text 
unchanged and set a new value for pe_text_revised.I’m not sure if words get 
added during the shuffler process – if they do, we’ll need to agree on what 
goes in each column.

*/