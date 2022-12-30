using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SampleSetup_2_Optimized
{
    /**
     * An optimized snake version where we are no longer using colliders, boundaries, rigidbodies etc
     * but use a 2d array as a lookup table instead.
     */
    public class Snake : MonoBehaviour
    {
        //used for initial direction of the snake and possible as an indicator (if it is not disabled)
        public Transform nextPositionIndicator;

        //reference to the snakefield so that we know where we are and where everything else is
        public SnakeField snakeField;

        //prefab to spawn every now and then which will make the snake grow when eaten
        public Transform applePrefab;
        //spawn rate in seconds
        public float appleSpawnDelay;
        //cached wait for second for the apple spawn
        private WaitForSeconds appleSpawnDelayWFS;

        //prefab to spawn at the end of the snake to make it grow when it eats an apple
        public Transform snakePartPrefab;
        //pause between moves 
        public float snakeUpdateDelay;
        //cached wait for second for the snake
        private WaitForSeconds snakeUpdateDelayWFS;

        private Vector3 currentDirection;
        private Vector3 newDirection;

        private bool gameOver = false;

        //Keep track of the body parts to update.
        //We could also keep track of the children of this object but then we'd be doing unnecessary transform updates.
        private List<Transform> snakeParts = new List<Transform>();

        void Awake()
        {
            //get the initial direction
            currentDirection = nextPositionIndicator.transform.position - transform.position;
            currentDirection.Normalize();
            newDirection = currentDirection;

            //make sure we are on whole positions to start with
            transform.SnapToGrid(1);

            //make sure the head is also a snake part to update
            snakeParts.Add(transform);

            snakeUpdateDelayWFS = new WaitForSeconds(snakeUpdateDelay);
            StartCoroutine(SnakeUpdate());

            appleSpawnDelayWFS = new WaitForSeconds(appleSpawnDelay);
            StartCoroutine(SpawnApples());
        }

        private IEnumerator SnakeUpdate()
        {
            while (true)
            {
                yield return snakeUpdateDelayWFS;

                //could we move or did we crash into a boundary or ourselves?
                if (!MoveSnake())
                {
                    Debug.Log("Game over");
                    gameOver = true;
                    StopAllCoroutines();
                    yield break;
                }
            }
        }

        private IEnumerator SpawnApples()
        {
            while (!gameOver)
            {
                yield return appleSpawnDelayWFS;
                SpawnApple();
            }
        }

        private bool MoveSnake()
        {
            Vector3 newHeadPosition = transform.position + newDirection;

            //went outside?
            if (!snakeField.IsInside(newHeadPosition)) return false;

            //bit own tail?
            Transform contents = snakeField.GetContents(newHeadPosition);
            if (contents != null && contents.CompareTag("SnakePart"))
            {
                return false;
            }

            //store the tail position in case we need to grow, before we move the head
            Transform tail = snakeParts.Last();
            Vector3 lastTailPosition = tail.position;
            snakeField.Clear(tail);

            //move all elements of the snake from back to front to the position of the element in front of it
            //except the head (note the i > 0) since it has no predecessor
            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                snakeParts[i].position = snakeParts[i - 1].position;
                snakeField.Store(snakeParts[i]);
            }
            
            //now update head:
            transform.position = newHeadPosition;
            snakeField.Store(transform);

            //and the rest of the administration
            nextPositionIndicator.position = newHeadPosition + newDirection;
            currentDirection = newDirection;

            //if we ate something else than our own tail
            if (contents != null)
            {
                snakeField.Clear(contents);
                Destroy(contents.gameObject);

                //spawn new tail
                Transform newTail = Instantiate(snakePartPrefab, lastTailPosition, Quaternion.identity);
                snakeParts.Add(newTail);
                snakeField.Store(newTail);
            }
            
            return true;
        }

        private void SpawnApple()
        {
            Vector3 randomWorldPosition = snakeField.GetRandomWorldPosition();
            if (snakeField.GetContents(randomWorldPosition) == null)
            {
                Transform pickup = Instantiate(applePrefab, randomWorldPosition, Quaternion.identity);
                snakeField.Store(pickup);
            }
        }

        private void Update()
        {
            if (gameOver) return;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                newDirection = Vector3.up;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                newDirection = Vector3.down;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                newDirection = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                newDirection = Vector3.right;
            }

            if (Vector3.Dot(currentDirection, newDirection) > -0.5f)
            {
                nextPositionIndicator.position = transform.position + newDirection;
            }
            else
            {
                newDirection = currentDirection;
            }
        }

    }
}