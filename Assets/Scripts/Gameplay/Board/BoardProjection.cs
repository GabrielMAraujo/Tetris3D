using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Script that handles block's downward future projection
public class BoardProjection : MonoBehaviour
{
    private Board board;

    private GameObject projectionBlock;

    private void Awake()
    {
        board = GetComponent<Board>();
        board.blockController.OnMovement += OnMovement;
        board.blockController.OnNewBlock += OnNewBlock;
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        board.blockController.OnMovement -= OnMovement;
        board.blockController.OnNewBlock -= OnNewBlock;
    }


    private void OnNewBlock(GameObject block, Vector2Int position)
    {
        //Create projection based on new block
        projectionBlock = Instantiate(block, Vector3.zero, Quaternion.identity);
        projectionBlock.name = "Projection";

        //Change color alpha to make block tiles look like a projection

        List<Renderer> renderers = projectionBlock.GetComponentsInChildren<Renderer>().ToList();

        foreach (var renderer in renderers)
        {
            renderer.material.color = new Color(
                renderer.material.color.r,
                renderer.material.color.g,
                renderer.material.color.b,
                board.boardData.projectionAlpha
            );
        }

        //Call first movement update
        OnMovement(projectionBlock.GetComponentsInChildren<BlockTile>().ToList(), position);
    }

    private void OnMovement(List<BlockTile> tiles, Vector2Int position)
    { 
        int highestColumn = 0;

        foreach(var tile in tiles)
        {
            int height = GetTileProjectionHeight(Vector2Int.RoundToInt(tile.transform.position));

            //Stores highest Y value in variable
            if(height > highestColumn)
            {
                highestColumn = height;
            }
        }

        //Move projection in calculated height
        projectionBlock.transform.position = new Vector3(position.x, highestColumn, 0);

    }

    //Gets tile's valid Y coordinate in projected downward collision 
    private int GetTileProjectionHeight(Vector2Int tilePosition)
    {
        int result = 0;

        //Top-down iteration through column until finding block or bound collision
        for(int j = tilePosition.y; j > 0; j--)
        {
            if(board.HasTile(new Vector2Int(tilePosition.x, j)))
            {
                //Adding 1 to put object above collision spot
                result = j + 1;
            }
        }

        return result;
    }
}
