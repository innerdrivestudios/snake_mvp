using System.Collections.Generic;
using UnityEngine;

/**
 * Represents all the different parts of the snake. Work in progress.
 */
public class SnakeModel 
{
    //Stores all different parts of the snake, head to tail, as Vector2Int positions
    private LinkedList<Vector2Int> _snake = new LinkedList<Vector2Int>();
    //passed in upon creation so that we can keep our snake administration up to date as we are moving the snake around
    private SnakeField _snakeField = null;                                  

    public SnakeModel (SnakeField snakeField)
	{
        _snakeField = snakeField;
	}

    public LinkedListNode<Vector2Int> FirstPart => _snake.First;

    public void AddPart(Vector2Int bodyPartPosition)
	{
        _snake.AddLast(bodyPartPosition);
        _snakeField.Store(bodyPartPosition, this);
	}

    public void Move(Vector2Int direction, bool grow)
	{
        //initialize for iteration over all body parts
        LinkedListNode<Vector2Int> snakePart = FirstPart;
        Vector2Int nextPartPosition = snakePart.Value + direction;

        //store the new position of the head in the snakefield grid since that is what we will occupy next
        _snakeField.Store(nextPartPosition, this);

        //iterate over all body parts shifting them to where their predecessor was
        while (snakePart != null)
		{
            //make sure we retain the current position for the next body part
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
        else //or clear tail if we are not extending
		{
            _snakeField.Clear(nextPartPosition);
		}
	}

    public Vector2Int GetNextHeadPositionFor(Vector2Int direction)
    {
        return FirstPart.Value + direction;
    }

    public bool IsNextHeadPositionValid (Vector2Int direction)
	{
        Vector2Int nextHeadPosition = GetNextHeadPositionFor(direction);
        return _snakeField.IsInside(nextHeadPosition) && _snakeField.GetContents(nextHeadPosition) != this;
    }

}
