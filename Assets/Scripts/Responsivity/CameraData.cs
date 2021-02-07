using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Data", menuName = "Camera Data")]
public class CameraData : ScriptableObject
{
    public Rect landscapeViewportRect;
    public Rect AspectRatio3_4ViewportRect;
    public Rect AspectRatio9_16ViewportRect;

    public Vector3 AspectRatio16_9CameraPosition;
    public Vector3 AspectRatio4_3CameraPosition;

    public Vector3 AspectRatio9_16CameraPosition;
    public Vector3 AspectRatio3_4CameraPosition;
}
