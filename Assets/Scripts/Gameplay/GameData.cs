using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public Vector2Int boardSize;

    public float startingPeriod;
    public float decrementPerLevel;
    public float speedPeriod;
}
