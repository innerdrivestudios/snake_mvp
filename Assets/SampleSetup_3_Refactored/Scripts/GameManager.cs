using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SampleSetup_3_Refactored
{
    /**
     * GameManager for the Snake game that ties all elements together.
     */
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1000)]
    public class GameManager : Singleton<GameManager>
    {
        [Header("Playing field settings")]
        [SerializeField] private int width = 40;
        [SerializeField] private int height = 30;

        [Header("Game play settings")]
        [Range(0.1f, 1)]
        [SerializeField] private float _stepDelay = 1;

        [Header("Other")]
        public UnityEvent onGameInit;
        public UnityEvent onGameStart;
        public UnityEvent onGameEnd;
        public UnityEvent<int> onPointScored;

        public SnakeModel snakeModel { get; private set; }                  //Defines all the parts of the snake
        public SnakeFieldModel snakeFieldModel { get; private set; }        //Keeps track of where everything is
        public float stepDelay => _stepDelay;

        public enum GameState { WAITING_TO_START, PLAYING, GAME_OVER }
        public GameState gameState { get; private set; } = GameState.WAITING_TO_START;
        
        private WaitForSeconds cachedWFSStepDelay;
        private SnakeModel.SnakeDirection lastSetDirection;

        private int currentScore = 0;

        override protected void Awake()
        {
            base.Awake();

            lastSetDirection = SnakeModel.SnakeDirection.DOWN;
            snakeModel = new SnakeModel(new Vector2Int(width >> 1, height >> 1), lastSetDirection, 5);
            snakeFieldModel = new SnakeFieldModel(width, height);
            snakeFieldModel.Store(snakeModel.headPosition, snakeModel);

            cachedWFSStepDelay = new WaitForSeconds(_stepDelay);

            onGameInit.Invoke();
        }

        public void SetNewDirection(SnakeModel.SnakeDirection pNewDirection)
        {
            if (snakeModel.IsValidDirection(pNewDirection))
            {
                lastSetDirection = pNewDirection;
            }
        }

        private IEnumerator PlayGame()
        {
            gameState = GameState.PLAYING;
            onGameStart.Invoke();

            while (Application.isPlaying)
            {
                yield return cachedWFSStepDelay;

                //will we move outside? If so game over...
                Vector2Int newPosition = snakeModel.GetNextHeadPositionFor(lastSetDirection);
                if (!snakeFieldModel.IsInside(newPosition)) break;

                //will we hit ourselves? If so game over...
                object nextHeadPositionContents = snakeFieldModel.GetContents(newPosition);
                if (nextHeadPositionContents is SnakeModel) break;

                //did we eat something? If so grow...
                bool grow = (nextHeadPositionContents != null);
                if (nextHeadPositionContents is GameObject pickup) Destroy(pickup);

                //if we didn't grow, clear tail position before moving on
                if (!grow) snakeFieldModel.Clear(snakeModel.tailPosition);
                snakeModel.Move(lastSetDirection, grow);
                //register new head position, will overwrite any pickup contents
                snakeFieldModel.Store(snakeModel.headPosition, snakeModel);

                if (grow) {
                    currentScore++;
                    onPointScored.Invoke(currentScore);
                }
            }

            Debug.Log("Game over");
            gameState = GameState.GAME_OVER;
            onGameEnd.Invoke();
        }

        public void StartGame()
		{
            if (gameState == GameState.WAITING_TO_START)
            {
                StartCoroutine(PlayGame());
            }
        }

        public void RestartGame()
		{
            if (gameState == GameState.GAME_OVER)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }


	}
}