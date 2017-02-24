namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Model;

    public class CommaUnitStrategy :IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (sentence.Texts.Count(t => t.IsComma) > 1)
            {
                int firstComma = sentence.Texts.FindIndex(text => text.IsComma);

                for (int i = firstComma+1; i < sentence.TextCount; i++)
                {
                    if (sentence.Texts[i].IsComma)
                        sentence.Texts.RemoveAt(i);
                }
            }

            return sentence;
        }
    }
}
