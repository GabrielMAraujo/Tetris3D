﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    //Timer to trigger block descent
    private float timer = 0;
    public Game game;
    public PlayerInput playerInput;

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
        transform.eulerAngles += new Vector3(0, 0, 90);
    }

    private void OnRotateRight()
    {
        transform.eulerAngles += new Vector3(0, 0, -90);
    }
}
