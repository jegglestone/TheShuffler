using System;
using System.Linq;
using ShufflerLibrary.Model;

namespace ShufflerLibrary.Strategy
{
    public class DdlUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsPyYo))
                return sentence;

            if (PyYoFollowedByNnAndMd1(sentence) || PyYoFollowedByNnAndMd1AndMdbk(sentence))
            {
                //1.1 & 1.3
                MoveMdAfterPyYo(sentence);
            }
            else if (PyYoFollowedByNnAndMdbkAndMd1(sentence))
            {
                //1.4
                MoveMdAfterMdbk(sentence);

                return sentence;
            }
            else if(PyYoFollowedByNnAndMdbk(sentence))
            {
                //1.2
                return sentence;
            }

            return sentence;
        }

        private bool PyYoFollowedByNnAndMd1AndMdbk(Sentence sentence)
        {
            throw new NotImplementedException();
        }

        private void MoveMdAfterMdbk(Sentence sentence)
        {
            // PyYo Nn Mdbk Md1
            int mdbkPosition = GetPyYoPosition(sentence) + 2; // assumption here
            int modifierPosition =
                            sentence.Texts.Skip(mdbkPosition)
                                            .ToList()
                                            .FindIndex(text => text.IsModifier)
                                            + mdbkPosition;
            int modifierEndPosition = 
                sentence.Texts[modifierPosition].pe_merge_ahead + 1;

            var modifierUnit = sentence.Texts.GetRange(
                modifierPosition,
                modifierEndPosition);

            sentence.Texts.RemoveRange(modifierPosition, modifierEndPosition);

            sentence.Texts.InsertRange(mdbkPosition + 1, modifierUnit);
        }

        private static void MoveMdAfterPyYo(Sentence sentence)
        {
            int modifierPosition = sentence.Texts.FindIndex(text => text.IsPyYo) + 2;

            int modifierEndPosition = sentence.Texts[modifierPosition].pe_merge_ahead + 1;

            var modifier = sentence.Texts.GetRange(modifierPosition, modifierEndPosition);

            sentence.Texts.RemoveRange(modifierPosition, modifierEndPosition);

            sentence.Texts.InsertRange(GetPyYoPosition(sentence) + 1, modifier);
        }

        private bool PyYoFollowedByNnAndMd1(Sentence sentence)
        {
            int pyYoPosition = GetPyYoPosition(sentence);

            if (sentence.Texts[pyYoPosition + 1].IsNN
                && sentence.Texts[pyYoPosition + 2].IsModifier)
            {
                return true;
            }

            return false;
        }

        private bool PyYoFollowedByNnAndMdbk(Sentence sentence)
        {
            int pyYoPosition = GetPyYoPosition(sentence);

            if (sentence.Texts[pyYoPosition + 1].IsNN
                && sentence.Texts[pyYoPosition + 2].IsMDBK())
            {
                return true;
            }
            return false;
        }
        private bool PyYoFollowedByNnAndMdbkAndMd1(Sentence sentence)  //PYyo +NN + MDBK + MD1
        {
            int pyYoPosition = GetPyYoPosition(sentence);

            if (sentence.Texts[pyYoPosition + 1].IsNN
                && sentence.Texts[pyYoPosition + 2].IsMDBK()
                && sentence.Texts.Skip(pyYoPosition + 2).Any(text => text.IsModifier))
            {
                return true;
            }
            return false;
        }

        private static int GetPyYoPosition(Sentence sentence)
        {
            return sentence.Texts.FindIndex(text => text.IsPyYo);
        }
    }
}
