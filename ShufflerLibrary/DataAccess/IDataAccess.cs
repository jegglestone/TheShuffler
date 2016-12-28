using System.Data;

namespace ShufflerLibrary.DataAccess
{
    public interface IDataAccess
    {
        IDataReader GetDataReader(int pe_pmd_id);

        void SaveText(int pePmdID, int peUserID, int peParaNo, 
            int pePhraseID, int? peWordID, string peTag, 
            string peText, string peTagRevised, int peMergeAhead, 
            string peTextRevised, string peRuleApplied, 
            int peOrder, int peCNum);
    }
}
