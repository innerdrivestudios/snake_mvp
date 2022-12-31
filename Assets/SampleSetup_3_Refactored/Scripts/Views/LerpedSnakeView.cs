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

		[SerializeField] private float playPulseSpeed = 0.2f;								//body pulsating speed while in play mode
		[SerializeField] private float waitPulseSpeed = 0.1f;								//body pulasting speed while not in play mode
		[Range(0, Mathf.PI)] [SerializeField] private float phaseOffsetPerBodyPart = 0.2f;	//sin wave phase offset modifier per body part
		[Range(0, 0.4f)] [SerializeField] private float scaleFactor = 0.2f;					//varies the original scale from -0.2 .. 0.2 (or whatever is set)
		[Range(0, 1)][SerializeField] private float scaleReductionFactor = 0.2f;			//each part gets (index/count) * scaleReductionFactor smaller

		private SnakeModel snakeModel;														//contains info about all the snake parts
		private IV2V3Converter v2V3Converter;												//helps converting snakemodel 2d coordinates to 3d space
		private float timeSinceLastUpdate = 0;												//for lerping, is reset every move

		private List<Transform> snakeBodyParts = new List<Transform>();						//all transform parts matching each snakepart from the snake model
		private List<SnakeBodyPartData> snakeBodyPartData = new List<SnakeBodyPartData>();	//store start and end data for lerping per body part

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
				
				//do we need to add visual parts (and accompanying body part data?)
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
				else //use the current position and rotation of a bodypart as starting values for the lerp
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