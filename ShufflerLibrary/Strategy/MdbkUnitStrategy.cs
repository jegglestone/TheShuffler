using System.Linq;
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

            var mdbkSentenceDecorator = new MdbkSentenceDecorator(sentence);

            var mdbkUnitUpToVbVbaPastPresBkp =
                mdbkSentenceDecorator.MdbkUnitUpToVbVbaPastPresBkp();

            if (mdbkUnitUpToVbVbaPastPresBkp.Any(text => text.IsModifier))
            {
                ApplyMdShufflingRules(
                    sentence, mdbkSentenceDecorator.MdbkPosition, mdbkSentenceDecorator);
            }
            else
            {
                if (mdbkUnitUpToVbVbaPastPresBkp.Any(text => text.IsNN))
                {
                    int lastNnPosition = 
                        mdbkSentenceDecorator.LastNNPositionAfter(mdbkUnitUpToVbVbaPastPresBkp);
                        
                    InsertDeParticleAtPosition(sentence, lastNnPosition + 1);

                    UnderlineMdbkToDe(sentence, mdbkSentenceDecorator.MdbkPosition);
                }
            }

            if (mdbkSentenceDecorator.NnUnitBeforeMdbk(
                sentence, mdbkSentenceDecorator.MdbkPosition))
            {
                int nnPosition =
                    mdbkSentenceDecorator.NNPositionBeforeMdbk();

                if (mdbkSentenceDecorator.ByBeforeNN(sentence, nnPosition))
                {
                    return sentence;
                }

                if (mdbkSentenceDecorator.PrenBeforeNN(sentence, nnPosition))
                {
                    MoveMdbkbeforePren(
                        sentence, nnPosition, mdbkSentenceDecorator.MdbkPosition, mdbkSentenceDecorator);
                }
                else if (mdbkSentenceDecorator.AdjBeforeNN(sentence, nnPosition))
                {
                    MoveMdbkBeforeAdj(
                        sentence, nnPosition, mdbkSentenceDecorator.MdbkPosition, mdbkSentenceDecorator);
                }
                else
                {
                    MoveMdbkBeforeUnit(
                        sentence, mdbkSentenceDecorator.MdbkPosition, nnPosition);                    
                }
            }

            return sentence;
        }

        private void MoveMdbkBeforeAdj(
            Sentence sentence, int nnPosition, int mdbkPosition, MdbkSentenceDecorator mdbkSentenceDecorator)
        {
            //2.1.2.2.If PREN is not found, search for an ADJ unit.If found, move MDBK to before ADJ.
            int adjPosition =
                mdbkSentenceDecorator.GetTextsUpToNN(sentence, nnPosition).ToList().FindLastIndex(
                    text => text.IsType(UnitTypes.ADJ_Adjective));

            MoveMdbkBeforeUnit(sentence, mdbkPosition, adjPosition);
        }

        private static void MoveMdbkbeforePren(
            Sentence sentence, int nnPosition, int mdbkPosition, MdbkSentenceDecorator mdbkSentenceDecorator)
        {
            // 2.1.2.1.	If PREN is found, move MDBK to before PREN
            int prenPosition =
                mdbkSentenceDecorator.GetTextsUpToNN(sentence, nnPosition).ToList().FindLastIndex(
                    text => text.IsPren);

            MoveMdbkBeforeUnit(sentence, mdbkPosition, prenPosition);
        }

        private static void MoveMdbkBeforeUnit(
            Sentence sentence, int mdbkPosition, int newPosition)
        {
            var mdbkUnit = sentence.Texts.GetRange(
                mdbkPosition,
                sentence.Texts[mdbkPosition].pe_merge_ahead + 1);

            sentence.Texts.RemoveRange(
                mdbkPosition,
                sentence.Texts[mdbkPosition].pe_merge_ahead + 1);

            sentence.Texts.InsertRange(
                newPosition,
                mdbkUnit);
        }

        private static void ApplyMdShufflingRules(
            Sentence sentence, int mdbkPosition, MdbkSentenceDecorator mdbkSentenceDecorator)
        {
            // 1.1.1
            int modifierPosition =
                mdbkSentenceDecorator.ModifierPositionAfterMdbk;

            InsertDeParticleAtPosition(
              sentence, modifierPosition);

            modifierPosition++;

            var mdSentenceDecorator =
                    new MdSentenceDecorator(sentence);

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

        private static void MoveMdUnitAfterMdbk(
            Sentence sentence, MdSentenceDecorator mdSentenceDecorator, int modifierPosition,
          int mdbkPosition)
        {
            var mdUnit =
                    mdSentenceDecorator.GetModifierUnitUpToVbPastPresBkp(modifierPosition);

            sentence.Texts.RemoveRange(modifierPosition, mdUnit.Count);

            sentence.Texts.InsertRange(
                mdbkPosition + 1 + sentence.Texts[mdbkPosition].pe_merge_ahead,
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
    }
}
