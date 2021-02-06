using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public ScoreData scoreData;
    public Game game;
    public BlockController blockController;

    public Text currentScoreText;
    public Text highScoreText;

    [HideInInspector]
    public int currentScore = 0;

    private int highScore;

    private void Awake()
    {
        blockController.OnBlockSettle += OnBlockSettle;
    }

    // Start is called before the first frame update
    void Start()
    {
        highScore = GetHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    private void SetHighScore(int score)
    {
        PlayerPrefs.SetInt("HighScore", score);
    }

    private void OnBlockSettle(List<Vector2Int> positions)
    {
        AddBlockScore(false);
    }

    public void AddBlockScore(bool withSpeed)
    {
        if (!withSpeed)
        {
            currentScore += scoreData.pointsWithoutSpeed * game.GetLevel();
        }
        else
        {
            currentScore += scoreData.pointsWithSpeed * game.GetLevel();
        }

        //Updates high score variable if needed
        if (currentScore > highScore)
            highScore = currentScore;

        UpdateScoreText();
    }

    public void AddRowScore()
    {
        currentScore += scoreData.pointsPerRow * game.GetLevel();

        //Updates high score variable if needed
        if (currentScore > highScore)
            highScore = currentScore;

        UpdateScoreText();
    }

    //Updates current score and high score text
    public void UpdateScoreText()
    {
        currentScoreText.text = currentScore.ToString("000000");
        highScoreText.text = currentScore.ToString("000000");
    }
}
