using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    //Timer to trigger block descent
    private float timer = 0;
    public Game game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
}
