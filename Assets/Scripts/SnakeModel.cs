using System.Collections.Generic;
using UnityEngine;

/**
 * Represents all the different parts of the snake.
 */
public class SnakeModel 
{
    private LinkedList<Vector2Int> snake = new LinkedList<Vector2Int>();

    public LinkedListNode<Vector2Int> First => snake.First;

    public void AddPart(Vector2Int bodyPartPosition)
	{
        snake.AddLast(bodyPartPosition);
	}
    
}
