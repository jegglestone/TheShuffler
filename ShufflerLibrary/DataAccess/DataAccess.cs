using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ShufflerLibrary.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public IDataReader GetDataReader(int pe_pmd_id)
        {

            // todo: sort this into a sproc or something
            var command = new SqlCommand(@"
                SELECT * FROM [dbo].[v3_Phrase_Element]
                WHERE pe_pmd_id = " + pe_pmd_id +
                "ORDER BY pe_order");

            var cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["ShufflerDatabaseConnection"].ConnectionString);
            command.Connection = cn;
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            return dr;
        }
    }
}
