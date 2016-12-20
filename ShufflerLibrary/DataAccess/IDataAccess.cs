using System.Data;

namespace ShufflerLibrary.DataAccess
{
    public interface IDataAccess
    {
        IDataReader GetDataReader(int pe_pmd_id);
    }
}
