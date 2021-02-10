using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVerticalController : BlockBaseController
{
    //Update to be call by BlockController
    public void UpdateVertical()
    {
        blockController.verticalTimer += Time.deltaTime;

        //Check for timer value
        if (!blockController.isRotating && blockController.verticalTimer > blockController.game.currentPeriod)
        {
            //Block rotation to avoid bugs
            blockController.allowRotation = false;

            //Try down movement. If it can't be done, settle block
            if (CanBlockMove(new Vector2Int(0, -1)))
            {
                transform.position += Vector3Int.down;
            }
            else
            {
                blockController.SettleBlockRoutine();
            }

            //Reset timer and rotation values
            blockController.verticalTimer = 0;
            blockController.allowRotation = true;
        }
    }
}
