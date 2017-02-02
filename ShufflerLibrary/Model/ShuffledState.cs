using System;

namespace ShufflerLibrary.Model
{
  public class ShuffledState
  {
    public Guid SentenceIdentifier { get; set; }

    public string SentenceState { get; set; }

    public string StrategyApplied { get; set; }
  }
}
