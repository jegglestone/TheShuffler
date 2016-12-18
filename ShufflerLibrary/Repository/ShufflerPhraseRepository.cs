namespace ShufflerLibrary.Repository
{
    using Model;

    public class ShufflerPhraseRepository : IShufflerPhraseRepository
    {
        public Document GetShufflerDocument(int pe_pmd_id)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveShuffledDocument(Document document)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IShufflerPhraseRepository
    {
        Document GetShufflerDocument(int pe_pmd_id);

        bool SaveShuffledDocument(Document document);
    }

}
