namespace ShufflerLibrary
{
    using Repository;

    public class Shuffler : IShuffler
    {
        private readonly IShufflerPhraseRepository _shufflerPhraseRepository;

        public Shuffler()
        {
            _shufflerPhraseRepository = new ShufflerPhraseRepository();
        }

        public bool ShuffleParagraph(int pe_pmd_id)
        {
            // get the sentence from the database
            _shufflerPhraseRepository.GetShufflerDocument(pe_pmd_id);

            // run each sentence through our shuffle strategies 

            // save the output back to the database

            return true;
        }
    }
}

/*
Pe_para_no(= paragraph number, for final output purposes).
Pe_tag / pe_tag_revised – I use ISNULL(pe_tag_revised, pe_tag) to return the revised tag, or if null, 
the original tag. The tag is the superscript text which precedes the text.
Pe_text / pe_text_revised – I use ISNULL as above to return revised text, or, if null, the original text.
Where pe_text_revised is blank(not null) this indicates that that word has been removed (ie ignore these).
Pe_merge_ahead – this defines “units” (word groups which are underlined together). 
If not zero, then include next NN words/phrases as part of the same unit.
Pe_order – I introduced this recently, to allow me to insert words during the primer stage 
(word gets added to the end of the table).
 
The output table (“Shuffled”) would have different values in pe_order column (numbering method to suit you),
but all other values (except, where relevant, pe_text_revised) unchanged. To remove a word, 
simply make pe_text_revised a blank string. Or, if you need to change a word, leave pe_text 
unchanged and set a new value for pe_text_revised.I’m not sure if words get added during the 
shuffler process – if they do, we’ll need to agree on what goes in each column.

*/
