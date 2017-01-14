using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShufflerLibrary.Strategy
{
    using Decorator;
    using Model;

    public class MDUnitStrategy : IStrategy
    {
        private readonly MDSentenceDecorator _mdSentenceDecorator;

        public MDUnitStrategy()
        {
            _mdSentenceDecorator=new MDSentenceDecorator();
        }

        public Sentence ShuffleSentence(Sentence sentence)
        {
            throw new NotImplementedException();
        }
    }
}
