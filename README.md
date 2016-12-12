# The Shuffler

The Main form plugs into the DocumentFormatter.cs service. This seperates the sentences and passes them through five main shuffling algorithms in sequence found in ClauserUnitStrategy.cs, AdverbStrategy.cs, TimerUnitStrategy.cs, ModifierStrategy.cs and PrenStrategy.cs with the lower level logic primarily abstracted to model/Sentence.cs.



