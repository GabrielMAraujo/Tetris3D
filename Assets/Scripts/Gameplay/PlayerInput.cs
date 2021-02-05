using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Callbacks for input events
public delegate void HorizontalInputCallback(int direction);
public delegate void InputCallback();

public class PlayerInput : MonoBehaviour
{
    public event HorizontalInputCallback OnHorizontalInputDown;
    public event InputCallback OnSpeedInputDown, OnSpeedInputUp;

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
            OnSpeedInputDown.Invoke();
        }

        if (Input.GetButtonUp("Speed"))
        {
            OnSpeedInputUp.Invoke();
        }
    }
}
