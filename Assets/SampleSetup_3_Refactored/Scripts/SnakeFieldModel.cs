using UnityEngine;

namespace SampleSetup_3_Refactored
{
    /** 
     * SnakeFieldModel allows us to know where objects are in a 2d grid.
     */
    public class SnakeFieldModel
    {
        //cache width and height to avoid GetLength(0) and GetLength(1) all the time.
        public readonly int width;
        public readonly int height;

        //grid that contains reference to all objects in the grid, can be anything
        private object[,] grid;

        public SnakeFieldModel(int pWidth, int pHeight)
        {
            Debug.Assert(pWidth > 0 && pHeight > 0, "SnakeField size invalid");

            width = pWidth;
            height = pHeight;
            grid = new object[width, height];
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
            return (pGridPosition.x >= 0 && pGridPosition.x < width && pGridPosition.y >= 0 && pGridPosition.y < height);
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
            else
			{
                Debug.Log("Not stored");
			}
        }

        public Vector2Int GetRandomLocation(int pBorderPadding = 0)
        {
            return new Vector2Int(Random.Range(pBorderPadding, width-pBorderPadding), Random.Range(pBorderPadding, height-pBorderPadding));
        }

    }
}