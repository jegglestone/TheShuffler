using ShufflerLibrary.Model;
using System.Text;

namespace ShufflerLibrary.Helper
{
    public static class ShuffledStateHelper
    {
        public static void AddShuffledState(Sentence sentence, string ruleApplied)
        {
            StringBuilder sentenceLineStringBuilder = new StringBuilder();
            foreach (var text in sentence.Texts)
            {
                sentenceLineStringBuilder.Append(text.actual_tag_used + text.actual_text_used + " ");
            }

            sentence.ShuffledStates.Add(new ShuffledState()
            {
                SentenceIdentifier = sentence.Sentence_Identifier,
                SentenceState = sentenceLineStringBuilder.ToString(),
                StrategyApplied = ruleApplied
            });
        }
    }
}
