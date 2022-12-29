using UnityEngine;

namespace SampleSetup_3_Refactored
{
    [RequireComponent(typeof(IV2V3Converter), typeof(GameManager))]
    [DisallowMultipleComponent]
    public class DebugView : MonoBehaviour
    {
        private SnakeModel snakeModel;
        private SnakeFieldModel snakeFieldModel;

        private IV2V3Converter v2v3Converter;

        [Header("Debug settings")]
        [SerializeField] bool renderSnake = true;
        [SerializeField] bool renderSnakeDirectionPreview = true;

        private void Awake()
        {
            v2v3Converter = GetComponent<IV2V3Converter>();
            snakeModel = GameManager.Instance.SnakeModel;
            snakeFieldModel = GameManager.Instance.SnakeFieldModel;
        }

        private void OnDrawGizmos()
        {
            if (snakeModel != null && renderSnake) drawSnakeModelGizmos();
            if (snakeFieldModel != null && renderSnake) drawSnakeFieldModelGizmos();
        }

        private void drawSnakeModelGizmos()
        {
            float blockSize = 0.6f;

            foreach (Vector2Int snakePartPosition in snakeModel)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(v2v3Converter.Convert(snakePartPosition), Vector3.one * blockSize);
            }

            if (renderSnakeDirectionPreview)
            {
                //draw where we want to go
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(v2v3Converter.Convert(snakeModel.NextHeadPosition), Vector3.one * blockSize);
            }
        }

        private void drawSnakeFieldModelGizmos()
		{
            float blockSize = 0.95f;

            for (int x = 0; x < snakeFieldModel.Width; x++)
            {
                for (int y = 0; y < snakeFieldModel.Height; y++)
                {
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    Gizmos.color = (snakeFieldModel.GetContents(gridPosition) == null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                    Gizmos.DrawCube(v2v3Converter.Convert(gridPosition), Vector3.one * blockSize);
                }
            }
        }

    }
}