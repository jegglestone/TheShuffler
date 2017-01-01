using System.Collections.Generic;

namespace ShufflerLibrary.Model
{
    public class Sentence : PhraseElement
    {
        public List<Text> Texts { get; set; }

        public int TextCount => Texts.Count;
    }

    public class ClauserSentence : Sentence
    {
        
    }
}
