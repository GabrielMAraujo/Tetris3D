using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    //Timer to trigger block descent
    private float timer = 0;
    public Game game;
    public PlayerInput playerInput;
    public BlockControllerData blockControllerData;

    private bool isRotating = false;

    void Awake()
    {
        playerInput.OnHorizontalInputDown += OnHorizontalInputDown;
        playerInput.OnRotateLeftDown += OnRotateLeft;
        playerInput.OnRotateRightDown += OnRotateRight;
    }

    private void OnDestroy()
    {
        playerInput.OnHorizontalInputDown -= OnHorizontalInputDown;
        playerInput.OnRotateLeftDown -= OnRotateLeft;
        playerInput.OnRotateRightDown -= OnRotateRight;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > game.currentPeriod)
        {
            //Try block descent and reset time
            if(transform.position.y > 0)
            {
                transform.position += Vector3.down;
            }

            timer = 0;
        }
    }

    //Move the block horizontally if possible
    private void OnHorizontalInputDown(int direction)
    {
        transform.position += Vector3.right * direction;
    }


    //Rotate block on z-axis
    private void OnRotateLeft()
    {
        IEnumerator coroutine = Rotate(90f);
        StartCoroutine(coroutine);
    }

    private void OnRotateRight()
    {
        IEnumerator coroutine = Rotate(-90f);
        StartCoroutine(coroutine);
    }

    //Rotates block in z-axis with specified angle
    private IEnumerator Rotate(float rotationAngle)
    {
        if (!isRotating)
        {
            isRotating = true;

            Vector3 targetRotation = transform.rotation * new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z + rotationAngle));

            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

            //Percentage of rotation progress
            float progress = 0f;

            while (progress != 1)
            {
                progress += blockControllerData.blockTurningSpeed;

                //Overflow protection
                if (progress > 1)
                    progress = 1;

                transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, progress);

                yield return null;
            }
            isRotating = false;
        }
    }
}
