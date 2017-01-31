using System.Linq;
using ShufflerLibrary.Model;

namespace ShufflerLibrary.Strategy
{
  public class PercentUnitStrategy : IStrategy
  {
    public Sentence ShuffleSentence(Sentence sentence)
    {
      if (!sentence.Texts.Any(text => text.IsPercent))
      {
        return sentence;
      }

      int percentPosition = sentence.Texts.FindIndex(text => text.IsPercent);

      if (!UnitBeforeThisIsDIG(sentence, percentPosition))
        return sentence;

      var percentUnit = sentence.Texts.First(text => text.IsPercent);

      sentence.Texts.RemoveAt(percentPosition);

      sentence.Texts.Insert(percentPosition - 1, percentUnit);

      return sentence;
    }

    private static bool UnitBeforeThisIsDIG(Sentence sentence, int percentPosition)
    {
      return sentence.Texts[percentPosition - 1].IsType(UnitTypes.DIG_Digit);
    }
  }
}
