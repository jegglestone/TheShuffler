using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ShufflerLibrary.DataAccess
{
    using System;

    public class ShufflerDataAccess : IDataAccess
    {
        private readonly string connectionString =
                ConfigurationManager.
                    ConnectionStrings["ShufflerDatabaseConnection"].ConnectionString;

        private SqlDataReader dataReader;

        private SqlConnection cn;

        public IDataReader GetDataReader(int pe_pmd_id)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.GetDocumentById"
            };
            command.Parameters.AddWithValue(
                "@pe_pmd_id", pe_pmd_id);

            cn = new SqlConnection(
                connectionString);

            command.Connection = cn;

            cn.Open();

            dataReader = command.ExecuteReader(
                CommandBehavior.CloseConnection);
            
            return dataReader;
        }

        public bool SaveText(int pePmdID, int peUserID, int peParaNo, int pePhraseID, int? peWordID, string peTag, string peText,
            string peTagRevised, int peMergeAhead, string peTextRevised, string peRuleApplied, int peOrder, int peCNum)
        {
            using (cn = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "dbo.[SaveShuffledText]",
                    Connection = cn,
                };
                command.Parameters.AddWithValue("@pe_pmd_id", pePmdID);
                command.Parameters.AddWithValue("@pe_user_id", peUserID);
                command.Parameters.AddWithValue("@pe_para_no", peParaNo);
                command.Parameters.AddWithValue("@pe_phrase_id", pePhraseID);
                command.Parameters.AddWithValue("@pe_word_id", peWordID);
                command.Parameters.AddWithValue("@pe_tag", peTag);
                command.Parameters.AddWithValue("@pe_text", peText);
                command.Parameters.AddWithValue("@pe_tag_revised", peTagRevised);
                command.Parameters.AddWithValue("@pe_merge_ahead", peMergeAhead);

                if (peTextRevised == "NULL")
                    command.Parameters.AddWithValue("@pe_text_revised", DBNull.Value);
                else command.Parameters.AddWithValue("@pe_text_revised", peTextRevised);

                command.Parameters.AddWithValue("@pe_rule_applied", peRuleApplied);
                command.Parameters.AddWithValue("@pe_order", peOrder);
                command.Parameters.AddWithValue("@pe_C_num", peCNum);
                cn.Open();

                command.Dispose();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public void Dispose()
        {
            // dispose connection and datareader etc
            if (!dataReader.IsClosed)
                dataReader.Close();
            dataReader?.Dispose();
            cn?.Dispose();
        }
    }
}
