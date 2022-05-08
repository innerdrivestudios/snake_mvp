using System.Collections.Generic;
using UnityEngine;

/**
 * Represents all the different parts of the snake. Work in progress.
 */
public class SnakeModel 
{
    private LinkedList<Vector2Int> _snake = new LinkedList<Vector2Int>();

    public LinkedListNode<Vector2Int> First => _snake.First;

    public void AddPart(Vector2Int bodyPartPosition)
	{
        _snake.AddLast(bodyPartPosition);
	}

    public void Move (Vector2Int direction)
	{
        First.Value = First.Value + direction;
	}
   
}
