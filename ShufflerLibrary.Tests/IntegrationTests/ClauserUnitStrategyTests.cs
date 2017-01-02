using NUnit.Framework;

namespace ShufflerLibrary.Tests.IntegrationTests
{
    using DataAccess;
    using Model;
    using Repository;
    using Strategy;

    [TestFixture]
    public class ClauserUnitStrategyTests
    {
        private Document _document;
        private ShufflerPhraseRepository _shufflerPhraseRepository;

        [Test]
        public void When_Clauser_At_Start_Of_sentence_dont_shuffle()
        {
            var sentence = new Sentence();
            sentence.Texts.Add(
            new Text()
            {
                pe_tag_revised = "CS",
                pe_text = "Before"
            });
            sentence.Texts.Add(new Text()
            {
                pe_text = "today"
            });
            sentence.Texts.Add(new Text()
            {
                pe_tag_revised="NBKP",
                pe_text = " , "
            });
            sentence.Texts.Add(new Text()
            {
                pe_tag_revised = "PAST",
                pe_text = " was "
            });
            sentence.Texts.Add(new Text()
            {
                pe_tag_revised = "ADJ",
                pe_text = " great "
            });
            sentence.Texts.Add(new Text()
            {
                pe_tag_revised = "BKP",
                pe_text = " . "
            });

            var clauserUnitStrategy =
                new ClauserUnitStrategy();

            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                sentence);
            
            Assert.That(sentenceReturned.Texts[0].pe_text == "Before");
            Assert.That(sentenceReturned.Texts[1].pe_text == "today");
            Assert.That(sentenceReturned.Texts[2].pe_text == " , ");
            Assert.That(sentenceReturned.Texts[3].pe_text == " was ");
            Assert.That(sentenceReturned.Texts[4].pe_text == " great ");
            Assert.That(sentenceReturned.Texts[5].pe_text == " . ");
        }

        [Test]
        public void When_Clauser_With_NBKP_WithoutNulThat_MoveToBeginningOfSentence()
        {
            // TMIn April and May NBKP, CShowever NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.
            _shufflerPhraseRepository
                = new ShufflerPhraseRepository(new ShufflerDataAccess());

            _document = _shufflerPhraseRepository.GetShufflerDocument(2016);

            var clauserUnitStrategy =
                new ClauserUnitStrategy();

            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                _document.Paragraphs[0].Sentences[0]);

            // CShowever NBKP, TMIn April and May NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.
            Assert.That(sentenceReturned.Texts.Count == 12);
            Assert.That(sentenceReturned.Texts[0].pe_text, Is.EqualTo(" however "));
            Assert.That(sentenceReturned.Texts[1].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[2].pe_text, Is.EqualTo(" In "));
            Assert.That(sentenceReturned.Texts[3].pe_text, Is.EqualTo(" April "));
            Assert.That(sentenceReturned.Texts[4].pe_text, Is.EqualTo(" and "));
            Assert.That(sentenceReturned.Texts[5].pe_text, Is.EqualTo(" May "));
            Assert.That(sentenceReturned.Texts[6].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[7].pe_text, Is.EqualTo(" the "));
            Assert.That(sentenceReturned.Texts[8].pe_text, Is.EqualTo(" report "));
            Assert.That(sentenceReturned.Texts[9].pe_text, Is.EqualTo(" wasn’t "));
            Assert.That(sentenceReturned.Texts[10].pe_text, Is.EqualTo(" good "));
            Assert.That(sentenceReturned.Texts[11].pe_text, Is.EqualTo(" . "));
        }
        
        [Test]
        public void When_Clauser_And_NulThat_WithoutNBKP_MoveToAfterNulThat()
        {
            // We were for PRENthe NNplan NULthat VBwas ADVwell PASTstructured CShowever long BKP.
            _shufflerPhraseRepository
                = new ShufflerPhraseRepository(new ShufflerDataAccess());

            _document = _shufflerPhraseRepository.GetShufflerDocument(2016);

            var clauserUnitStrategy =
                new ClauserUnitStrategy();

            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                _document.Paragraphs[1].Sentences[0]);

            // We were for PRENthe NNplan NULthat CShowever long, VBwas ADVwell PASTstructured BKP.
            Assert.That(sentenceReturned.Texts.Count == 13);
            Assert.That(sentenceReturned.Texts[0].pe_text, Is.EqualTo(" We "));
            Assert.That(sentenceReturned.Texts[1].pe_text, Is.EqualTo(" were "));
            Assert.That(sentenceReturned.Texts[2].pe_text, Is.EqualTo(" for "));
            Assert.That(sentenceReturned.Texts[3].pe_text, Is.EqualTo(" the "));
            Assert.That(sentenceReturned.Texts[4].pe_text, Is.EqualTo(" plan "));
            Assert.That(sentenceReturned.Texts[5].pe_text, Is.EqualTo(" that "));
            Assert.That(sentenceReturned.Texts[6].pe_text, Is.EqualTo(" however "));
            Assert.That(sentenceReturned.Texts[7].pe_text, Is.EqualTo(" long "));
            Assert.That(sentenceReturned.Texts[8].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[9].pe_text, Is.EqualTo(" was "));
            Assert.That(sentenceReturned.Texts[10].pe_text, Is.EqualTo(" well "));
            Assert.That(sentenceReturned.Texts[11].pe_text, Is.EqualTo(" structured "));
            Assert.That(sentenceReturned.Texts[12].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void When_Clauser_WithoutNBKP_WithoutNulThat_ShuffleClauserAndRestOfSentence_To_BeginningOfSentence()
        {
            //The meeting was over CSbefore he had a chance lai speak zhiqian BKP.
            _shufflerPhraseRepository
                = new ShufflerPhraseRepository(new ShufflerDataAccess());

            _document = _shufflerPhraseRepository.GetShufflerDocument(2016);

            var clauserUnitStrategy =
                new ClauserUnitStrategy();

            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                _document.Paragraphs[2].Sentences[0]);

            //CSbefore he had a chance lai speak zhiqian NBKP, The meeting was over 
            Assert.That(sentenceReturned.Texts[0].pe_text, Is.EqualTo(" before "));
            Assert.That(sentenceReturned.Texts[1].pe_text, Is.EqualTo(" he "));
            Assert.That(sentenceReturned.Texts[2].pe_text, Is.EqualTo(" had "));
            Assert.That(sentenceReturned.Texts[3].pe_text, Is.EqualTo(" a "));
            Assert.That(sentenceReturned.Texts[4].pe_text, Is.EqualTo(" chance "));
            Assert.That(sentenceReturned.Texts[5].pe_text, Is.EqualTo("lai"));
            Assert.That(sentenceReturned.Texts[6].pe_text, Is.EqualTo(" speak "));
            Assert.That(sentenceReturned.Texts[7].pe_text, Is.EqualTo(" zhiqian "));
            Assert.That(sentenceReturned.Texts[8].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[9].pe_text, Is.EqualTo(" The "));
            Assert.That(sentenceReturned.Texts[10].pe_text, Is.EqualTo(" meeting "));
            Assert.That(sentenceReturned.Texts[11].pe_text, Is.EqualTo(" was "));
            Assert.That(sentenceReturned.Texts[12].pe_text, Is.EqualTo(" over "));
            Assert.That(sentenceReturned.Texts[13].pe_text, Is.EqualTo(" . "));
        }

        [Test]
        public void WhenClauser_WithNBKP_WithNulThat_MoveToAfterNulThat()
        {
            //The meeting that was over CSbefore he had a chance lai speak zhiqian, I think BKP.
            _shufflerPhraseRepository
                = new ShufflerPhraseRepository(new ShufflerDataAccess());

            _document = _shufflerPhraseRepository.GetShufflerDocument(2016);

            var clauserUnitStrategy =
                new ClauserUnitStrategy();

            var sentence = _document.Paragraphs[2].Sentences[0];
            sentence.Texts.Insert(2, new Text
            {
                pe_text = " that ",
                pe_tag_revised = "NUL"
            });
            sentence.Texts[13].pe_tag_revised = "NBKP";
            sentence.Texts[13].pe_text = " , ";
            sentence.Texts.Add(new Text{pe_text = " I " });
            sentence.Texts.Add(new Text { pe_text = " think " });
            sentence.Texts.Add(new Text {pe_text = " . ", pe_tag="BKP"});

            // act
            var sentenceReturned = clauserUnitStrategy.ShuffleSentence(
                sentence);

            //The meeting that CSbefore he had a chance lai speak zhiqian, was over I think BKP.
            Assert.That(sentenceReturned.Texts[0].pe_text, Is.EqualTo(" The "));
            Assert.That(sentenceReturned.Texts[1].pe_text, Is.EqualTo(" meeting "));
            Assert.That(sentenceReturned.Texts[2].pe_text, Is.EqualTo(" that "));
            Assert.That(sentenceReturned.Texts[3].pe_text, Is.EqualTo(" before "));
            Assert.That(sentenceReturned.Texts[4].pe_text, Is.EqualTo(" he "));
            Assert.That(sentenceReturned.Texts[5].pe_text, Is.EqualTo(" had "));
            Assert.That(sentenceReturned.Texts[6].pe_text, Is.EqualTo(" a "));
            Assert.That(sentenceReturned.Texts[7].pe_text, Is.EqualTo(" chance "));
            Assert.That(sentenceReturned.Texts[8].pe_text, Is.EqualTo("lai"));
            Assert.That(sentenceReturned.Texts[9].pe_text, Is.EqualTo(" speak "));
            Assert.That(sentenceReturned.Texts[10].pe_text, Is.EqualTo(" zhiqian "));
            Assert.That(sentenceReturned.Texts[11].pe_text, Is.EqualTo(" , "));
            Assert.That(sentenceReturned.Texts[12].pe_text, Is.EqualTo(" was "));
            Assert.That(sentenceReturned.Texts[13].pe_text, Is.EqualTo(" over "));
            Assert.That(sentenceReturned.Texts[14].pe_text, Is.EqualTo(" I "));
            Assert.That(sentenceReturned.Texts[15].pe_text, Is.EqualTo(" think "));
            Assert.That(sentenceReturned.Texts[16].pe_text, Is.EqualTo(" . "));
            
        }

        [TearDown]
        public void TearDown()
        {
            _document = null;
            _shufflerPhraseRepository = null;
        }
    }
}
