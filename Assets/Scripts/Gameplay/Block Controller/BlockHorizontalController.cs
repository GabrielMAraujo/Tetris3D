using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHorizontalController : BlockBaseController
{

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    //Move the block horizontally if possible
    public override void OnHorizontalInputDown(int direction)
    {
        base.OnHorizontalInputDown(direction);

        if (!blockController.isRotating && CanBlockMove(new Vector2Int(direction, 0)))
        {
            transform.position += Vector3.right * direction;
        }
    }
}
