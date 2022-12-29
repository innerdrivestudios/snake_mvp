using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleSetup_3_Refactored
{

    /**
     * The SnakeModel represents the body of the snake, made up of enumerable Vector2Int parts.
     * Includes methods for moving, extending and catching events for these occurances.
     */
    public class SnakeModel : IEnumerable
    {
        public event Action OnSnakePositionUpdated = delegate { };

        //Define directions as an enum to make sure only valid values can be passed in from the outside.
        public enum SnakeDirection { NONE = 0, RIGHT = 1, UP = 2, LEFT = 3, DOWN = 4 };

        //Define a matching direction array that gives us the actual direction vectors matching the directions above.
        private Vector2Int[] snakeDirections = { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

        //Stores all different parts of the snake, head to tail, as Vector2Int positions in a linked list.
        private LinkedList<Vector2Int> snakeParts = new LinkedList<Vector2Int>();

        //In snake there is a difference between the direction we are moving in (currentDirection)
        //and the direction we want to move in (newDirection).
        //Which newDirection is allowed depends on the currentDirection.
        private SnakeDirection newDirection = SnakeDirection.NONE;
        private SnakeDirection currentDirection = SnakeDirection.NONE;

        public SnakeModel(Vector2Int pInitialHeadPosition)
        {
            snakeParts.AddLast(pInitialHeadPosition);
        }

        public bool SetNewDirection(SnakeDirection pNewDirection)
        {
            if (!IsValidDirection(pNewDirection)) return false;
            newDirection = pNewDirection;
            return true;
        }

        public bool Move(bool pGrow)
        {
            if (!IsValidDirection(newDirection)) return false;

            currentDirection = newDirection;

            //just add new position in front of snake
            snakeParts.AddFirst(snakeParts.First.Value + snakeDirections[(int)currentDirection]);

            //if we grow, we do not remove the last part, otherwise we do
            if (!pGrow)
            {
                snakeParts.RemoveLast();
            }

            OnSnakePositionUpdated();

            return true;
        }

        public Vector2Int HeadPosition => snakeParts.First.Value;
        public Vector2Int NextHeadPosition => HeadPosition + snakeDirections[(int)newDirection];
        public Vector2Int TailPosition => snakeParts.Last.Value;
        public SnakeDirection CurrentDirection => currentDirection;
        public SnakeDirection NewDirection => newDirection;
        public int Length => snakeParts.Count;

        public bool IsValidDirection(SnakeDirection pDirection)
        {
            return pDirection != SnakeDirection.NONE && Vector2.Dot(snakeDirections[(int)currentDirection], snakeDirections[(int)pDirection]) >= 0;
        }

        public IEnumerator GetEnumerator()
        {
            return snakeParts.GetEnumerator();
        }
    }
}