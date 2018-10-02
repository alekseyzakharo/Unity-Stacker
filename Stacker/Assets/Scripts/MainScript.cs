using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {

    public GameObject prefabBlock;

    public GameObject newBlock;
    public GameObject oldBlock;

    string direction;

    Vector3 leftVec = new Vector3(-15, 0, 0);
    Vector3 rightVec = new Vector3(0, 0, -15);

    // Use this for initialization
    void Start () {
        direction = "right";
        oldBlock.SendMessage("Stop");
        //spawn intial block
        spawnBlock();
        //newBlock.SendMessage("Stop");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawnBlock()
    {
        Vector3 blockVec = leftVec;
        if (direction == "right")
            blockVec = rightVec;
        newBlock = Instantiate(prefabBlock, blockVec, Quaternion.identity);
        newBlock.SendMessage("Direction", direction);

        newBlock.transform.parent = GameObject.Find("Stacks").transform;

        if (direction == "right")
            direction = "left";
        else
            direction = "right";
    }

    void translateBlocksDown()
    {

    }

}
