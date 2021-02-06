﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Game game;
    public BlockController blockController;

    //Game object which parents settled blocks
    [HideInInspector]
    public GameObject blocksContainer;

    private int[,] boardMatrix;

    private void Awake()
    {
        blockController.OnBlockSettle += OnBlockSettle;
    }

    private void Start()
    {
        //Create int array according to board size
        boardMatrix = new int[game.gameData.boardSize.x, game.gameData.boardSize.y];
        blocksContainer = new GameObject();
        blocksContainer.name = "Settled Blocks Container";
        blocksContainer.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        blockController.OnBlockSettle -= OnBlockSettle;
    }

    //Store block tiles in boolean matrix
    private void OnBlockSettle(List<Vector2Int> positions)
    {
        List<int> possibleRows = new List<int>();

        foreach(var position in positions)
        {
            //Warn editor if an illegal settlement has been made
            if (boardMatrix[position.x, position.y] == 1)
                Debug.LogWarning("Tile settled in an illegal place!");

            //Store tiles' Y coordinates to check for row clears
            if (!possibleRows.Contains(position.y))
                possibleRows.Add(position.y);

            boardMatrix[position.x, position.y] = 1;
        }

        CheckForClearedRows(possibleRows);

        PrintBoardMatrix();
    }

    //Checks if coordinate has a tile in board matrix
    public bool HasTile(Vector2Int position)
    {
        return boardMatrix[position.x, position.y] == 1;
    }

    private void CheckForClearedRows(List<int> possibleRows)
    {
        //Accounts for cleared rows, to offset other row values accondingly
        int clearedRows = 0;

        foreach(int row in possibleRows)
        {
            int effectiveRow = row - clearedRows;

            if (IsRowFull(effectiveRow))
            {
                RemoveRowFromMatrix(effectiveRow);
                RemoveRowFromBoard(effectiveRow);
                clearedRows++;
            }
        }
    }

    //Returns true if row is full
    private bool IsRowFull(int row)
    {
        bool successAll = false;
        //Iterate through row in board matrix to see if all elements are 1. If a 0 is found, interrupt loop
        for (int i = 0; i < game.gameData.boardSize.x; i++)
        {
            successAll = HasTile(new Vector2Int(i, row));

            if (!successAll)
                break;
        }

        return successAll;
    }

    //Removes row from board matrix
    private void RemoveRowFromMatrix(int row)
    {
        //Start from row to be removed, and goes up until the second last row
        for(int j = row; j < game.gameData.boardSize.y - 1; j++)
        {
            for (int i = 0; i < game.gameData.boardSize.x; i++) {
                //Store upper row element value in lower row
                boardMatrix[i,j] = boardMatrix[i,j+1];
            }
        }

        //For the last row, fill it with zeroes
        for(int i = 0; i < game.gameData.boardSize.x; i++)
        {
            boardMatrix[i, game.gameData.boardSize.y - 1] = 0;
        }
    }

    //Remove actual tile game objects from scene
    private void RemoveRowFromBoard(int row)
    {
        List<Transform> tiles = blocksContainer.GetComponentsInChildren<Transform>().ToList();
        //As GetComponentsInChildren looks in the parent as well, ignore first element, which is the container itself
        tiles.RemoveAt(0);

        //Get tiles from row to be removed
        List<Transform> rowTiles = tiles.Where(t => Mathf.RoundToInt(t.position.y) == row).ToList();

        //Destroy all tiles from row
        foreach (var tile in rowTiles)
        {
            Destroy(tile.gameObject);
        }

        //Get all tiles from rows bigger than the removed row
        List<Transform> biggerRowTiles = tiles.Where(t => Mathf.RoundToInt(t.position.y) > row).ToList();

        //Lower a row in each bigger row tile
        foreach (var tile in biggerRowTiles)
        {
            tile.position += Vector3.down;
        }

    }

    //DEBUG ONLY - prints board matrix in log
    private void PrintBoardMatrix()
    {
        int rowLength = boardMatrix.GetLength(0);
        int colLength = boardMatrix.GetLength(1);

        string arrayString = "";
        for (int j = colLength - 1; j >= 0; j--)
        {
            for (int i = 0; i < rowLength; i++)
            {
                arrayString += string.Format("{0} ", boardMatrix[i, j]);
            }
            arrayString += System.Environment.NewLine + System.Environment.NewLine;
        }

        Debug.Log(arrayString);
    }
}
