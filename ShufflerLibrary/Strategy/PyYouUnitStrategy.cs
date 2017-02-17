using System.Collections.Generic;
using System.Linq;
using ShufflerLibrary.Decorator;
using ShufflerLibrary.Model;

namespace ShufflerLibrary.Strategy
{
    public class PyYoUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.HasPyYo) return sentence;

            var pyYoSentenceDecorator = new PyYoSentenceDecorator(sentence);
       
            if (PyYoPreceededByPyJinxindeThenPrenOrNn(sentence, pyYoSentenceDecorator))
            {
                ShuffleJinxingdePrenNnUnit(sentence, pyYoSentenceDecorator);
            }
            else if (PyYoPreceededByPydeNnpPastPyDe(sentence, pyYoSentenceDecorator))
            {
                ShufflePydeNnpPastPyDeUnit(sentence, pyYoSentenceDecorator);
            }

            return sentence;
        }

        private static void ShuffleJinxingdePrenNnUnit(
            Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            if (pyYoSentenceDecorator.UnitAfterPyYo.Any(text => text.IsMDBK()))
            {
                //1.2.
                MovePyJinxingdePreNNAroundMdbk(
                    sentence, pyYoSentenceDecorator);
            }
            else
            {
                //1.1.
                MovePyJinxingdePrenNnAfterPyYo(
                    sentence, pyYoSentenceDecorator);
            }
        }

        private void ShufflePydeNnpPastPyDeUnit(
            Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            if (pyYoSentenceDecorator.UnitAfterPyYo.Any(text => text.IsMDBK()))
            {
                //2.2.
                MovePastDePyDeNnpAroundYoAndMdbk(sentence, pyYoSentenceDecorator);
            }
            else
            {
                //2.1.  
                MovePastDePyDeNnpAfterYo(sentence, pyYoSentenceDecorator);
            }
        }

        private static void MovePastDePyDeNnpAroundYoAndMdbk(
            Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            var pastPyDePyDeNnp = sentence.Texts.GetRange(
                pyYoSentenceDecorator.PyYoPosition - 4, 4);

            sentence.Texts.RemoveRange(
                pyYoSentenceDecorator.PyYoPosition - 4, 4);

            int yoPeMergeAhead =
                sentence.Texts[pyYoSentenceDecorator.PyYoPosition].pe_merge_ahead;

            sentence.Texts.InsertRange(
                pyYoSentenceDecorator.PyYoPosition + 1 + yoPeMergeAhead,
                pastPyDePyDeNnp.GetRange(2, 2));

            //move PYde  NNP to after the MDBK unit
            int mdbkPosition = 
                pyYoSentenceDecorator.MdbkPositionRelativeToPyYo;

            int mdbkPeMergeAhead = 
                sentence.Texts[mdbkPosition].pe_merge_ahead;

            sentence.Texts.InsertRange(
                mdbkPosition + 1 + mdbkPeMergeAhead, pastPyDePyDeNnp.GetRange(0, 2));
        }

        private static void MovePastDePyDeNnpAfterYo(
            Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            var pastPyDePyDeNnp = sentence.Texts.GetRange(
                pyYoSentenceDecorator.PyYoPosition - 4, 4);

            MovePyDeNNPBeforePastPyDe(pastPyDePyDeNnp);

            sentence.Texts.RemoveRange(
                pyYoSentenceDecorator.PyYoPosition - 4, 4);

            int peMergeAhead =
                sentence.Texts[pyYoSentenceDecorator.PyYoPosition].pe_merge_ahead;

            sentence.Texts.InsertRange(
                pyYoSentenceDecorator.PyYoPosition + 1 + peMergeAhead,
                pastPyDePyDeNnp.GetRange(0, 4));
        }

        private static void MovePyJinxingdePreNNAroundMdbk(
            Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            int jinxingdePosition = pyYoSentenceDecorator.JinxingdePosition;

            var jinxingdePrenNnUnit =
                sentence.Texts.GetRange(jinxingdePosition, 3);

            sentence.Texts.RemoveRange(jinxingdePosition, 3);

            int mdbkPosition = 
                pyYoSentenceDecorator.MdbkPositionRelativeToPyYo + pyYoSentenceDecorator.PyYoPosition;

            sentence.Texts.InsertRange(
                mdbkPosition + 1 + sentence.Texts[mdbkPosition].pe_merge_ahead,
                jinxingdePrenNnUnit.GetRange(1, 2));

            sentence.Texts.Insert(mdbkPosition,
                jinxingdePrenNnUnit[0]);
        }

        private static void MovePyDeNNPBeforePastPyDe(List<Text> pastPyDePyDeNnp)
        {
            pastPyDePyDeNnp.InsertRange(4, pastPyDePyDeNnp.GetRange(0, 2));
            pastPyDePyDeNnp.RemoveRange(0, 2);
        }

        private static void MovePyJinxingdePrenNnAfterPyYo(
            Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            int jinxingdePosition = pyYoSentenceDecorator.JinxingdePosition;

            var jinxingdePrenNnUnit =
                sentence.Texts.GetRange(jinxingdePosition, 3);

            sentence.Texts.RemoveRange(jinxingdePosition, 3);

            sentence.Texts.InsertRange(
                pyYoSentenceDecorator.PyYoPosition + pyYoSentenceDecorator.FirstVbaVbPastPresBkpAfterYo,
                jinxingdePrenNnUnit);
        }

        private static bool PyYoPreceededByPyJinxindeThenPrenOrNn(Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            var textsUpToPyYo = sentence.Texts.Take(pyYoSentenceDecorator.PyYoPosition).ToList();
            if (textsUpToPyYo.Count < 3) return false;

            if (textsUpToPyYo.Last().IsNN 
                && textsUpToPyYo[textsUpToPyYo.Count-2].IsPren
                && textsUpToPyYo[textsUpToPyYo.Count - 3].IsPyJinxingde)
            {
                return true;
            }
            return false;
        }

        private static bool PyYoPreceededByPydeNnpPastPyDe(Sentence sentence, PyYoSentenceDecorator pyYoSentenceDecorator)
        {
            var textsUpToPyYo = sentence.Texts.Take(pyYoSentenceDecorator.PyYoPosition).ToList();
            if (textsUpToPyYo.Count < 4) return false;

            if (textsUpToPyYo.Last().IsDe()
                && textsUpToPyYo[textsUpToPyYo.Count - 2].IsPast
                && textsUpToPyYo[textsUpToPyYo.Count - 3].IsNNP
                && textsUpToPyYo[textsUpToPyYo.Count - 4].IsDe())
            {
                return true;
            }

            return false;
        }
    }
}
