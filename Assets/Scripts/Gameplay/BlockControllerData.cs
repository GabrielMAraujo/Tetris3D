using UnityEngine;

[CreateAssetMenu(fileName = "Block Controller Data", menuName = "Block Controller Data")]
public class BlockControllerData : ScriptableObject
{
    [Range(0f, 0.5f)]
    public float blockTurningSpeed;
}
