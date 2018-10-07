using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {
    public Text scoreText;
    public Text tol;
    public GameObject prefabBlock;

    //block
    public float tolerance = 0.2f;
    private GameObject newBlock;
    private GameObject oldBlock;
    private string direction;

    //color
    private Color color;
    private float r = 20;
    private float g = 20;
    private float b = 20;

    //block position 
    private float xMin = -2.0f;
    private float xMax = 2.0f;
    private float zMin = -2.0f;
    private float zMax = 2.0f;

    private float spawnPoint = -15.0f;

    private float score = 0;

    // Use this for initialization
    void Start () {
        color = new Color(0, 0, 0);
        oldBlock = GameObject.Find("InitialBlock");
        oldBlock.GetComponent<Renderer>().material.color = color;
        scoreText.text = score.ToString();


        direction = "right";
        oldBlock.SendMessage("Stop");
        //spawn intial block
        updateColor();
        spawnBlock();
        

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            newBlock.SendMessage("Stop");
            checkPosition();
            scoreText.text = (score+=100).ToString();
            oldBlock = newBlock;
            translateBlocksDown();
            updateColor();
            spawnBlock();

        }
    }

    private void spawnBlock()
    {
        newBlock = instantiateBlock(blockPosition(direction), blockScale(xMax, xMin, zMax, zMin), direction, true, GameObject.Find("Stacks").transform);
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
        float newBlock_xMax = newBlock.transform.position.x + (newBlock.transform.localScale.x / 2);
        float newBlock_xMin = newBlock.transform.position.x - (newBlock.transform.localScale.x / 2);
        float newBlock_zMax = newBlock.transform.position.z + (newBlock.transform.localScale.z / 2);
        float newBlock_zMin = newBlock.transform.position.z - (newBlock.transform.localScale.z / 2);

        //check if you completely missed the block below
        if (newBlock_xMax < xMin || newBlock_xMin > xMax || newBlock_zMax < zMin || newBlock_zMin > zMax)
        {
            newBlock.SendMessage("Direction", "Down");
            newBlock.SendMessage("Go");

            //********************GAME OVER *************
            return;
        }
        //at this point we know that the block did not miss so check againts tolerance to see if user should get perfect position
        if(compareOldBlockNewBlockTolerance())
        {
            newBlock.transform.position = new Vector3(oldBlock.transform.position.x, oldBlock.transform.position.y+1, oldBlock.transform.position.z);
        }
        else
        {
            Vector3 blockBPos = new Vector3(1, 1, 1);
            Vector3 blockBSca = new Vector3(1, 1, 1);

            //logic for figuring out falling block position/scale
            fallingBlockPositionScale(newBlock_xMax, newBlock_xMin, newBlock_zMax, newBlock_zMin, ref blockBPos, ref blockBSca);

            Vector3 blockAPos = blockPositionCenter(xMax, xMin, zMax, zMin);
            Vector3 blockASca = blockScale(xMax, xMin, zMax, zMin);

            Destroy(newBlock);
            newBlock = instantiateBlock(blockAPos, blockASca, direction, false, GameObject.Find("Stacks").transform);

            instantiateBlock(blockBPos, blockBSca, "Up", true, GameObject.Find("FallingPieces").transform);
        }
    }

    private void fallingBlockPositionScale(float new_Xmax, float new_Xmin, float new_Zmax, float new_Zmin, ref Vector3 blockPos, ref Vector3 blockSca)
    {
        if (new_Xmax < xMax && new_Xmax > xMin)
        {
            xMax = new_Xmax;
            blockPos = blockPositionCenter(xMin, new_Xmin, zMax, zMin);
            blockSca = blockScale(xMin, new_Xmin, zMax, zMin);
        }
        else if (new_Xmin < xMax && new_Xmin > xMin)
        {
            xMin = new_Xmin;
            blockPos = blockPositionCenter(new_Xmax, xMax, zMax, zMin);
            blockSca = blockScale(new_Xmax, xMax, zMax, zMin);
        }
        if (new_Zmax < zMax && new_Zmax > zMin)
        {
            zMax = new_Zmax;
            blockPos = blockPositionCenter(xMax, xMin, zMin, new_Zmin);
            blockSca = blockScale(xMax, xMin, zMin, new_Zmin);
        }
        else if (new_Zmin < zMax && new_Zmin > zMin)
        {
            zMin = new_Zmin;
            blockPos = blockPositionCenter(xMax, xMin, new_Zmax, zMax);
            blockSca = blockScale(xMax, xMin, new_Zmax, zMax);
        }
    }

    private GameObject instantiateBlock(Vector3 position, Vector3 scale, string direction, bool go, Transform parent)
    {
        GameObject obj = Instantiate(prefabBlock, position, Quaternion.identity);
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().material.color = color;
        if (go)
            obj.SendMessage("Go");
        else
            obj.SendMessage("Stop");
        obj.SendMessage("Direction", direction);
        obj.transform.parent = parent;
        return obj;
    }

    //if tolerance is greater than allowed 
    private bool compareOldBlockNewBlockTolerance()
    {
        float ox = oldBlock.transform.position.x;
        float oz = oldBlock.transform.position.z;
        float nx = newBlock.transform.position.x;
        float nz = newBlock.transform.position.z;

        tol.text = Math.Abs((ox - nx) + (oz - nz)).ToString() + " : " + tolerance.ToString() ;

        if (Math.Abs((ox - nx) + (oz - nz)) < tolerance)
        {
            return true;
        }
        return false;
    }

    private void translateBlocksDown()
    {
        Transform stacks = GameObject.Find("Stacks").GetComponentInChildren<Transform>();
        foreach(Transform child in stacks)
        {
            child.transform.Translate(new Vector3(0, -1, 0));
        }
    }

    private void updateColor()
    {
        if(color.r < 1)
        {
            if ((color.r + (r / 255)) > 1)
                color.r = 1;
            else
                color.r += (r / 255);
        }
        else if(color.g < 1)
        {
            if ((color.g + (g / 255)) > 1)
                color.g = 1;
            else
                color.g += (g / 255);
        }
        else if (color.b < 1)
        {
            if ((color.b + (b / 255)) > 1)
                color.b = 1;
            else
                color.b += (b / 255);
        }
    }
}
