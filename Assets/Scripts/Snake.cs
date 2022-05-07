using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    /*
    public SnakeField snakeField;

    private Vector3 _currentDirection;
    private Vector3 _newDirection;

    public float blockUpdateDelay;
    private WaitForSeconds _snakeUpdateDelayYI;
    private bool gameOver = false;

    private List<GameObject> _spawns = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        _currentDirection = transform.GetChild(0).localPosition - transform.GetChild(1).position;
        _newDirection = _currentDirection;

        _snakeUpdateDelayYI = new WaitForSeconds(blockUpdateDelay);
        StartCoroutine(snakeUpdate());
    }

    private void Update()
    {
        if (gameOver) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _newDirection = Vector3.up;
        } 
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _newDirection = Vector3.down;
        } 
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _newDirection = Vector3.left;
        } 
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _newDirection = Vector3.right;
        }

        if (Vector3.Dot (_currentDirection, _newDirection) > -0.5f)
        {
            transform.GetChild(0).localPosition = transform.GetChild(1).localPosition + _newDirection;
        } else
        {
            _newDirection = _currentDirection;
        }
    }

    private IEnumerator snakeUpdate()
    {
        while (true)
        {
            yield return _snakeUpdateDelayYI;
            
            if (moveSnake())
            {
                checkSpawn();
            } else
            {
                Debug.Log("Game over");
                gameOver = true;
                yield break;
            }
        }
    }

    private bool moveSnake()
    {
        Vector3 newHeadPosition = transform.GetChild(0).localPosition;

        //went outside?
        if (!snakeField.IsInside(newHeadPosition)) return false;

        //bit own tail?
        GameObject contents = snakeField.GetContents(newHeadPosition);
        if (contents != null && contents.transform.parent == transform) { 
                return false;
        }

        //in order to move we first clear the tail position so we don't leave anything behind
        Vector3 lastTailPosition = transform.GetChild(transform.childCount - 1).position;
        snakeField.Clear(transform.GetChild(transform.childCount - 1).gameObject);

        //then we move all elements of the snake, overwriting old elements the array
        for (int i = transform.childCount-1; i > 0; i--)
        {
            transform.GetChild(i).localPosition = transform.GetChild(i - 1).localPosition;
            snakeField.Store(transform.GetChild(i).gameObject);
        }

        //then we move our indicator by one and store the new direction as the currentdirection
        //note that until we overwrite newDirection, newDirection is also still current direction
        transform.GetChild(0).localPosition += _newDirection;
        _currentDirection = _newDirection;

        //if we ate something else then our own tail
        if (contents != null)
        {
            Destroy(contents);
            _spawns.Remove(contents);
            //spawn new tail
            GameObject tail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tail.transform.localPosition = lastTailPosition;
            //tail.transform.localScale = Vector3.one * (1 + 0.1f * Mathf.Sin(-0.5f*Mathf.PI +  (transform.childCount-2) * Mathf.PI * 2 * 0.3f));
            tail.transform.parent = transform;
        }

        return true;
    }

    private void checkSpawn()
    {
        while (_spawns.Count < 3)
        {
            Vector3 randomWorldPosition = snakeField.GetRandomWorldPosition();
            if (snakeField.GetContents(randomWorldPosition) == null)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = randomWorldPosition;
                _spawns.Add(sphere);
                snakeField.Store(sphere);
            }
        }
    }
    */
}
