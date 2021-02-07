using UnityEngine;

public class SoundEventEmitter : MonoBehaviour
{
    //Singleton pattern
    public static SoundEventEmitter instance;

    private void Awake()
    {
        instance = this;
    }

    //Play sound effect one time only
    public void PlaySFXOneShot(string eventString)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventString);
    }

    public void SetFMODGlobalParameter(string parameter, int value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameter, value);
    }
}
