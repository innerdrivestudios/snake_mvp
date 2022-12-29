using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class DefaultV2V3Converter : MonoBehaviour, IV2V3Converter
	{
		private Vector3 offset = new Vector2();
		private SnakeFieldModel snakeFieldModel;

		private void Awake()
		{
			snakeFieldModel = GameManager.Instance.SnakeFieldModel;
			offset = new Vector3((-snakeFieldModel.Width / 2.0f) + 0.5f, (-snakeFieldModel.Height / 2.0f) + 0.5f, 0);
		}

		public Vector3 Convert(Vector2Int pVector2Int)
		{
			return new Vector3(pVector2Int.x, pVector2Int.y, 0) + offset;
		}
	}
}