using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Data", menuName = "Score Data")]
public class ScoreData : ScriptableObject
{
    public int pointsWithoutSpeed;
    public int pointsWithSpeed;

    public int pointsPerRow;
}
