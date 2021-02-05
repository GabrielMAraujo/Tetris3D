using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameData gameData;

    //Period to trigger block descent
    [HideInInspector]
    public float currentPeriod = 0;

    void Start()
    {
        currentPeriod = gameData.startingPeriod;
    }

    private void OnLevelUp()
    {
        currentPeriod -= gameData.decrementPerLevel;
    }
}
