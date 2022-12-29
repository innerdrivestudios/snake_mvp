using System.Collections;
using UnityEngine;

namespace SampleSetup_3_Refactored
{

    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 5;
        [SerializeField] private GameObject pickupPrefab;
        
        IEnumerator Start()
        {
            WaitForSeconds cachedSpawnWFS = new WaitForSeconds(spawnRate);
            IV2V3Converter v2V3Converter = GetComponent<IV2V3Converter>();
            SnakeFieldModel snakeFieldModel = GameManager.Instance.SnakeFieldModel;
            SnakeModel snakeModel = GameManager.Instance.SnakeModel;

            while (true)
			{
                yield return cachedSpawnWFS;
                if (GameManager.Instance.GameOver) break;

                //skip spawns as long as snake isn't moving...
                if (snakeModel.CurrentDirection == SnakeModel.SnakeDirection.NONE) continue;

                Vector2Int randomPosition = snakeFieldModel.GetRandomLocation();
                if (snakeFieldModel.GetContents(randomPosition) == null)
				{
                    GameObject pickup = Instantiate(pickupPrefab, v2V3Converter.Convert(randomPosition), Quaternion.identity);
                    snakeFieldModel.Store(randomPosition, pickup);
				}
			}
        }

    }

}