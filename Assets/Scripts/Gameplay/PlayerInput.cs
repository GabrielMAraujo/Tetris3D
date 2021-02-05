using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Callbacks for input events
public delegate void HorizontalInputCallback(int direction);
public delegate void InputCallback();

public class PlayerInput : MonoBehaviour
{
    public event HorizontalInputCallback OnHorizontalInputDown;
    public event InputCallback OnSpeedDown, OnSpeedUp, OnRotateLeftDown, OnRotateRightDown;

    // Update is called once per frame
    void Update()
    {
        //Detecting input
        if (Input.GetButtonDown("Left"))
        {
            OnHorizontalInputDown.Invoke(-1);
        }

        if (Input.GetButtonDown("Right"))
        {
            OnHorizontalInputDown.Invoke(1);
        }

        if (Input.GetButtonDown("Speed"))
        {
            OnSpeedDown.Invoke();
        }

        if (Input.GetButtonUp("Speed"))
        {
            OnSpeedUp.Invoke();
        }

        if (Input.GetButtonDown("RotateLeft"))
        {
            OnRotateLeftDown.Invoke();
        }

        if (Input.GetButtonDown("RotateRight"))
        {
            OnRotateRightDown.Invoke();
        }
    }
}
