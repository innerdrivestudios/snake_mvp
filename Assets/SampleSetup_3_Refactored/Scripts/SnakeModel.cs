using System;
using System.Collections.Generic;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
    /**
     * The SnakeModel represents the body of the snake, made up of enumerable Vector2Int parts.
     * Includes methods for moving, extending and catching events for these occurances.
     */
    public class SnakeModel
    {
        public event Action onSnakePositionUpdated = delegate { };

        //Define directions as an enum to make sure only valid values can be passed in from the outside.
        public enum SnakeDirection { RIGHT = 0, UP = 1, LEFT = 2, DOWN = 3 };

        //Define a matching direction array that gives us the actual direction vectors matching the directions above.
        private Vector2Int[] snakeDirections = { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

        //Stores all different parts of the snake, head to tail, as Vector2Int positions in a linked list.
        private LinkedList<Vector2Int> snakeParts = new LinkedList<Vector2Int>();

        public SnakeDirection currentDirection { get; private set; } = SnakeDirection.DOWN;

        public SnakeModel(Vector2Int pInitialHeadPosition, SnakeDirection pDirection = SnakeDirection.DOWN, int pLength = 1)
        {
            currentDirection = pDirection;

            for (int i = 0; i < pLength; i++) { 
                snakeParts.AddLast(pInitialHeadPosition - i * snakeDirections[(int)pDirection]);
            }
        }

        public bool Move(SnakeDirection pNewDirection, bool pGrow)
        {
            if (!IsValidDirection(pNewDirection)) return false;
            currentDirection = pNewDirection;

            //just add new position in front of snake, very cheap O(1) operation since this is a linkedlist
            snakeParts.AddFirst(snakeParts.First.Value + snakeDirections[(int)currentDirection]);

            //if we grow, we do not remove the last part, otherwise we do
            if (!pGrow)
            {
                snakeParts.RemoveLast();
            }

            onSnakePositionUpdated();

            return true;
        }

        public Vector2Int headPosition => snakeParts.First.Value;
        public Vector2Int nextHeadPosition => headPosition + snakeDirections[(int)currentDirection];
        public Vector2Int tailPosition => snakeParts.Last.Value;
        public int length => snakeParts.Count;

        public bool IsValidDirection(SnakeDirection pDirection)
        {
            return Vector2.Dot(snakeDirections[(int)currentDirection], snakeDirections[(int)pDirection]) >= 0;
        }

        public Vector2Int GetNextHeadPositionFor (SnakeDirection pNewDirection)
		{
            return headPosition + snakeDirections[(int)pNewDirection];
        }

        public LinkedList<Vector2Int>.Enumerator GetEnumerator()
        {
            return snakeParts.GetEnumerator();
        }
    }
}