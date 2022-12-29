﻿using UnityEngine;

namespace SampleSetup_2_Optimized
{
    /**
     * Simple grid that allows us to quickly track where certain elements are.
     * Do not scale the grid since its scale will be completely ignored.
     * 
     * Note that we always assume a grid size of 1 (room for improvement there).
     */
    public class SnakeField : MonoBehaviour
    {
        public int Width;
        public int Height;

        //the offset we need to make sure the center of the grid is at our transform's position
        //and the offset that we need to convert between gridspace and worldspace
        private Vector3 offset = new Vector2();

        //x and y align with worldspace
        private Transform[,] grid;

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (grid == null || grid.GetLength(0) != Width || grid.GetLength(1) != Height)
            {
                //if the grid size changes reset the grid's backing store and the offset we use for conversion
                //Required offset based on grid size to keep grid centered on our transform:
                //1 -> 0, 2 -> -.5, 3 -> -1, 4 -> -1.5, etc
                offset = new Vector3((-Width / 2.0f) + 0.5f, (-Height / 2.0f) + 0.5f, 0);
                grid = new Transform[Width, Height];
            }
        }

        private Vector2Int getGridIndex(Vector3 pWorldPosition)
        {
            //we take the given world position, first make it relative to our own position, then relative to the 
            //topleft position of the grid. Round what is left, we always assume a grid size of one.
            Vector3 localPosition = pWorldPosition - transform.position - offset;
            localPosition.x = Mathf.Round(localPosition.x);
            localPosition.y = Mathf.Round(localPosition.y);
            localPosition.z = Mathf.Round(localPosition.z);

            return new Vector2Int((int)localPosition.x, (int)localPosition.y);
        }

        public bool IsInside(Vector3 pWorldPosition)
        {
            Vector2Int localPosition = getGridIndex(pWorldPosition);
            return (localPosition.x >= 0 && localPosition.x < Width && localPosition.y >= 0 && localPosition.y < Height);
        }

        public Transform GetContents(Vector3 pWorldPosition)
        {
            Vector2Int localPosition = getGridIndex(pWorldPosition);

            if (localPosition.x >= 0 && localPosition.x < Width && localPosition.y >= 0 && localPosition.y < Height)
            {
                return grid[localPosition.x, localPosition.y];
            }
            else
            {
                return null;
            }
        }

        public void Clear(Transform pTransform)
        {
            Vector2Int gridIndex = getGridIndex(pTransform.position);
            grid[gridIndex.x, gridIndex.y] = null;
        }

        public void Store(Transform pTransform)
        {
            Vector2Int gridIndex = getGridIndex(pTransform.position);
            grid[gridIndex.x, gridIndex.y] = pTransform;
        }

        public Vector3 GetRandomWorldPosition()
        {
            Vector3 localPosition = new Vector3(Random.Range(0, Width), Random.Range(0, Height), 0);
            Vector3 worldPosition = localPosition + transform.position + offset;
            return worldPosition;
        }

        private void OnDrawGizmos()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Gizmos.color = (grid[x, y] == null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                    Vector3 worldPosition = transform.position + new Vector3(x, y, 0) + new Vector3(offset.x, offset.y, 0);
                    Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
                }
            }
        }

    }
}