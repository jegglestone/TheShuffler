namespace ShufflerLibrary.Decorator
{
    using Model;
    using System.Collections.Generic;

    public class SentenceDecorator
    {
        protected Sentence Sentence { get; set; }

        public List<Text> Texts => Sentence.Texts;

        public Text SentenceBreaker => Sentence.SentenceBreaker;

        public int Pe_para_no => Sentence.pe_para_no;

        public int TextCount => Sentence.TextCount;
    }
}
