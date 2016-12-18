using System;
using System.Collections.Generic;

namespace ShufflerLibrary.Model
{
    public class Paragraph
    {
        public int pe_para_no { get; set; }

        public List<Sentence> Sentences { get; set; }
    }
}
