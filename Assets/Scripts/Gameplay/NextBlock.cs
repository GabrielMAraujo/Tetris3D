using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlock : MonoBehaviour
{
    public BlockControllerData blockControllerData;
    [HideInInspector]
    public GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        block = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);
        block.transform.SetParent(transform);
    }

    //Get random block from block pool
    private GameObject GetRandomBlock()
    {
        int rand = Random.Range(0, blockControllerData.blockPool.Count - 1);

        return blockControllerData.blockPool[rand];
    }

    public void SwitchBlock(GameObject go)
    {
        block = go;
        go.transform.position = transform.position;
        go.transform.SetParent(transform);
    }
}
