namespace Main.Model
{
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class Sentence
    {
        private Paragraph _sentenceElement;

        public Text[] SentenceArray {
            get
            {
               return _sentenceElement.Descendants<Text>().ToArray();
            }
        }

        public Sentence(Paragraph sentenceElement)
        {
            _sentenceElement = sentenceElement;
        }


    }
}
