using System.Data;
using System.Data.SqlTypes;

namespace ShufflerLibrary.DataAccess
{
    using System;
    public interface IDataAccess : IDisposable
    {
        IDataReader GetDataReader(int pePmdId);

        bool SaveText(
            int pePmdId, 
            int peUserId, 
            int peParaNo, 
            int pePhraseId, 
            int? peWordId, 
            string peTag, 
            string peText, 
            string peTagRevised, 
            int peMergeAhead, 
            string peTextRevised, 
            string peRuleApplied, 
            int peOrder, 
            int peCNum, 
            Guid sentenceIdentifier,
            int sentenceNumber, 
            int sentenceOption, 
            int selectedOptionSelected);

      bool SaveShuffledState(
          Guid sentenceIdentifier,
          string sentenceStructure,
          string ruleApplied);
    }
}
