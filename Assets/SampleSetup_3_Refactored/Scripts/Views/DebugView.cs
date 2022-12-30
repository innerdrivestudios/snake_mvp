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

        private void Awake()
        {
            v2v3Converter = GetComponent<IV2V3Converter>();
            snakeModel = GameManager.instance.snakeModel;
            snakeFieldModel = GameManager.instance.snakeFieldModel;
        }

        private void OnDrawGizmos()
        {
            if (snakeModel != null) DrawSnakeModelGizmos();
            if (snakeFieldModel != null) DrawSnakeFieldModelGizmos();
        }

        private void DrawSnakeModelGizmos()
        {
            float blockSize = 0.6f;

            foreach (Vector2Int snakePart in snakeModel)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(v2v3Converter.Convert(snakePart), Vector3.one * blockSize);
            }
        }

        private void DrawSnakeFieldModelGizmos()
		{
            float blockSize = 0.95f;

            for (int x = 0; x < snakeFieldModel.width; x++)
            {
                for (int y = 0; y < snakeFieldModel.height; y++)
                {
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    Gizmos.color = (snakeFieldModel.GetContents(gridPosition) == null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                    Gizmos.DrawCube(v2v3Converter.Convert(gridPosition), Vector3.one * blockSize);
                }
            }
        }

    }
}