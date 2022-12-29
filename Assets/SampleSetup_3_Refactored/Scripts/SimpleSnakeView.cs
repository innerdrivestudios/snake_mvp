using System.Collections.Generic;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class SimpleSnakeView : MonoBehaviour
	{
		[SerializeField] private GameObject snakeBodyPartPrefab;

		private SnakeModel snakeModel;
		private LinkedList<GameObject> snakeBodyParts = new LinkedList<GameObject>();
		private IV2V3Converter v2V3Converter;

		private void Awake()
		{
			v2V3Converter = GetComponent<IV2V3Converter>();
			snakeModel = GameManager.Instance.SnakeModel;
			snakeModel.OnSnakePositionUpdated += moveSnake;
		}

		private void Start()
		{
			//execute once to initialize after awake
			moveSnake();
		}

		private void moveSnake()
		{
			ensureEnoughBodyParts();

			LinkedListNode<GameObject> currentSnakeBodyPart = snakeBodyParts.First;

			foreach (Vector2Int snakeModelPartPosition in snakeModel)
			{
				currentSnakeBodyPart.Value.transform.position = v2V3Converter.Convert(snakeModelPartPosition);
				currentSnakeBodyPart = currentSnakeBodyPart.Next;
			}
		}

		private void ensureEnoughBodyParts()
		{
			//snake only ever grows
			while (snakeBodyParts.Count < snakeModel.Length)
			{
				GameObject bodyPart = Instantiate(snakeBodyPartPrefab, transform);
				snakeBodyParts.AddLast(bodyPart);
			}
		}
	}

}