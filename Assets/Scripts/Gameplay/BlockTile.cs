using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTile : MonoBehaviour
{
    //Checks if tile won't be out of bounds in the potential movement
    public bool CanMove(Vector2Int movement, Vector2Int boardSize)
    {
        //Building Rect with board dimensions
        Rect boardRect = new Rect(0, 0, boardSize.x, boardSize.y);

        //Transforming value to integer to avoid rounding problems
        Vector2Int possiblePosition = Vector2Int.RoundToInt(transform.position) + movement;

        return boardRect.Contains(possiblePosition);
    }
}
