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
        playerInput.OnHorizontalInputDown += OnHorizontalInputDown;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        playerInput.OnHorizontalInputDown -= OnHorizontalInputDown;
    }

    //Move the block horizontally if possible
    private void OnHorizontalInputDown(int direction)
    {
        if (!blockController.isRotating && CanBlockMove(new Vector2Int(direction, 0)))
        {
            transform.position += Vector3.right * direction;
            blockController.TriggerOnMovement();
        }
    }
}
