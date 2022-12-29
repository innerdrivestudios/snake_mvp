using System;

namespace SampleSetup_3_Refactored
{
	public interface IInputProvider
	{
		event Action<SnakeModel.SnakeDirection> OnDirectionChanged;
	}
}
