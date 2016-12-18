using System.Collections.Generic;

namespace ShufflerLibrary.Model
{
    public class Document
    {
        public int pe_pmd_id { get; set; }

        public List<Paragraph> Paragraphs { get; set; } 
    }
}
