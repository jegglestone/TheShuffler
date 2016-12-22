namespace ShufflerLibrary.IntegrationTests
{
    using System.Runtime.InteropServices;
    using DataAccess;
    using NUnit.Framework;
    using Repository;
    using Rhino.Mocks;

    [TestFixture]
    public class ShufflerPhraseRepository
    {
        public void PassesSentencesToClauserStrategy()
        {


        var mockDataAccess = MockRepository.GenerateMock<IDataAccess>();
        ShufflerPhraseRepository shufflerPhraseRepository
            = new ShufflerPhraseRepository(
                mockDataAccess);

        }
    }
}
