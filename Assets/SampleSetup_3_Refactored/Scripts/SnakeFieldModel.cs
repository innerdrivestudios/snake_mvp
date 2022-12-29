using UnityEngine;

namespace SampleSetup_3_Refactored
{
    /** 
     * SnakeFieldModel allows us to know where objects are in a 2d grid.
     */
    public class SnakeFieldModel
    {
        //cache width and height to avoid GetLength(0) and GetLength(1) all the time.
        public readonly int Width;
        public readonly int Height;

        //grid that contains reference to all objects in the grid, can be anything
        private object[,] grid;

        public SnakeFieldModel(int pWidth, int pHeight)
        {
            Debug.Assert(pWidth > 0 && pHeight > 0, "SnakeField size invalid");

            Width = pWidth;
            Height = pHeight;
            grid = new object[Width, Height];
        }

        public object GetContents(Vector2Int pGridPosition)
        {
            if (IsInside(pGridPosition))
            {
                return grid[pGridPosition.x, pGridPosition.y];
            }
            else
            {
                return null;
            }
        }

        public bool IsInside(Vector2Int pGridPosition)
        {
            return (pGridPosition.x >= 0 && pGridPosition.x < Width && pGridPosition.y >= 0 && pGridPosition.y < Height);
        }

        public void Clear(Vector2Int pGridPosition)
        {
            if (IsInside(pGridPosition))
            {
                grid[pGridPosition.x, pGridPosition.y] = null;
            }
        }

        public void Store(Vector2Int pGridPosition, object pObject)
        {
            if (IsInside(pGridPosition))
            {
                grid[pGridPosition.x, pGridPosition.y] = pObject;
            }
        }

        public Vector2Int GetRandomLocation()
        {
            return new Vector2Int(Random.Range(0, Width), Random.Range(0, Height));
        }

    }
}