using System;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class PlayerInput : MonoBehaviour, IInputProvider
	{
		public event Action<SnakeModel.SnakeDirection> OnDirectionChanged = delegate { };

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				OnDirectionChanged(SnakeModel.SnakeDirection.RIGHT);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				OnDirectionChanged(SnakeModel.SnakeDirection.UP);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				OnDirectionChanged(SnakeModel.SnakeDirection.LEFT);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				OnDirectionChanged(SnakeModel.SnakeDirection.DOWN);
			}
		}
	}
}