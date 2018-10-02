using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public float speed = 5.0f;

    enum dir {Left, Right, Down };

    private bool STOP = false;
    private dir direction;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //make sure we can move
        if (!STOP)
        {
            //check which direction we are coming from?
            if (direction == dir.Right)
            {
                transform.Translate(new Vector3(0,0,speed*Time.deltaTime));
            }
            else if(direction == dir.Left)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            else
            {
                transform.Translate(new Vector3(0,speed * Time.deltaTime, 0));
            }
        }
	}

    void Direction(string blockDirection)
    {
        if (blockDirection == "right")
            direction = dir.Right;
        else if (blockDirection == "left")
            direction = dir.Left;
        else
            direction = dir.Down;
    }

    void Size(int width, int depth)
    {

    }

    //function that stops this object from transforming
    void Stop()
    {
        STOP = true;
    }

}
