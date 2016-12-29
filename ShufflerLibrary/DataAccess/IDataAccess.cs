using System.Data;

namespace ShufflerLibrary.DataAccess
{
    using System;
    public interface IDataAccess : IDisposable
    {
        IDataReader GetDataReader(int pe_pmd_id);

        bool SaveText(int pePmdID, int peUserID, int peParaNo, 
            int pePhraseID, int? peWordID, string peTag, 
            string peText, string peTagRevised, int peMergeAhead, 
            string peTextRevised, string peRuleApplied, 
            int peOrder, int peCNum);
    }
}
