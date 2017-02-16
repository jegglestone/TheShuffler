using ShufflerLibrary.Model;

namespace ShufflerLibrary.Decorator
{
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
    }
}
