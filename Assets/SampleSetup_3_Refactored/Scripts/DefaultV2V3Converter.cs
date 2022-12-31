using UnityEngine;

namespace SampleSetup_3_Refactored
{
	/**
	 * Converts 2d grid positions to 3d space.
	 */
	[DisallowMultipleComponent]
	public class DefaultV2V3Converter : MonoBehaviour, IV2V3Converter
	{
		private Vector3 offset = new Vector2();
		private SnakeFieldModel snakeFieldModel;

		private void Awake()
		{
			//annoying that we need a reference to the model through the gamemanager, but it is what it is
			snakeFieldModel = GameManager.instance.snakeFieldModel;
			offset = new Vector3((-snakeFieldModel.width / 2.0f) + 0.5f, (-snakeFieldModel.height / 2.0f) + 0.5f, 0);
		}

		public Vector3 Convert(Vector2Int pVector2Int)
		{
			return new Vector3(pVector2Int.x, pVector2Int.y, 0) + offset + transform.position;
		}
	}
}