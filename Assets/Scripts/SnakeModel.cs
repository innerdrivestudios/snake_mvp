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

    public void Move(Vector2Int direction, bool grow)
	{
        //initialize for iteration over all body parts
        Vector2Int nextPartPosition = First.Value + direction;
        LinkedListNode<Vector2Int> snakePart = First;

        //iterate over all body parts shifting them to where their predecessor was
        while (snakePart != null)
		{
            Vector2Int currentPartPosition = snakePart.Value;
            snakePart.Value = nextPartPosition;
            
            nextPartPosition = currentPartPosition;
            snakePart = snakePart.Next;
		}

        //extend if required
        if (grow)
		{
            AddPart(nextPartPosition);
		}
	}
   
}
