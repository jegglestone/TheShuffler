namespace ShufflerLibrary.Decorator
{
    using Model;
    using System.Collections.Generic;

    public class SentenceDecorator
    {
        protected Sentence Sentence { get; set; }

        public List<Text> Texts
        {
            get { return Sentence.Texts; }
            set { Sentence.Texts = value; }
        }

        public Text SentenceBreaker => Sentence.SentenceBreaker;

        public int Pe_para_no => Sentence.pe_para_no;

        public int TextCount => Sentence.TextCount;

        public bool HasVBVBAPAST => Sentence.HasVBVBAPAST;

        public bool HasDG => Sentence.HasDG;
    }
}
