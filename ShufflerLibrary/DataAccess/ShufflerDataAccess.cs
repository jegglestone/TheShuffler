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

        public IDataReader GetDataReader(int pePmdId)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.GetDocumentById"
            };
            command.Parameters.AddWithValue(
                "@pe_pmd_id", pePmdId);

            cn = new SqlConnection(
                connectionString);

            command.Connection = cn;

            cn.Open();

            dataReader = command.ExecuteReader(
                CommandBehavior.CloseConnection);
            
            return dataReader;
        }

        public bool SaveText(
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
            int selectedOptionSelected)
        {
            using (cn = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "dbo.[SaveShuffledText]",
                    Connection = cn,
                };
                
                command.Parameters.AddWithValue("@pe_pmd_id", pePmdId);
                command.Parameters.AddWithValue("@pe_user_id", peUserId);
                command.Parameters.AddWithValue("@pe_para_no", peParaNo);
                command.Parameters.AddWithValue("@pe_phrase_id", pePhraseId);
                command.Parameters.AddWithValue("@pe_word_id", peWordId);
                command.Parameters.AddWithValue("@pe_tag", peTag);
                command.Parameters.AddWithValue("@pe_text", peText);
                command.Parameters.AddWithValue("@pe_merge_ahead", peMergeAhead);

                if (peTagRevised != null 
                    && peTagRevised.ToLower() != "null")
                {
                  command.Parameters.AddWithValue("@pe_tag_revised", peTagRevised);
                }

                if (peTextRevised != null 
                    && peTextRevised.ToLower() != "null")
                {
                  command.Parameters.AddWithValue("@pe_text_revised", peTextRevised);
                }
             
                command.Parameters.AddWithValue("@pe_rule_applied", peRuleApplied);
                command.Parameters.AddWithValue("@pe_order", peOrder);
                command.Parameters.AddWithValue("@pe_C_num", peCNum);
        
                command.Parameters.AddWithValue("@sentence_no", sentenceNumber);
                command.Parameters.AddWithValue("@sentence_option", sentenceOption);
                command.Parameters.AddWithValue("@sentence_option_selected", selectedOptionSelected);

                command.Parameters.AddWithValue("@sentence_identifier", sentenceIdentifier);

                cn.Open();

                command.Dispose();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public bool SaveShuffledState(
          Guid sentenceIdentifier, string sentenceStructure, string ruleApplied)
        {
            using (cn = new SqlConnection(connectionString))
            {
              var command = new SqlCommand
              {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.[SaveShuffledState]",
                Connection = cn,
              };

              command.Parameters.AddWithValue("@sentenceIdentifier", sentenceIdentifier);
              command.Parameters.AddWithValue("@sentenceStructure", sentenceStructure);
              command.Parameters.AddWithValue("@ruleApplied", ruleApplied);

              cn.Open();

              command.Dispose();
              return command.ExecuteNonQuery() == 1;
            }
        }

        public void Dispose()
        {
            if (!dataReader.IsClosed)
                dataReader.Close();
            dataReader?.Dispose();
            cn?.Dispose();
        }

    }
}
