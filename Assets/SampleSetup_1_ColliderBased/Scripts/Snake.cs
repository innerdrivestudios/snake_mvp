using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SampleSetup_1_ColliderBased
{
    /**
     * Collision based implementation of the Snake game, everything in a single class.
     * 
     * @author J.C. Wichman, InnerDriveStudios.com
     */
    public class Snake : MonoBehaviour
    {
        //used for initial direction of the snake and possible as an indicator (if it is not disabled)
        public Transform NextPositionIndicator;

        //prefab to spawn every now and then which will make the snake grow when eaten
        public GameObject ApplePrefab;
        //spawn rate in seconds
        public float AppleSpawnDelay;

        //prefab to spawn at the end of the snake to make it grow when it eats an apple
        public GameObject SnakePartPrefab;
        //pause between moves 
        public float SnakeUpdateDelay;

        public Vector2 AppleSpawnXRange;
        public Vector2 AppleSpawnYRange;

        //currentDirection and newDirection are stored separately to make sure the snake doesn't turn back on itself
        private Vector3 currentDirection;
        private Vector3 newDirection;
        //keep track of lastTailPosition before move so that we know where to spawn a new part of the snake if necessary
        private Vector3 lastTailPosition;

        private bool gameOver = false;

        //Keep track of the body parts to update.
        //We could also keep track of the children of this object but then we'd be doing unnecessary transform updates.
        private List<Transform> snakeParts = new List<Transform>();

        void Awake()
        {
            //get the initial direction
            currentDirection = NextPositionIndicator.transform.position - transform.position;
            currentDirection.Normalize();
            newDirection = currentDirection;

            //make sure we are on whole positions to start with
            transform.SnapToGrid(1);

            //make sure the head is also a snake part to update
            snakeParts.Add(transform);

            StartCoroutine(snakeUpdate());
            StartCoroutine(spawnApples());
        }

        private IEnumerator snakeUpdate()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(SnakeUpdateDelay);
                moveSnake();
            }
            Debug.Log("Game over");
            StopAllCoroutines();
        }

        private IEnumerator spawnApples()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(AppleSpawnDelay);
                spawnApple();
            }
        }

        private void moveSnake()
        {
            //use position, in case the NextIndicator is not a child of the head
            Vector3 newHeadPosition = transform.position + newDirection;
            //store the tail position in case we need to grow, before we move the head
            lastTailPosition = snakeParts.Last().position;

            //move all elements of the snake from back to front to the position of the element in front of it
            //except the head (note the i > 0) since it has no predecessor
            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                snakeParts[i].position = snakeParts[i-1].position;
            }
            //now update head:
            transform.position = newHeadPosition;
            
            //and the rest of the administration
            NextPositionIndicator.position = newHeadPosition + newDirection;
            currentDirection = newDirection;
        }

        private void spawnApple()
        {
            Vector3 randomAppleSpawnPosition =
                new Vector3(
                    Mathf.Round(Random.Range(AppleSpawnXRange.x, AppleSpawnXRange.y)),
                    Mathf.Round(Random.Range(AppleSpawnYRange.x, AppleSpawnYRange.y)),
                    0
                );

            //check if it is far away enough from anything else
            if (Physics.OverlapBox(randomAppleSpawnPosition, Vector3.one).Length == 0)
			{
                Instantiate(ApplePrefab, randomAppleSpawnPosition, Quaternion.identity);
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

            //check if requested direction is valid
            if (Vector3.Dot(currentDirection, newDirection) > -0.5f)
            {
                NextPositionIndicator.position = transform.position + newDirection;
            }
            else
            {
                newDirection = currentDirection;
            }
        }

		private void OnTriggerEnter(Collider other)
		{
            if (other.CompareTag("SnakePart") || other.CompareTag("Boundary"))
			{
                gameOver = true;
                StopAllCoroutines();
                return;
			}

            if (other.CompareTag("Pickup"))
			{
                Destroy(other.gameObject);
                GameObject newSnakePart = Instantiate(SnakePartPrefab, lastTailPosition, Quaternion.identity);
                snakeParts.Add(newSnakePart.transform);

                //if you'd want to update any points, do it here ;)
            }
        }

	}
}