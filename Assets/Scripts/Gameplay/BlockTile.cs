using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTile : MonoBehaviour
{
    //Checks if tile won't be out of bounds or in an illegal position in the potential movement
    public bool CanMove(Vector2Int movement, Vector2Int boardSize, Board board, bool isRotating, int rotation = 0, Transform controllerTransform = null)
    {
        Vector2Int possiblePosition = Vector2Int.zero;

        //Horizontal / vertical movement
        if (rotation == 0)
        {
            //If already rotating, block movement
            if(isRotating)
                return false;

            //Transforming value to integer to avoid rounding problems
            possiblePosition = Vector2Int.RoundToInt(transform.position) + movement;
        }
        //Rotation
        else
        {
            //Game object to project possible position after rotation
            GameObject projectedGO = Instantiate(gameObject);

            //Simulating BlockController object, which the rotation is based in
            GameObject parentGO = new GameObject();
            parentGO.transform.position = controllerTransform.position;
            parentGO.transform.rotation = controllerTransform.rotation;
            projectedGO.transform.SetParent(parentGO.transform);

            //Equalizing local position to real tile's and simulating rotation to get possible position
            projectedGO.transform.localPosition = transform.localPosition;
            parentGO.transform.Rotate(new Vector3(0, 0, rotation));

            possiblePosition = Vector2Int.RoundToInt(projectedGO.transform.position);
            print(possiblePosition);

            //Destroy auxilliary game objects
            Destroy(projectedGO);
            Destroy(parentGO);
        }

        //Check if move is legal according to board matrix
        bool legalMove = !board.HasTile(possiblePosition);

        return legalMove;
    }
}
