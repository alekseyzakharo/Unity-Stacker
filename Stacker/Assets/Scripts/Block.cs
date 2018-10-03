using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public float speed = 5.0f;

    enum dir {Left, Right, Down, Up };

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
            else if(direction == dir.Down)
            {
                transform.Translate(new Vector3(0,-speed * Time.deltaTime, 0));
            }
            else
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        string name = collision.gameObject.name;
        if (name == "floor" || name == "ceiling")
            Destroy(gameObject);
        if (name == "right" || name == "left")
        {
            //Game Over***********
            Direction("down");
        }
    }

    void Direction(string blockDirection)
    {
        if (blockDirection == "right")
            direction = dir.Right;
        else if (blockDirection == "left")
            direction = dir.Left;
        else if (blockDirection == "Down")
            direction = dir.Down;
        else
            direction = dir.Up;
    }

    //function that stops this object from transforming
    void Stop()
    {
        STOP = true;
    }

    void Go()
    {
        STOP = false;
    }

}
