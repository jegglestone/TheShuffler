using System.Linq;

namespace ShufflerLibrary.Strategy
{
    using Model;

    public class MdNulThatUnitStrategy : IStrategy
    {
        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsMdNulThat))
                return sentence;

            int mdNulThatPosition = 
                sentence.Texts.FindIndex(text => text.IsMdNulThat);

            int prenPosition = 
                sentence.Texts.Take(mdNulThatPosition).ToList().FindIndex(text => text.IsPren);

            int dePositionFromMdPosition= 
                sentence.Texts.Skip(mdNulThatPosition).ToList()
                .FindIndex(text => text.IsDe()) + 1;
            
            var mdUnit = sentence.Texts.GetRange(
                mdNulThatPosition, dePositionFromMdPosition);

            sentence.Texts.RemoveRange(
                mdNulThatPosition, dePositionFromMdPosition);

            sentence.Texts.InsertRange(prenPosition+1, mdUnit);  
          
            return sentence;
        }
    }
}
