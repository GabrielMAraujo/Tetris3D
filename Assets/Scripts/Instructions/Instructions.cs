using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    public void CloseInstructions()
    {
        //Removes Instructions from scene stack
        SceneManager.UnloadSceneAsync("Instructions");
    }
}
