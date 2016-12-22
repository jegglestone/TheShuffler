namespace ShufflerLibrary.Strategies
{
    using Model;

    public interface IStrategy
    {
        Sentence ShuffleSentence(Sentence sentence);
    }
}
