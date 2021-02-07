using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text scoreText, highScoreText;

    private Score score;

    private void Start()
    {
        //Find score script reference to get current and high scores
        score = FindObjectOfType<Score>();

        scoreText.text = score.GetCurrentScore().ToString("000000");
        highScoreText.text = score.GetHighScore().ToString("000000");
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }

    public void Replay()
    {
        SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single);
    }
}
