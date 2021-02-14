using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FMODEvents;

public class BlockRotationController : BlockBaseController
{
    public override void Start()
    {
        base.Start();
        playerInput.OnRotateLeftDown += OnRotateLeftDown;
        playerInput.OnRotateRightDown += OnRotateRightDown;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        playerInput.OnRotateLeftDown -= OnRotateLeftDown;
        playerInput.OnRotateRightDown -= OnRotateRightDown;
    }

    //Rotate block on z-axis
    private void OnRotateLeftDown()
    {
        CheckAndStartRotation(-1);
    }

    private void OnRotateRightDown()
    {
        CheckAndStartRotation(1);
    }

    private void CheckAndStartRotation(int direction)
    {
        //Check if rotation time is bigger than remaining time to go down. If it is, don't rotate
        float remainingTime = blockController.game.currentPeriod - blockController.verticalTimer;
        float rotationTime = (1f / blockController.blockControllerData.blockTurningSpeed) * Time.fixedDeltaTime;

        bool enoughTime = remainingTime > rotationTime;

        if (!blockController.isRotating && blockController.allowRotation && enoughTime)
        {
            int rotation = -90 * direction;

            //Check if rotation is allowed
            bool allowed = CanBlockMove(Vector2Int.zero, rotation);

            if (allowed)
            {
                //Play rotation SFX
                eventEmitter.SetFMODGlobalParameter(
                    FMODEvents.GetString<GlobalParameters>(GlobalParameters.DIRECTION),
                    direction);
                eventEmitter.PlaySFXOneShot(FMODEvents.GetString<SFX>(SFX.ROTATION));

                IEnumerator coroutine = Rotate(rotation);
                StartCoroutine(coroutine);
            }
        }
    }

    //Rotates block in z-axis with specified angle
    private IEnumerator Rotate(float rotationAngle)
    {
        if (!blockController.isRotating && blockController.allowRotation)
        {
            blockController.isRotating = true;
            blockController.allowRotation = false;

            Vector3 targetRotation = transform.rotation * new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z + rotationAngle));

            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

            //Percentage of rotation progress
            float progress = 0f;

            while (progress != 1)
            {
                progress += blockController.blockControllerData.blockTurningSpeed;

                //Overflow protection
                if (progress > 1)
                    progress = 1;

                transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, progress);

                yield return null;
            }
            blockController.isRotating = false;
            blockController.allowRotation = true;

            blockController.TriggerOnMovement();
        }
    }
}
