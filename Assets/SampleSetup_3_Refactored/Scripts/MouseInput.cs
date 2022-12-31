using System;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class MouseInput : MonoBehaviour
	{
		private GameManager gameManager;

		private Vector3 mouseDownPosition;

		private void Awake()
		{
			gameManager = GameManager.instance;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				mouseDownPosition = Input.mousePosition;
			}

			if (Input.GetMouseButtonUp(0))
			{
				handleMouseUp(mouseDownPosition - Input.mousePosition);
			}
		}

		private void handleMouseUp(Vector3 mouseDelta)
		{
			if (mouseDelta.magnitude < 50)
			{
				startOrRestart();
			} 
			else if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
			{
				gameManager.SetNewDirection(mouseDelta.x < 0 ? SnakeModel.SnakeDirection.RIGHT : SnakeModel.SnakeDirection.LEFT);
			}
			else
			{
				gameManager.SetNewDirection(mouseDelta.y < 0 ? SnakeModel.SnakeDirection.UP : SnakeModel.SnakeDirection.DOWN);
			}
		}

		private void startOrRestart()
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


