using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Game game;
    public BlockController blockController;

    private int[,] boardMatrix;

    private void Awake()
    {
        blockController.OnBlockSettle += OnBlockSettle;
    }

    private void Start()
    {
        //Create int array according to board size
        boardMatrix = new int[game.gameData.boardSize.x, game.gameData.boardSize.y];
    }

    private void OnDestroy()
    {
        blockController.OnBlockSettle -= OnBlockSettle;
    }

    //Store block tiles in boolean matrix
    private void OnBlockSettle(List<Vector2Int> positions)
    {
        foreach(var position in positions)
        {
            //Warn editor if an illegal settlement has been made
            if (boardMatrix[position.x, position.y] == 1)
                Debug.LogWarning("Tile settled in an illegal place!");

            boardMatrix[position.x, position.y] = 1;
        }
        PrintBoardMatrix();
    }

    //Checks if coordinate has a tile in board matrix
    public bool HasTile(Vector2Int position)
    {
        return boardMatrix[position.x, position.y] == 1;
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
