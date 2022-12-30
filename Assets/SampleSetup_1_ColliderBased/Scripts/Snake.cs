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
        public Transform nextPositionIndicator;

        //prefab to spawn every now and then which will make the snake grow when eaten
        public Transform applePrefab;
        //spawn rate in seconds
        public float appleSpawnDelay;

        //prefab to spawn at the end of the snake to make it grow when it eats an apple
        public Transform snakePartPrefab;
        //pause between moves 
        public float snakeUpdateDelay;

        public Vector2 appleSpawnXRange;
        public Vector2 appleSpawnYRange;

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
            currentDirection = nextPositionIndicator.transform.position - transform.position;
            currentDirection.Normalize();
            newDirection = currentDirection;

            //make sure we are on whole positions to start with
            transform.SnapToGrid(1);

            //make sure the head is also a snake part to update
            snakeParts.Add(transform);

            StartCoroutine(SnakeUpdate());
            StartCoroutine(SpawnApples());
        }

        private IEnumerator SnakeUpdate()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(snakeUpdateDelay);
                MoveSnake();
            }
        }

        private IEnumerator SpawnApples()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(appleSpawnDelay);
                SpawnApple();
            }
        }

        private void MoveSnake()
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
            nextPositionIndicator.position = newHeadPosition + newDirection;
            currentDirection = newDirection;
        }

        private void SpawnApple()
        {
            Vector3 randomAppleSpawnPosition =
                new Vector3(
                    Mathf.Round(Random.Range(appleSpawnXRange.x, appleSpawnXRange.y)),
                    Mathf.Round(Random.Range(appleSpawnYRange.x, appleSpawnYRange.y)),
                    0
                );

            //check if it is far away enough from anything else
            if (Physics.OverlapBox(randomAppleSpawnPosition, Vector3.one).Length == 0)
			{
                Instantiate(applePrefab, randomAppleSpawnPosition, Quaternion.identity);
			}
        }

        private void Update()
        {
            if (gameOver)
            {
                enabled = false;
                return;
            }

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
                nextPositionIndicator.position = transform.position + newDirection;
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
                Debug.Log("Game Over");
                StopAllCoroutines();
                return;
			}

            if (other.CompareTag("Pickup"))
			{
                Destroy(other.gameObject);
                Transform newSnakePart = Instantiate(snakePartPrefab, lastTailPosition, Quaternion.identity);
                snakeParts.Add(newSnakePart);

                //if you'd want to update any points, do it here ;)
            }
        }

	}
}