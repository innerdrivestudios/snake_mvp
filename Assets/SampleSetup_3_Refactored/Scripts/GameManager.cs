using System.Collections;
using UnityEngine;

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
        //Define the playing field size
        [SerializeField] private int width = 40;
        [SerializeField] private int height = 30;

        [Header("Game play settings")]
        [Range(0.1f, 1)]
        [SerializeField] private float stepDelay = 1;

        public SnakeModel SnakeModel { get; private set; }                  //Defines all the parts of the snake
        public SnakeFieldModel SnakeFieldModel { get; private set; }        //Keeps track of where everything is
        private IInputProvider inputProvider;                               //Provides direction to the snake
        private WaitForSeconds cachedWFSStepDelay;

        public bool GameOver { get; private set; } = false;

        override protected void Awake()
        {
            base.Awake();

            SnakeModel = new SnakeModel(new Vector2Int(width >> 1, height >> 1));
            SnakeFieldModel = new SnakeFieldModel(width, height);
            SnakeFieldModel.Store(SnakeModel.HeadPosition, SnakeModel);

            inputProvider = GetComponent<IInputProvider>();
            inputProvider.OnDirectionChanged += InputProvider_OnDirectionChanged;

            cachedWFSStepDelay = new WaitForSeconds(stepDelay);
        }

        private void InputProvider_OnDirectionChanged(SnakeModel.SnakeDirection pNewDirection)
        {
            SnakeModel.SetNewDirection(pNewDirection);
        }

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                yield return cachedWFSStepDelay;

                //if we are not moving, don't do anything
                if (SnakeModel.NewDirection == SnakeModel.SnakeDirection.NONE) continue;
                //will we move outside? If so game over
                if (!SnakeFieldModel.IsInside(SnakeModel.NextHeadPosition)) break;
                //did we hit ourselves? If so game over
                object nextHeadPositionContents = SnakeFieldModel.GetContents(SnakeModel.NextHeadPosition);
                if (nextHeadPositionContents is SnakeModel)
                {
                    Debug.Log("hit snake");
                    break;
                }

                //did we eat something? If so grow
                bool grow = (nextHeadPositionContents != null);
                if (nextHeadPositionContents is GameObject pickup) Destroy(pickup);

                //if we didn't grow clear tail position before moving on
                if (!grow) SnakeFieldModel.Clear(SnakeModel.TailPosition);
                SnakeModel.Move(grow);
                //register new head position, will overwrite any pickup contents
                SnakeFieldModel.Store(SnakeModel.HeadPosition, SnakeModel);
            }

            Debug.Log("Game over");
            GameOver = true;
        }
    }
}