using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {

    public GameObject prefabBlock;

    public GameObject newBlock;
    public GameObject oldBlock;

    string direction;

    private float xMin = -2.0f;
    private float xMax = 2.0f;

    private float zMin = -2.0f;
    private float zMax = 2.0f;

    private float spawnPoint = -15.0f;

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
        if (Input.GetMouseButtonDown(0))
        {
            newBlock.SendMessage("Stop");
            checkPosition();
            translateBlocksDown();
            spawnBlock();
        }
    }

    private void spawnBlock()
    {
        //get the block position
        Vector3 blockVec = blockPosition(direction);

        newBlock = Instantiate(prefabBlock, blockVec, Quaternion.identity);
        newBlock.transform.localScale = blockScale(xMax,xMin,zMax,zMin);
        newBlock.SendMessage("Direction", direction);
        newBlock.SendMessage("Go");

        newBlock.transform.parent = GameObject.Find("Stacks").transform;

        if (direction == "right")
            direction = "left";
        else
            direction = "right";
    }

    private Vector3 blockPosition(string direction)
    {
        Vector3 vec = blockPositionCenter(xMax, xMin, zMax, zMin);
        if (direction == "left")
            vec.x = spawnPoint;
        else
            vec.z = spawnPoint;
        return vec;
    }

    private Vector3 blockPositionCenter(float x_max, float x_min, float z_max, float z_min)
    {
        float xCenter = (x_max + x_min) / 2;
        float zCenter = (z_max + z_min) / 2;
        return new Vector3(xCenter, 0, zCenter);
    }

    private Vector3 blockScale(float x_max, float x_min, float z_max, float z_min)
    {
        float xSize = x_max - x_min;
        float zSize = z_max - z_min;
        return new Vector3(xSize, 1, zSize);
    }

    private void checkPosition()
    {
        Vector3 blockAPos;
        Vector3 blockASca;
        Vector3 blockBPos = new Vector3(1, 1, 1);
        Vector3 blockBSca = new Vector3(1, 1, 1);

        float newBlock_xMax = newBlock.transform.position.x + (newBlock.transform.localScale.x / 2);
        float newBlock_xMin = newBlock.transform.position.x - (newBlock.transform.localScale.x / 2);
        float newBlock_zMax = newBlock.transform.position.z + (newBlock.transform.localScale.z / 2);
        float newBlock_zMin = newBlock.transform.position.z - (newBlock.transform.localScale.z / 2);

        //check if you completely missed the block below
        if (newBlock_xMax < xMin || newBlock_xMin > xMax || newBlock_zMax < zMin || newBlock_zMin > zMax)
        {
            newBlock.SendMessage("Direction", "Down");
            newBlock.SendMessage("Go");

            //GAME OVER *************
            return;
        }

        if (newBlock_xMax < xMax && newBlock_xMax > xMin)
        {
            xMax = newBlock_xMax;
            blockBPos = blockPositionCenter(xMin, newBlock_xMin, zMax, zMin);
            blockBSca = blockScale(xMin, newBlock_xMin, zMax, zMin);
        }
        else if(newBlock_xMin < xMax && newBlock_xMin > xMin)
        {
            xMin = newBlock_xMin;
            blockBPos = blockPositionCenter(newBlock_xMax, xMax, zMax, zMin);
            blockBSca = blockScale(newBlock_xMax, xMax, zMax, zMin);
        }
        if (newBlock_zMax < zMax && newBlock_zMax > zMin)
        {
            zMax = newBlock_zMax;
            blockBPos = blockPositionCenter(xMax, xMin, zMin, newBlock_zMin);
            blockBSca = blockScale(xMax, xMin, zMin, newBlock_zMin);
        }
        else if (newBlock_zMin < zMax && newBlock_zMin > zMin)
        {
            zMin = newBlock_zMin;
            blockBPos = blockPositionCenter(xMax, xMin, newBlock_zMax, zMax);
            blockBSca = blockScale(xMax, xMin, newBlock_zMax, zMax);
        }
        blockAPos = blockPositionCenter(xMax, xMin, zMax, zMin);
        blockASca = blockScale(xMax, xMin, zMax, zMin);

        Destroy(newBlock);
        newBlock = Instantiate(prefabBlock, blockAPos, Quaternion.identity);
        newBlock.transform.localScale = blockASca;
        newBlock.SendMessage("Stop");
        newBlock.transform.parent = GameObject.Find("Stacks").transform;

        GameObject extra = Instantiate(prefabBlock, blockBPos, Quaternion.identity);
        extra.transform.localScale = blockBSca;
        extra.SendMessage("Direction", "Up");
        extra.transform.parent = GameObject.Find("FallingPieces").transform;

        oldBlock = newBlock;
    }

    void translateBlocksDown()
    {
        Transform stacks = GameObject.Find("Stacks").GetComponentInChildren<Transform>();
        foreach(Transform child in stacks)
        {
            child.transform.Translate(new Vector3(0, -1, 0));
        }
    }

}
