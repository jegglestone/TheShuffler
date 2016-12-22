using System.Collections.Generic;

namespace ShufflerLibrary.Model
{
    public class Paragraph : PhraseElement
    {
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
    }
}
