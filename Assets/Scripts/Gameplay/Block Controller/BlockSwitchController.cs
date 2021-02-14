using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSwitchController : BlockBaseController
{
    public override void Start()
    {
        base.Start();
        playerInput.OnSwitchDown += OnSwitchDown;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        playerInput.OnSwitchDown -= OnSwitchDown;
    }

    //Changes between current and next tile
    private void OnSwitchDown()
    {
        if (!blockController.isRotating)
        {
            //Check if switch won't cause out of bounds or collisions
            List<Vector2Int> possiblePositions = blockController.nextBlock.GetPossibleNextBlockCoordinates(Vector2Int.RoundToInt(transform.position));

            //For each coordinate, verify if it would have a block. If so, cancel switch
            bool successAll = false;

            foreach (var possiblePosition in possiblePositions)
            {
                successAll = !blockController.board.HasTile(possiblePosition);
                //If failed, interrupt loop
                if (!successAll)
                {
                    break;
                }
            }

            if (successAll)
            {
                GameObject aux = blockController.currentBlock;
                blockController.GetNextBlock();
                blockController.nextBlock.SwitchBlock(aux);
                blockController.TriggerOnNewBlock();
            }
        }
    }
}
