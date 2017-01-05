namespace ShufflerLibrary
{
    using System.Diagnostics;
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
        }

        public bool ShuffleParagraph(int pe_pmd_id)
        {
            var document = 
                _shufflerPhraseRepository.GetShufflerDocument(pe_pmd_id);
            
            for (int i = 0; i < document.Paragraphs.Count; i++)
            {
                var paragraph = document.Paragraphs[i];
                for (int index = 0; index < paragraph.Sentences.Count; index++)
                {
                    var sentence = paragraph.Sentences[index];
                    ShuffleSentence(paragraph, index, sentence);
                }
            }
            
            // save the output back to the database
            return _shufflerPhraseRepository.SaveShuffledDocument(document);
        }

        private void ShuffleSentence(Paragraph paragraph, int index, Sentence sentence)
        {
            sentence =
                _clauserUnitStrategy.ShuffleSentence(sentence);
   
            sentence =
                _adverbUnitStrategy.ShuffleSentence(sentence);
            
           sentence =
               _timerUnitStrategy.ShuffleSentence(sentence);
              
            // more strategies here

            //finally
            sentence =
                SentenceOrderReSetter.SetPeOrderAsc(sentence);

            paragraph.Sentences[index] =
                sentence;
        }
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
