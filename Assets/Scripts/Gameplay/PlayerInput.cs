using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Callback for input events
public delegate void HorizontalInputCallback(int direction);

public class PlayerInput : MonoBehaviour
{
    public event HorizontalInputCallback OnHorizontalInputDown;

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
    }
}
