using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Script that handles block's downward future projection
public class BoardProjection : MonoBehaviour
{
    private Board board;

    private GameObject projectionBlock;

    //Stores the tile in which the projection will be based
    private BlockTile collisionTile;

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
        //Destroy current projection if exists
        if(projectionBlock != null)
        {
            Destroy(projectionBlock);
        }

        //Create projection based on new block
        projectionBlock = Instantiate(block, Vector3.zero, Quaternion.identity);
        projectionBlock.name = "Projection";

        //Call first movement update
        OnMovement(board.blockController.gameObject);

        //Remove script from projection tiles
        foreach (var blockTile in projectionBlock.GetComponentsInChildren<BlockTile>().ToList())
        {
            Destroy(blockTile);
        }

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
    }

    private void OnMovement(GameObject controller)
    {
        //Maintain position and rotation from original block
        projectionBlock.transform.position = controller.transform.position;
        projectionBlock.transform.rotation = controller.transform.rotation;

        List<BlockTile> tiles = controller.GetComponentsInChildren<BlockTile>().ToList();

        //Iterate downwards from original postion until collide with something
        int initialHeight = Mathf.RoundToInt(projectionBlock.transform.position.y);
        for (int j = initialHeight; j >= 0; j--)
        {
            Vector2Int moveAmount = Vector2Int.down + (Vector2Int.down * (initialHeight - j));

            //If projection can move down without colliding with something, do so
            if(CanProjectionMove(tiles, moveAmount))
            {
                projectionBlock.transform.position += Vector3.down;
            }
            //If not, interrupt loop and exit, projection got as far as it could
            else
            {
                break;
            }
        }

    }

    //Verify all projection tiles movement possibility
    private bool CanProjectionMove(List<BlockTile> tiles, Vector2Int moveDirection)
    {
        bool successAll = false;

        //All tiles have to be able to move in order to confirm movement
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                successAll = !board.HasTile(Vector2Int.RoundToInt(tile.transform.position) + moveDirection);

                //If failed, interrupt loop
                if (!successAll)
                {
                    break;
                }
            }
        }

        return successAll;
    }
}
