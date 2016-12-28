namespace ShufflerLibrary.Strategy
{
    using Model;

    public interface IStrategy
    {
        Sentence ShuffleSentence(Sentence sentence);
    }
}
