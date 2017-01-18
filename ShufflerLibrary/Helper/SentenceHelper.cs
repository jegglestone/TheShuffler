namespace ShufflerLibrary.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class SentenceHelper
    {
        public static List<Sentence> SplitSentenceOptions(Sentence sentence)
        {
            var sentences = new List<Sentence>();
            int sentenceStartPosition = 0;

            int sentenceCount = sentence.Texts.Count(text => text.IsSentenceEnd);

            for (int i = 0; i < sentence.TextCount; i++)
            {
                if (sentence.Texts[i].actual_text_used.Replace(" ", "") == "."
                    && sentence.Texts[i].actual_tag_used == "BKP")
                {
                    var newSentence = new Sentence();
                    
                    if(sentences.Count == 0)  // first sentence
                    {
                        newSentence.Texts.AddRange(
                        sentence.Texts.GetRange(
                            sentenceStartPosition, i + 1));
                        sentences.Add(newSentence);

                        sentenceStartPosition = i + 1;
                        AssignSentenceProperties(sentence, newSentence);
                    }
                    else if (sentences.Count < sentenceCount) // middle sentences
                    {
                        newSentence.Texts.AddRange(
                        sentence.Texts.GetRange(
                            sentenceStartPosition, i + 1 - sentenceStartPosition));
                        sentences.Add(newSentence);

                        sentenceStartPosition = i + 1;
                        AssignSentenceProperties(sentence, newSentence);
                    }
                    //else if (sentences.Count == sentenceCount - 1) // last sentence - ever called?
                    //{
                    //    newSentence.Texts.AddRange(
                    //    sentence.Texts.GetRange(
                    //        sentenceStartPosition, i + 1 - sentenceStartPosition));
                    //    sentences.Add(newSentence);
                    //    AssignSentenceProperties(sentence, newSentence);
                    //}
                    
                }
            }

            return sentences;
        }

        private static void AssignSentenceProperties(Sentence sentence, Sentence newSentence)
        {
            newSentence.pe_para_no = sentence.pe_para_no;
            newSentence.Sentence_No = sentence.Sentence_No;
            newSentence.SentenceHasMultipleOptions = sentence.SentenceHasMultipleOptions;
        }
    }
}