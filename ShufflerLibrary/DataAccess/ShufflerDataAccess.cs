using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ShufflerLibrary.DataAccess
{
    public class ShufflerDataAccess : IDataAccess
    {
        public IDataReader GetDataReader(int pe_pmd_id)
        {
            // todo: sort this into a parameterised sproc or something
            var command = new SqlCommand(@"
                SELECT * FROM [dbo].[v3_Phrase_Element]
                WHERE pe_pmd_id = " + pe_pmd_id +
                "ORDER BY pe_order");

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
    }
}
