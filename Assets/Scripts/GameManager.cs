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

    [Header ("Playing field settings")]
    //Define the playing field size
	[SerializeField] private int _width;
    [SerializeField] private int _height;

    [Header("Game play settings")]
    [Range(0.1f, 1)]
    [SerializeField] private float _stepDelay = 1;
    [SerializeField] private float _boostFactor = 2;
    [SerializeField] private float _powerUpSpawnRate = 5;

    private float _lastSpawnTime = 0;
    private float _currentStepDelay = 0;

    [Header("Debug settings")]
    [SerializeField] bool _renderGizmos = true;
    [SerializeField] bool _renderDirectionPreview = true;

    //Helper grid for spatial partitioning so that we can get quickly detect pickups or collisions
    private SnakeField _snakeField;     
    //Defines all the parts of the snake
    private SnakeModel _snakeModel;

    //In snake there is a difference between the direction we are moving in (lastMovedDirection)
    //and the direction we want to move in (inputDirection)
    //Which inputDirection is allowed depends on the lastMovedDirection
    private SnakeDirection _inputDirection;
    private SnakeDirection _lastMovedDirection;

    private bool _directionSet = false; //has any direction been set at all (by default this remains the same if input is not provided)
    private bool _snakeMoving = false;  //becomes true once a direction has been set and we've moved at least once...

    private void Awake()
    {
        _snakeField = new SnakeField(_width, _height);
        _snakeModel = new SnakeModel(_snakeField);

        Vector2Int headPosition = new Vector2Int(_width >> 1, _height >> 1);

        for (int i = 0; i < 5; i++)
		{
            _snakeModel.AddPart(headPosition);
            headPosition += _snakeDirections[(int)SnakeDirection.DOWN];
		}

        _currentStepDelay = _stepDelay;
    }

	private IEnumerator Start()
	{
		while (Application.isPlaying)
		{
            yield return new WaitForSeconds(_currentStepDelay);

            //as long as no direction has been set we skip the game loop
            if (!_directionSet) continue;

            //check if we can move to the new position or if we are going to hit an invalid position
            Vector2Int direction = _snakeDirections[(int)_inputDirection];

            //check if the new position is a valid position, do this before we move otherwise the snake will occupy the 
            //newHeadPosition and the valid check will always return false
            bool validPosition = _snakeModel.IsNextHeadPositionValid(direction);
            //Check if there is a pickup at the location we are moving to
            GameObject pickup = _snakeField.GetContents(_snakeModel.GetNextHeadPositionFor(direction)) as GameObject;

            //Move whether we are valid or not (we want to see the accident happen :)) and grow if we ate a pickup
            _snakeModel.Move(direction, pickup != null);
            //store the fact that we actually moved in this direction, so new input directions might become valid
            _lastMovedDirection = _inputDirection;
            //mark the snake as moving after the first time we've moved for real (and keep setting it for no reason after that)
            _snakeMoving = true;

            if (pickup != null)
			{
                Destroy(pickup);
                _stepDelay *= 0.99f;
			}

            if (!validPosition)
			{
                Debug.Log("Game over");
                break;
			}
		}

        Debug.Log("Game loop ended");
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

	private void Update()
	{
        checkBoost();

		if (_snakeMoving && Time.time > _lastSpawnTime + _powerUpSpawnRate)
		{
            Vector2Int randomPosition = new Vector2Int(Random.Range(0, _width), Random.Range(0, _height));
            object gridContents = _snakeField.GetContents(randomPosition);

            //if the grid position is free, spawn a powerup there
            //otherwise don't do anything, we'll run the same code next frame, that way spawning is sliced over frame and
            //we'll never run into hiccups due to "while (not free) do random" loops
            if (gridContents == null)
			{
                GameObject powerUpStub = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                powerUpStub.transform.position = vector2IntToVector3(randomPosition);
                powerUpStub.name = "TastyAppleEatMe";

                _snakeField.Store(randomPosition, powerUpStub);
                _lastSpawnTime = Time.time;
			}
		}
	}

	private void checkBoost()
   	{
        bool shiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        _currentStepDelay = _stepDelay / (shiftKeyDown ? _boostFactor : 1);
    }

    private void OnDrawGizmos()
    {
        if (_snakeField == null) return;
        if (!_renderGizmos) return;

        Vector3 worldPosition;

        //draw whole grid
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                Gizmos.color = (_snakeField.GetContents(gridPosition) == null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                worldPosition = vector2IntToVector3(gridPosition);
                Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
            }
        }

        if (_snakeModel == null) return;

        LinkedListNode<Vector2Int> snakeElement = _snakeModel.FirstPart;

        if (_renderDirectionPreview)
        {
            //draw where we want to go
            Gizmos.color = Color.cyan;
            Vector2Int inputDirection = _snakeDirections[(int)_inputDirection];
            worldPosition = vector2IntToVector3(snakeElement.Value + inputDirection);
            Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));

            //draw where we are headed
            Gizmos.color = Color.magenta;
            Vector2Int lastMovedDirection = _snakeDirections[(int)_lastMovedDirection];
            worldPosition = vector2IntToVector3(snakeElement.Value + lastMovedDirection);
            Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
        }

        //draw snake body
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        while (snakeElement != null) {
            worldPosition = vector2IntToVector3(snakeElement.Value);
            Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
            snakeElement = snakeElement.Next;
        }
    }

    private Vector3 vector2IntToVector3 (Vector2Int gridPosition)
	{
        return new Vector3(gridPosition.x, gridPosition.y, 0) + transform.position;
    }

}
