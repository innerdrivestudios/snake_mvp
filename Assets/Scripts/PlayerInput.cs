using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GetComponent<GameManager>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			_gameManager.SetDirection(GameManager.SnakeDirection.RIGHT);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_gameManager.SetDirection(GameManager.SnakeDirection.UP);
		} 
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_gameManager.SetDirection(GameManager.SnakeDirection.LEFT);
		} 
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_gameManager.SetDirection(GameManager.SnakeDirection.DOWN);
		}
	}
}
