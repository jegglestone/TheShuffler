namespace ShufflerLibrary.Repository
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Model;

    public class ShufflerPhraseRepository : IShufflerPhraseRepository
    {
        public Document GetShufflerDocument(int pe_pmd_id)
        {
            var document = new Document();

            var texts = new List<Text>();

            // todo: sort this into a sproc or something
            var command = new SqlCommand(@"
                SELECT * FROM [dbo].[v3_Phrase_Element]
                WHERE pe_pmd_id = "  + pe_pmd_id +
                "ORDER BY pe_order"); 

            var cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["ShufflerDatabaseConnection"].ConnectionString);
            command.Connection = cn;
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                texts.Add(new Text
                {
                    pe_C_num = int.Parse(dr["pe_C_num"].ToString())
                    , pe_merge_ahead = int.Parse(dr["pe_merge_ahead"].ToString())
                    , pe_order = int.Parse(dr["pe_order"].ToString())
                    , pe_para_no = int.Parse(dr["pe_para_no"].ToString()) // Inherited association key
                    , pe_phrase_id = int.Parse(dr["pe_phrase_id"].ToString())
                    , pe_rule_applied = dr["pe_rule_applied"].ToString()
                    , pe_tag = dr["pe_tag"].ToString()
                    , pe_tag_revised = dr["pe_tag_revised"].ToString()
                    , pe_text = dr["pe_text"].ToString()
                    , pe_text_revised = dr["pe_text_revised"].ToString()
                    , pe_user_id = int.Parse(dr["pe_user_id"].ToString())
                    , pe_word_id = int.Parse(dr["pe_word_id"].ToString())
                }); 

                if (document.pe_pmd_id == 0)
                    document.pe_pmd_id = 1234;
            }

            return null;
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
