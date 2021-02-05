using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameData gameData;
    public PlayerInput playerInput;

    //Period to trigger block descent

    //Exposed to public
    [HideInInspector]
    public float currentPeriod = 0;

    //Period when not in speed mode
    private float normalPeriod = 0;

    private void Awake()
    {
        playerInput.OnSpeedInputDown += OnSpeedInputDown;
        playerInput.OnSpeedInputUp += OnSpeedInputUp;
    }


    void Start()
    {
        normalPeriod = gameData.startingPeriod;
        currentPeriod = normalPeriod;
    }

    private void OnDestroy()
    {
        playerInput.OnSpeedInputDown -= OnSpeedInputDown;
        playerInput.OnSpeedInputUp -= OnSpeedInputUp;
    }

    private void OnLevelUp()
    {
        normalPeriod -= gameData.decrementPerLevel;
        currentPeriod = normalPeriod;
    }

    //Shrink the current period
    private void OnSpeedInputDown()
    {
        currentPeriod = gameData.speedPeriod;
    }

    //Return to normal period
    private void OnSpeedInputUp()
    {
        currentPeriod = normalPeriod;
    }
}
