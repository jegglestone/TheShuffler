using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ShufflerLibrary.DataAccess
{
    public class ShufflerDataAccess : IDataAccess
    {
        public IDataReader GetDataReader(int pe_pmd_id)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.GetDocumentById"
            };
            command.Parameters.AddWithValue(
                "@pe_pmd_id", pe_pmd_id); 

            string connectionString = 
                ConfigurationManager.
                    ConnectionStrings["ShufflerDatabaseConnection"].ConnectionString;

            var cn = new SqlConnection(
                connectionString);

            command.Connection = cn;

            cn.Open();

            SqlDataReader dr = command.ExecuteReader(
                CommandBehavior.CloseConnection);

            return dr;
        }

        public void SaveText(int pePmdID, int peUserID, int peParaNo, int pePhraseID, int? peWordID, string peTag, string peText,
            string peTagRevised, int peMergeAhead, string peTextRevised, string peRuleApplied, int peOrder, int peCNum)
        {
            throw new System.NotImplementedException();
        }
    }
}
