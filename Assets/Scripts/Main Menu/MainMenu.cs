using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenInstructions()
    {
        //Adds Instructions to scene stack
        SceneManager.LoadScene("Instructions", LoadSceneMode.Additive);
    }

    public void OpenGameplay()
    {
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }
}
