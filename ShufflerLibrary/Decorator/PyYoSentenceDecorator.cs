using ShufflerLibrary.Model;

namespace ShufflerLibrary.Decorator
{
    using System.Collections.Generic;
    using System.Linq;

    public class PyYoSentenceDecorator : SentenceDecorator
    {
        public PyYoSentenceDecorator(Sentence sentence)
        {
            Sentence = sentence;
        }

        public int PyYoPosition
        {
            get
            {
                return Texts.FindIndex(text => text.IsPyYo);
            }
        }

        public int JinxingdePosition
        {
            get
            {
                return Texts.Take(PyYoPosition)
                    .ToList()
                    .FindIndex(text => text.IsPyJinxingde);
            }
        }

        public int FirstVbaVbPastPresBkpAfterYo
        {
            get
            {
                return 
                    Texts
                    .Skip(PyYoPosition)
                    .ToList()
                    .FindIndex(
                        text =>
                            text.IsVbVbaPast || text.IsPres ||
                            text.IsType(UnitTypes.BKP_BreakerPunctuation));                
            }
        }

        public int MdbkPositionRelativeToPyYo
        {
            get
            {
                return 
                    Texts
                    .Skip(PyYoPosition)
                    .ToList()
                    .FindIndex(text => text.IsMDBK());
            }
        }

        public IEnumerable<Text> UnitAfterPyYo => Texts
            .Skip(PyYoPosition)
            .ToList()
            .Take(FirstVbaVbPastPresBkpAfterYo);
    }
}
