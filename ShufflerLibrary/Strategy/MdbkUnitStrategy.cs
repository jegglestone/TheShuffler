using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using ShufflerLibrary.Decorator;
using ShufflerLibrary.Helper;
using ShufflerLibrary.Model;

namespace ShufflerLibrary.Strategy
{
  public class MdbkUnitStrategy : IStrategy
  {
    public Sentence ShuffleSentence(Sentence sentence)
    {
      if (!sentence.Texts.Any(text => text.IsMDBK()))
        return sentence;

      int mdbkPosition = sentence.Texts.FindIndex(text => text.IsMDBK());
      var mdbkUnitUpToVbVbaPastPresBkp = MdbkUnitUpToVbVbaPastPresBkp(sentence, mdbkPosition);

      if (mdbkUnitUpToVbVbaPastPresBkp.Any(text => text.IsModifier))
      {
        ApplyMdShufflingRules(sentence, mdbkPosition);
      }
      else
      {
        //TODO: Could be no NN - exxception
        int lastNnPosition = mdbkUnitUpToVbVbaPastPresBkp.FindLastIndex(
          text => text.IsNN) + mdbkPosition;

        InsertDeParticleAtPosition(sentence, lastNnPosition + 1);

        UnderlineMdbkToDe(sentence, mdbkPosition);
      }

      if (NnUnitBeforeMdbk(sentence, mdbkPosition))
      {

      }

      return sentence;
    }

    private static bool NnUnitBeforeMdbk(Sentence sentence, int mdbkPosition)
    {
      return sentence.Texts.Take(mdbkPosition).Any(text => text.IsNN);
    }

    private static void ApplyMdShufflingRules(Sentence sentence, int mdbkPosition)
    {
      // 1.1.1
      int modifierPosition =
              GetModifierPosition(sentence, mdbkPosition);

      InsertDeParticleAtPosition(
        sentence, modifierPosition);

      modifierPosition++;

      var mdSentenceDecorator =
              new MDSentenceDecorator(sentence);

      //*1.1.2.Move the MD unit to after the MDBK unit:
      MoveMdUnitAfterMdbk(
        sentence, mdSentenceDecorator, modifierPosition, mdbkPosition);

      // 1.1.3.Underline MDBK all the way to and including ‘de’ to form one MDBK unit:*/
      UnderlineMdbkToDe(sentence, mdbkPosition);
    }

    private static void UnderlineMdbkToDe(Sentence sentence, int mdbkPosition)
    {
      sentence.Texts[mdbkPosition].pe_merge_ahead =
              sentence
                      .Texts
                      .Skip(mdbkPosition).ToList()
                      .FindIndex(text => text.IsDe());
    }

    private static void MoveMdUnitAfterMdbk(Sentence sentence, MDSentenceDecorator mdSentenceDecorator, int modifierPosition,
      int mdbkPosition)
    {
      var mdUnit =
              mdSentenceDecorator.GetModifierUnitUpToVbPastPresBkp(modifierPosition);

      sentence.Texts.RemoveRange(modifierPosition, mdUnit.Count);

      sentence.Texts.InsertRange(mdbkPosition + 2, // TODO: After MDBK which is 2 words?
        mdUnit);
    }

    private static void InsertDeParticleAtPosition(Sentence sentence, int newPosition)
    {
      sentence.Texts.Insert(
        newPosition,
        DeParticleHelper.CreateNewDeParticle(
          sentence.Texts[newPosition - 1].pe_order,
          0));
    }

    private static int GetModifierPosition(Sentence sentence, int mdbkPosition)
    {
      return sentence.Texts.Skip(mdbkPosition).ToList()
                      .FindIndex(text => text.IsModifier) + mdbkPosition;
    }

    private static List<Text> MdbkUnitUpToVbVbaPastPresBkp(Sentence sentence, int mdbkPosition)
    {
      int mdbkUnitEndPosition =
              sentence.Texts.Skip(mdbkPosition).ToList().FindIndex(
                text => text.IsVbVbaPast
                        || text.IsType(UnitTypes.PRES_Participle)
                        || text.IsType(UnitTypes.BKP_BreakerPunctuation))
              + mdbkPosition;

      var mdbkUnitUpToVbVbaPastPresBkp = new List<Text>();

      for (int i = mdbkPosition; i < mdbkUnitEndPosition; i++)
      {
        mdbkUnitUpToVbVbaPastPresBkp.Add(sentence.Texts[i]);
      }
      return mdbkUnitUpToVbVbaPastPresBkp;
    }
  }
}
