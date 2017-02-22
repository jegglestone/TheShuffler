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

        public Document GetShufflerDocument(int pePmdId)
        {
            var document = new Document();
            IDataReader dr = _dataAccess.GetDataReader(pePmdId);
            var texts = new List<Text>();

            var paragraph = new Paragraph();

            while (dr.Read())
            {
                SetDocumentId(pePmdId, document);

                var text = CreateText(dr);
                texts.Add(text);

                //TODO: Could be ! or ? in future requirements. 
                if (text.pe_text.Replace(" ", "") == ".") 
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
            _dataAccess.Dispose();
            dr.Close();
            dr.Dispose();
            return document;
        }

        private static void SetDocumentId(int pePmdId, Document document)
        {
            if (document.pe_pmd_id == 0)
                document.pe_pmd_id = pePmdId;
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

            if (dr["pe_text_revised"] is DBNull)
                text.pe_text_revised = null;
            if (dr["pe_tag_revised"] is DBNull)
                text.pe_tag_revised = null;


            return text;
        }

        public bool SaveShuffledDocument(Document document)
        {
            foreach (var paragraph in document.Paragraphs)
            {
                foreach (var sentence in paragraph.Sentences)
                {
                    foreach (var text in sentence.Texts)
                    {
                        try
                        {
                            if (
                                _dataAccess.SaveText(
                                    document.pe_pmd_id,
                                    text.pe_user_id,
                                    paragraph.pe_para_no,
                                    text.pe_phrase_id,
                                    text.pe_word_id,
                                    text.pe_tag,
                                    text.pe_text,
                                    text.pe_tag_revised,
                                    text.pe_merge_ahead,
                                    text.pe_text_revised,
                                    text.pe_rule_applied,
                                    text.pe_order,
                                    text.pe_C_num,
                                    sentence.Sentence_Identifier,
                                    sentence.Sentence_No,
                                    text.Sentence_Option,
                                    sentence.Sentence_Option_Selected) == false)
                                return false;
                        }
                        finally
                        {
                            _dataAccess.Dispose();
                        }
                    }

                    foreach (var shuffledState in sentence.ShuffledStates)
                    {
                        try
                        {
                            if (_dataAccess.SaveShuffledState(
                                    shuffledState.SentenceIdentifier,
                                    shuffledState.SentenceState,
                                    shuffledState.StrategyApplied) == false)
                            return false;
                        }
                        finally
                        {
                            _dataAccess.Dispose();
                        }
                   }
                }
            }
            return true;
        }
    }

    public interface IShufflerPhraseRepository
    {
        Document GetShufflerDocument(int pePmdId);

        bool SaveShuffledDocument(Document document);
    }

}
