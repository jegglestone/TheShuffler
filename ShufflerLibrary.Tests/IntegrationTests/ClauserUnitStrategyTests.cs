using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShufflerLibrary.Tests.IntegrationTests
{
    [TestFixture]
    public class ClauserUnitStrategyTests
    {
        public void WhenClauserAndNBKPMoveToBeginningOfSentence()
        {
            // TMIn April and May NBKP, CShowever NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.

            // CShowever NBKP, TMIn April and May NBKP, PRENthe NNreport VBwasn’t ADJgood BKP.
        }

        public void WhenClauserAndNBKPMoveToAfterNulThat()
        {
            // We were for PRENthe NNplan NULthat VBwas ADVwell PASTstructured CShowever long BKP.

            // We were for PRENthe NNplan NULthat CShowever long VBwas ADVwell PASTstructured BKP.
        }
    }
}
