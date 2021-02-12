using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Data", menuName = "Board Data")]
public class BoardData : ScriptableObject
{
    public Vector2Int boardSize;
    [Range(0f, 1f)]
    public float projectionAlpha;
}
