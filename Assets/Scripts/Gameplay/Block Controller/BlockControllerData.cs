using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block Controller Data", menuName = "Block Controller Data")]
public class BlockControllerData : ScriptableObject
{
    public List<GameObject> blockPool;

    [Range(0f, 0.5f)]
    public float blockTurningSpeed;
    public Vector2 blockStartingPosition;

    public Vector3 nextBlockScale;
}
