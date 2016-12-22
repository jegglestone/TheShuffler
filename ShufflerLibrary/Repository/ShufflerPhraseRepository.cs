namespace ShufflerLibrary.Repository
{
    using System;
    using System.Collections.Generic;
    using Model;
    using DataAccess;
    using System.Data;

    public class ShufflerPhraseRepository : IShufflerPhraseRepository
    {
        private readonly IDataAccess _dataAccess;

        public ShufflerPhraseRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Document GetShufflerDocument(int pe_pmd_id)
        {
            var document = new Document();
            IDataReader dr = _dataAccess.GetDataReader(pe_pmd_id);
            var texts = new List<Text>();

            var paragraph = new Paragraph();

            while (dr.Read())
            {
                SetDocumentId(pe_pmd_id, document);

                var text = CreateText(dr);
                texts.Add(text);

                if (text.pe_text.Replace(" ", "") == ".")//TOTO: Could be ! or ? in future
                {
                    paragraph.Sentences.Add(
                        new Sentence()
                        {
                            pe_para_no = text.pe_para_no,
                            Texts = texts
                        });

                    texts = new List<Text>();
                }


                if (paragraph.pe_para_no != text.pe_para_no)
                {
                    paragraph = new Paragraph {pe_para_no = text.pe_para_no};
                    document.Paragraphs.Add(paragraph);
                }
            }
        




            // TODO: AGGREGATE TEXTS INTO SENTENCES WHERE THERE IS A BKP.

            // TODO: IF pe_para_no CHANGES THEN ADD A NEW PARAGRAPH

            return document;
        }

        private static void SetDocumentId(int pe_pmd_id, Document document)
        {
            if (document.pe_pmd_id == 0)
                document.pe_pmd_id = pe_pmd_id;
        }

        private static Text CreateText(IDataRecord dr)
        {
            var text = new Text
            {
                pe_merge_ahead = int.Parse(dr["pe_merge_ahead"].ToString()),
                pe_order = int.Parse(dr["pe_order"].ToString()),
                pe_para_no = int.Parse(dr["pe_para_no"].ToString()), // Inherited association key,
                pe_rule_applied = dr["pe_rule_applied"].ToString(),
                pe_tag = dr["pe_tag"].ToString(),
                pe_tag_revised = dr["pe_tag_revised"].ToString(),
                pe_text = dr["pe_text"].ToString(),
                pe_text_revised = dr["pe_text_revised"].ToString(),
                pe_user_id = int.Parse(dr["pe_user_id"].ToString())
            };
            if (!(dr["pe_phrase_id"] is DBNull))
                text.pe_phrase_id = int.Parse(dr["pe_phrase_id"].ToString());
            if (!(dr["pe_word_id"] is DBNull))
                text.pe_word_id = int.Parse(dr["pe_word_id"].ToString());
            if (!(dr["pe_C_num"] is DBNull))
                text.pe_C_num = int.Parse(dr["pe_C_num"].ToString());
            return text;
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
