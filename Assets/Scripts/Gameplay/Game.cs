using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameCallback();

public class Game : MonoBehaviour
{
    public event GameCallback OnGameOver;

    public GameData gameData;
    public PlayerInput playerInput;
    public BlockController blockController;

    //Period to trigger block descent
    [HideInInspector]
    public float currentPeriod = 0;

    //Period when not in speed mode
    private float normalPeriod = 0;
    //Game level
    private int level = 1;

    private void Awake()
    {
        playerInput.OnSpeedDown += OnSpeedDown;
        playerInput.OnSpeedUp += OnSpeedUp;
        blockController.OnBlockSettle += OnBlockSettle;
        Time.timeScale = 1;
    }


    void Start()
    {
        normalPeriod = gameData.startingPeriod;
        currentPeriod = normalPeriod;
    }

    private void OnDestroy()
    {
        playerInput.OnSpeedDown -= OnSpeedDown;
        playerInput.OnSpeedUp -= OnSpeedUp;
        blockController.OnBlockSettle -= OnBlockSettle;
    }

    #region Events
    //Shrink the current period
    private void OnSpeedDown()
    {
        currentPeriod = gameData.speedPeriod;
    }

    //Return to normal period
    private void OnSpeedUp()
    {
        currentPeriod = normalPeriod;
    }

    private void OnBlockSettle(List<Vector2Int> positions, bool speed)
    {
        bool gameOver = false;
        foreach(var position in positions)
        {
            gameOver = IsGameOver(position);
            if (gameOver)
            {
                //OnGameOver.Invoke();
                GameOver();
                return;
            }
        }
        
    }

    private void OnLevelUp()
    {
        normalPeriod -= gameData.decrementPerLevel;
        currentPeriod = normalPeriod;
    }
    #endregion


    #region Level
    public int GetLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        level++;
        OnLevelUp();
    }
    #endregion

    #region Game Over
    //Checks if game is over to the specific coordinate
    private bool IsGameOver(Vector2Int position)
    {
        return position.y > (gameData.gameOverLimitRow - 1);
    }

    private void GameOver()
    {
        //Stop time
        Time.timeScale = 0;

        //Destroy required objects

        //Canvas
        GameObject canvasGO = FindObjectOfType<Canvas>().gameObject;
        if (canvasGO != null)
            Destroy(canvasGO);

        SceneManager.LoadSceneAsync("Game Over", LoadSceneMode.Additive);
    }
    #endregion

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }
}
