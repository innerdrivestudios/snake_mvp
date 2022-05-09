using UnityEngine;

/** 
 * SnakeField allows us to administrate where pickup objects and the snake are.
 * This in turn allows us to quickly detect whether we've hit a pickup objects and 
 * where we can spawn new pickup objects.
 */
public class SnakeField
{
    //cache width and height to avoid GetLength(0) and GetLength(1) all the time.
    private int _width = 0;
    private int _height = 0;
    //grid that contains reference to all objects in the grid, can be anything
    private object[,] _grid;

    public SnakeField (int width, int height)
	{
        Debug.Assert(width > 0 && height > 0, "SnakeField size invalid");

        _width = width;
        _height = height;
		_grid = new object[_width, _height];
    }

    public object GetContents(Vector2Int gridPosition)
    {
        if (IsInside(gridPosition))
        {
            return _grid[gridPosition.x, gridPosition.y];
        } 
        else
        {
            return null;
        }
    }

    public bool IsInside (Vector2Int gridPosition)
    {
        return (gridPosition.x >= 0 && gridPosition.x < _width && gridPosition.y >= 0 && gridPosition.y < _height);
    }

    public void Clear (Vector2Int gridPosition)
    {
        if (IsInside(gridPosition)) {
            _grid[gridPosition.x, gridPosition.y] = null;
        }
    }

    public void Store (Vector2Int gridPosition, object obj)
    {
        if (IsInside(gridPosition))
		{
           _grid[gridPosition.x, gridPosition.y] = obj;
		}
    }

    public Vector2Int GetRandomLocation()
	{
        return new Vector2Int(Random.Range(0, _width), Random.Range(0, _height));
	}

}
