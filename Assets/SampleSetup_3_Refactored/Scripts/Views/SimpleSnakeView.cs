using System.Collections.Generic;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class SimpleSnakeView : MonoBehaviour
	{
		[SerializeField] private GameObject snakeBodyPartPrefab;

		private SnakeModel snakeModel;
		private List<GameObject> snakeBodyParts = new List<GameObject>();
		private IV2V3Converter v2V3Converter;

		private void Awake()
		{
			v2V3Converter = GetComponent<IV2V3Converter>();
			snakeModel = GameManager.instance.snakeModel;
			snakeModel.onSnakePositionUpdated += MoveSnake;
		}

		private void Start()
		{
			//execute once to initialize after awake
			MoveSnake();
		}

		private void MoveSnake()
		{
			EnsureEnoughBodyParts();

			int snakeBodyPartIndex = 0;

			foreach (Vector2Int snakePart in snakeModel)
			{
				//localPosition only works if parent is at 0,0,0
				snakeBodyParts[snakeBodyPartIndex].transform.localPosition = v2V3Converter.Convert(snakePart);
				snakeBodyPartIndex++;
			}
		}

		private void EnsureEnoughBodyParts()
		{
			//snake only ever grows
			while (snakeBodyParts.Count < snakeModel.length)
			{
				GameObject bodyPart = Instantiate(snakeBodyPartPrefab, transform);
				snakeBodyParts.Add(bodyPart);
			}
		}
	}

}