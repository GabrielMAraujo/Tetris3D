using System.Collections;
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
    }

    private void OnDestroy()
    {
        playerInput.OnHorizontalInputDown -= OnHorizontalInputDown;
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
}
