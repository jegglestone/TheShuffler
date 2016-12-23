namespace ShufflerLibrary.Tests.Unit
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using DataAccess;
    using NUnit.Framework;
    using Repository;
    using Rhino.Mocks;

    [TestFixture]
    public class ShufflerPhraseRepositoryUnit
    {
        private IDataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _dataReader = MockRepository.GenerateStub<SqlDataReader>();
        }

        [Test]
        public void PassesSentenceToClauserStrategy()
        {
            _dataReader.Stub(x => x.Read()).Repeat.Twice().Return(true);

            _dataReader.Stub(x => x["pe_pmd_id"]).Return(14.15d);
            _dataReader.Stub(x => x["pe_user_id"]).Return("1");
            _dataReader.Stub(x => x["pe_para_no"]).Return("1");
            _dataReader.Stub(x => x["pe_phrase_id"]).Return("1");
            _dataReader.Stub(x => x["pe_word_id"]).Return("29");
            _dataReader.Stub(x => x["pe_tag"]).Return("BKP");
            _dataReader.Stub(x => x["pe_text"]).Return(".");
            _dataReader.Stub(x => x["pe_tag_revised"]).Return("NULL");
            _dataReader.Stub(x => x["pe_merge_ahead"]).Return("0");
            _dataReader.Stub(x => x["pe_text_revised"]).Return("NULL");
            _dataReader.Stub(x => x["pe_rule_applied"]).Return("NULL");
            _dataReader.Stub(x => x["pe_order"]).Return("1214420");
            _dataReader.Stub(x => x["pe_C_num"]).Return("2");

            var mockDataAccess = MockRepository.GenerateStub<IDataAccess>();
            mockDataAccess.Stub(x => x.GetDataReader(Arg<int>.Is.Anything)).Return(
                _dataReader);

            ShufflerPhraseRepository shufflerPhraseRepository
                = new ShufflerPhraseRepository(
                    mockDataAccess);

            var document = shufflerPhraseRepository.GetShufflerDocument(1234);

            Assert.That(document, Is.Not.Null);
            var firstOrDefault = document.Paragraphs.FirstOrDefault();
            Assert.That(firstOrDefault != null && firstOrDefault.Sentences.Count == 1);
        }
    }
}
