using System;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class PlayerInput : MonoBehaviour, IInputProvider
	{
		public event Action<SnakeModel.SnakeDirection> onDirectionChanged = delegate { };

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				onDirectionChanged(SnakeModel.SnakeDirection.RIGHT);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				onDirectionChanged(SnakeModel.SnakeDirection.UP);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				onDirectionChanged(SnakeModel.SnakeDirection.LEFT);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				onDirectionChanged(SnakeModel.SnakeDirection.DOWN);
			}
		}
	}
}