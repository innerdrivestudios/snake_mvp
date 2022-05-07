using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * GameManager for the Snake game that ties all elements together.
 */
public class GameManager : MonoBehaviour
{
    //Define directions as an enum to make sure only valid values can be passed in from the outside
    public enum SnakeDirection { RIGHT = 0, UP = 1, LEFT = 2, DOWN = 3 };
    //Define a matching direction array that gives us the actual direction vectors matching the directions above
    private Vector2Int[] _snakeDirections = { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

    //Define the playing field size
	[SerializeField] private int _width;
    [SerializeField] private int _height;

    //Helper grid for spatial partitioning so that we can get quickly detect pickups or collisions
    private SnakeField _snakeField;     
    //Defines all the parts of the snake
    private SnakeModel _snakeModel;

    //In snake there is a difference between the direction we are moving in (lastMovedDirection) and the direction we want to move in (inputDirection)
    //Which inputDirections are allowed depends on the lastMovedDirection
    private SnakeDirection _inputDirection;
    private SnakeDirection _lastMovedDirection;

    private bool _directionSet = false; //has any direction been set at all (by default this remains the same if input is not provided)
    private bool _snakeMoving = false;  //becomes true once a direction has been set and we've moved at least once...

    private void Awake()
    {
        _snakeField = new SnakeField(_width, _height);
        
        _snakeModel = new SnakeModel();
        _snakeModel.AddPart(new Vector2Int(_width >> 1, _height >> 1));
    }

	private IEnumerator Start()
	{
		while (Application.isPlaying)
		{
            yield return new WaitForSeconds(1);

            if (_directionSet)
			{
                //We are not moving the snake yet, just implementing the input and direction mechanism
                //_snakeModel.Move(_snakeDirections[(int)_inputDirection]);
                _lastMovedDirection = _inputDirection;
                _snakeMoving = true;
			}
		}
	}

    /**
     * Specifies a direction the user would like to move in. 
     * Whether the direction is accepted depends on whether we are already moving and if so, in which direction we are moving
     */
	public void SetDirection (SnakeDirection newDirection)
	{
        //Input is accepted if we are not moving (since we can still go everywhere) OR if newDirection is valid
        bool newDirectionToLeftFrontOrRightOfCurrent = Vector2.Dot(_snakeDirections[(int)_lastMovedDirection], _snakeDirections[(int)newDirection]) >= 0;

        if (!_snakeMoving || newDirectionToLeftFrontOrRightOfCurrent)
		{
            _inputDirection = newDirection;
            _directionSet = true;
		}
	}

    private void OnDrawGizmos()
    {
        if (_snakeField == null) return;

        Vector3 worldPosition;

        //draw whole grid
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Gizmos.color = (_snakeField.GetContents(new Vector2Int(x, y)) == null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                worldPosition = new Vector3(x, y, 0) + transform.position;
                Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
            }
        }

        if (_snakeModel == null) return;

        LinkedListNode<Vector2Int> snakeElement = _snakeModel.First;

        //draw where we want to go
        Gizmos.color = Color.cyan;
        Vector2Int inputDirection = _snakeDirections[(int)_inputDirection];
        worldPosition = new Vector3(snakeElement.Value.x + inputDirection.x, snakeElement.Value.y + inputDirection.y, 0) + transform.position;
        Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));

        //draw where we are headed
        Gizmos.color = Color.magenta;
        Vector2Int lastMovedDirection = _snakeDirections[(int)_lastMovedDirection];
        worldPosition = new Vector3(snakeElement.Value.x + lastMovedDirection.x, snakeElement.Value.y + lastMovedDirection.y, 0) + transform.position;
        Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));

        //draw snake body
        Gizmos.color = Color.blue;
        while (snakeElement != null) {
            worldPosition = new Vector3(snakeElement.Value.x, snakeElement.Value.y, 0) + transform.position;
            Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
            snakeElement = snakeElement.Next;
        }
    }

    /*
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
            snakeModel.AddPart(snakeField.GetRandomLocation());
		}
	}
    */

}
