using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlock : MonoBehaviour
{
    public BlockControllerData blockControllerData;
    [HideInInspector]
    public GameObject block;

    //GameObject container that helps to center off-centered pivot object (such as blocks) for exhibition
    private GameObject blockContainer;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate empty container
        blockContainer = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        blockContainer.name = "Next Block Container";
        blockContainer.transform.SetParent(transform);

        GenerateNewBlock();
    }

    public void SwitchBlock(GameObject go)
    {
        block = go;
        go.transform.position = transform.position;
        go.transform.SetParent(blockContainer.transform);

        //Ajust container according to block center
        var offset = FindBlockCenter(go);
        if (offset != null)
        {
            blockContainer.transform.position -= new Vector3(offset.Value.x, offset.Value.y);
        }
    }

    //Generates a new block drafted from the block pool
    public void GenerateNewBlock()
    {
        block = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);
        block.transform.SetParent(blockContainer.transform);

        //Ajust container according to block center
        var offset = FindBlockCenter(block);
        if(offset != null)
        {
            blockContainer.transform.position -= new Vector3(offset.Value.x, offset.Value.y);
        }
    }

    //Get random block from block pool
    private GameObject GetRandomBlock()
    {
        int rand = Random.Range(0, blockControllerData.blockPool.Count - 1);

        return blockControllerData.blockPool[rand];
    }

    //Finds block true center to help piece exhibition
    private Vector2? FindBlockCenter(GameObject block)
    {
        var tiles = block.GetComponentsInChildren<BlockTile>();

        if (tiles == null)
            return null;

        //Stores minimum and maximum values of each coordinate
        float xMin = 0f, xMax = 0f, yMin = 0f, yMax = 0f;

        //Collect x and y max and min coordinates
        foreach (var tile in tiles)
        {
            float xCoord = tile.transform.localPosition.x;
            float yCoord= tile.transform.localPosition.y;

            xMin = xCoord < xMin ? xCoord : xMin;
            yMin = yCoord < yMin ? yCoord : yMin;

            xMax = xCoord > xMax ? xCoord : xMax;
            yMax = yCoord > yMax ? yCoord : yMax;
        }

        //Getting center point with total width and height

        float width = xMax - xMin;
        float height = yMax - yMin;

        float xCenter = xMin + (width / 2f);
        float yCenter = yMin + (height / 2f);

        return new Vector2(xCenter, yCenter);
    }
}
