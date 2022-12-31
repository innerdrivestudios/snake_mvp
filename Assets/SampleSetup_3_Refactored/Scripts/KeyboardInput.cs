using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class KeyboardInput : MonoBehaviour
	{
		private GameManager gameManager;

		private void Awake()
		{
			gameManager = GameManager.instance;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				gameManager.SetNewDirection(SnakeModel.SnakeDirection.RIGHT);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				gameManager.SetNewDirection(SnakeModel.SnakeDirection.UP);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				gameManager.SetNewDirection(SnakeModel.SnakeDirection.LEFT);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				gameManager.SetNewDirection(SnakeModel.SnakeDirection.DOWN);
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (gameManager.gameState == GameManager.GameState.WAITING_TO_START)
				{
					gameManager.StartGame();
				}
				else if (gameManager.gameState == GameManager.GameState.GAME_OVER)
				{
					gameManager.RestartGame();
				}
			}
		}


	}
}