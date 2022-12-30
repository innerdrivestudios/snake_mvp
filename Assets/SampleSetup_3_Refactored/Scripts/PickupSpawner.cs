using System.Collections;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 5;
        [SerializeField] private GameObject pickupPrefab;

		private void Awake()
		{
            GameManager.instance.onGameStart.AddListener(ActivateSpawner);
            GameManager.instance.onGameEnd.AddListener(DeactivateSpawner);
		}

		private void ActivateSpawner()
		{
            StartCoroutine(SpawnCoroutine());
		}

        private void DeactivateSpawner()
		{
            StopAllCoroutines();
		}

        private IEnumerator SpawnCoroutine()
        {
            WaitForSeconds cachedSpawnWFS = new WaitForSeconds(spawnRate);
            IV2V3Converter v2V3Converter = GetComponent<IV2V3Converter>();
            SnakeFieldModel snakeFieldModel = GameManager.instance.snakeFieldModel;

            while (Application.isPlaying)
			{
                yield return cachedSpawnWFS;

                Vector2Int randomPosition = snakeFieldModel.GetRandomLocation(1);
                if (snakeFieldModel.GetContents(randomPosition) == null)
				{
                    GameObject pickup = Instantiate(pickupPrefab, v2V3Converter.Convert(randomPosition), Quaternion.identity);
                    snakeFieldModel.Store(randomPosition, pickup);
				}
			}
        }

    }

}