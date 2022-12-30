using System.Collections.Generic;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	[DisallowMultipleComponent]
	public class LerpedSnakeView : MonoBehaviour
	{
		[SerializeField] private Transform snakeBodyHeadPrefab;
		[SerializeField] private Transform snakeBodyOddPartPrefab;
		[SerializeField] private Transform snakeBodyEvenPartPrefab;

		[SerializeField] private float playPulseSpeed = 0.2f;
		[SerializeField] private float waitPulseSpeed = 0.1f;
		[Range(0, Mathf.PI)] [SerializeField] private float phaseOffsetPerBodyPart = 0.2f;
		[Range(0, 0.4f)] [SerializeField] private float scaleFactor = 0.2f;
		[Range(0, 1)][SerializeField] private float scaleReductionFactor = 0.2f;

		private SnakeModel snakeModel;
		private IV2V3Converter v2V3Converter;
		private float timeSinceLastUpdate = 0;

		private List<Transform> snakeBodyParts = new List<Transform>();
		private List<SnakeBodyPartData> snakeBodyPartData = new List<SnakeBodyPartData>();

		private Vector3 originalScale;

		class SnakeBodyPartData
		{
			public Vector3 currentPosition;
			public Vector3 targetPosition;
			public Quaternion currentRotation;
			public Quaternion targetRotation;
		}

		private void Awake()
		{
			v2V3Converter = GetComponent<IV2V3Converter>();
			snakeModel = GameManager.instance.snakeModel;
			snakeModel.onSnakePositionUpdated += OnSnakePositionUpdated;
			originalScale = snakeBodyHeadPrefab.localScale;
		}

		private void Start()
		{
			//execute once to initialize after awake
			OnSnakePositionUpdated();
		}

		private void OnSnakePositionUpdated()
		{
			timeSinceLastUpdate = 0;
			int snakePartIndex = 0;

			foreach (Vector2Int snakePart in snakeModel)
			{
				bool justInitialized = false;
				if (snakeBodyParts.Count <= snakePartIndex)
				{
					AddBodyPart(snakeBodyParts.Count);
					justInitialized = true;
				}

				//get the new target position and rotation for this body part 
				SnakeBodyPartData bodyPartData = snakeBodyPartData[snakePartIndex];
				bodyPartData.targetPosition = v2V3Converter.Convert(snakePart);
				bodyPartData.targetRotation = Quaternion.LookRotation((snakePartIndex == 0 ?v2V3Converter.Convert(snakeModel.nextHeadPosition):bodyPartData.targetPosition) - bodyPartData.currentPosition, Vector3.back);

				Transform bodyPart = snakeBodyParts[snakePartIndex];

				//if we were just created, we cannot lerp, we just need to use the target values directly
				if (justInitialized)
				{
					bodyPart.localPosition = bodyPartData.currentPosition = bodyPartData.targetPosition;
					bodyPart.localRotation = bodyPartData.currentRotation = bodyPartData.targetRotation;
					bodyPart.gameObject.SetActive(true);
				}
				else
				{
					bodyPartData.currentPosition = bodyPart.localPosition;
					bodyPartData.currentRotation = bodyPart.localRotation;
				}

				snakePartIndex++;
			}
		}

		private void AddBodyPart (int pBodyPartIndex)
		{
			Transform bodyPart = Instantiate(pBodyPartIndex == 0 ? snakeBodyHeadPrefab : (pBodyPartIndex % 2 == 0 ? snakeBodyEvenPartPrefab : snakeBodyOddPartPrefab), transform);
			bodyPart.gameObject.SetActive(false);
			snakeBodyParts.Add(bodyPart);
			snakeBodyPartData.Add(new SnakeBodyPartData());
		}

		private void Update()
		{
			if (GameManager.instance.gameState == GameManager.GameState.GAME_OVER)
			{
				Destroy(this);
				return;
			}

			timeSinceLastUpdate += Time.deltaTime;
			float interpolationValue = timeSinceLastUpdate / GameManager.instance.stepDelay;
			float phaseSpeedMultiplier = Mathf.PI * (1 / GameManager.instance.stepDelay) * (GameManager.instance.gameState == GameManager.GameState.PLAYING?playPulseSpeed:waitPulseSpeed);

			for (int i = 0; i < snakeBodyParts.Count; i++)
			{
				SnakeBodyPartData bodyPartData = snakeBodyPartData[i];
				snakeBodyParts[i].transform.localPosition = Vector3.Lerp(bodyPartData.currentPosition, bodyPartData.targetPosition, interpolationValue);
				snakeBodyParts[i].transform.localRotation = Quaternion.Lerp(bodyPartData.currentRotation, bodyPartData.targetRotation, interpolationValue);

				float bodyPartIndexInfluence = (i / (float)snakeBodyParts.Count) * scaleReductionFactor;

				snakeBodyParts[i].transform.localScale =
					originalScale +
					new Vector3(1, 1, 1) * scaleFactor * Mathf.Sin(Time.realtimeSinceStartup * phaseSpeedMultiplier + i * Mathf.PI * phaseOffsetPerBodyPart) -
					new Vector3(1, 1, 1) * bodyPartIndexInfluence;
			}

		}

	}

}