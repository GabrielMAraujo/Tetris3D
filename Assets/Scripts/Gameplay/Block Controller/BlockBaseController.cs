using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockBaseController : MonoBehaviour
{
    protected BlockController blockController;
    protected PlayerInput playerInput;
    protected SoundEventEmitter eventEmitter;

    public virtual void Awake()
    {
        
        blockController = GetComponent<BlockController>();
    }

    public virtual void Start()
    {
        playerInput = PlayerInput.instance;
        eventEmitter = SoundEventEmitter.instance;
    }

    public virtual void Update()
    {
    }

    public virtual void OnDestroy()
    {
    }

    //Verify all block tiles movement possibility
    public bool CanBlockMove(Vector2Int moveDirection, int rotation = 0)
    {
        bool successAll = false;

        //All tiles have to be able to move in order to confirm movement
        if (blockController.tiles != null)
        {
            foreach (var tile in blockController.tiles)
            {
                successAll = tile.CanMove(
                    moveDirection,
                    blockController.board.boardData.boardSize,
                    blockController.board,
                    blockController.isRotating,
                    rotation,
                    transform
                );

                //If failed, interrupt loop
                if (!successAll)
                {
                    break;
                }
            }
        }

        return successAll;
    }
}
