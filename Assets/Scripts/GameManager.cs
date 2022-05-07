using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    private SnakeField snakeField;
    private SnakeModel snakeModel;

    private void Awake()
    {
        snakeField = new SnakeField(width, height);
        snakeModel = new SnakeModel();
    }

    private void OnDrawGizmos()
    {
        if (snakeField == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Gizmos.color = (snakeField.GetContents(new Vector2Int(x, y)) == null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                Vector3 worldPosition = new Vector3(x, y, 0) + transform.position;
                Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
            }
        }

        if (snakeModel == null) return;

        LinkedListNode<Vector2Int> snakeElement = snakeModel.First;

        Gizmos.color = Color.blue;

        while (snakeElement != null) {
            Vector3 worldPosition = new Vector3(snakeElement.Value.x, snakeElement.Value.y, 0) + transform.position;
            Gizmos.DrawCube(worldPosition, new Vector3(0.9f, 0.9f, 0.1f));
            snakeElement = snakeElement.Next;
        }
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
            snakeModel.AddPart(snakeField.GetRandomLocation());
		}
	}

}
