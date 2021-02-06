using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void BlockTilesCallback(List<Vector2Int> positions);

public class BlockController : MonoBehaviour
{
    public event BlockTilesCallback OnBlockSettle;

    //Timer to trigger block descent
    private float timer = 0;
    public Game game;
    public PlayerInput playerInput;
    public BlockControllerData blockControllerData;
    public Board board;
    public NextBlock nextBlock;

    private List<BlockTile> tiles;
    private GameObject currentBlock;

    private bool isRotating = false;

    void Awake()
    {
        playerInput.OnHorizontalInputDown += OnHorizontalInputDown;
        playerInput.OnRotateLeftDown += OnRotateLeft;
        playerInput.OnRotateRightDown += OnRotateRight;
        playerInput.OnSwitchDown += OnSwitchDown;
    }

    private void Start()
    {
        currentBlock = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);
        currentBlock.transform.SetParent(transform);

        tiles = currentBlock.GetComponentsInChildren<BlockTile>().ToList();

        transform.position = blockControllerData.blockStartingPosition;
    }

    private void OnDestroy()
    {
        playerInput.OnHorizontalInputDown -= OnHorizontalInputDown;
        playerInput.OnRotateLeftDown -= OnRotateLeft;
        playerInput.OnRotateRightDown -= OnRotateRight;
        playerInput.OnSwitchDown -= OnSwitchDown;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > game.currentPeriod)
        {
            //Try down movement. If it can't be done, settle block
            if (CanBlockMove(new Vector2Int(0, -1)))
            {
                transform.position += Vector3Int.down;
            }
            else
            {
                if (tiles != null)
                {
                    //Get tile positions
                    List<Vector2Int> positions = new List<Vector2Int>();

                    foreach (var tile in tiles)
                    {
                        positions.Add(Vector2Int.RoundToInt(tile.transform.position));

                        //Re-parent block tiles to board
                        tile.transform.SetParent(board.transform);

                        //Destroy tile component to leave static tile only
                        Destroy(tile);
                    }

                    OnBlockSettle.Invoke(positions);

                    //Destroy block parent game object and references
                    Destroy(currentBlock);
                    tiles = null;

                    //Fetches block and triggers a new block generation from NextBlock
                    GetNextBlock();
                    nextBlock.GenerateNewBlock();

                    //Resets position to the top
                    transform.position = blockControllerData.blockStartingPosition;
                }
            }

            timer = 0;
        }
    }

    //Move the block horizontally if possible
    private void OnHorizontalInputDown(int direction)
    {
        if(CanBlockMove(new Vector2Int(direction, 0)))
        {
            transform.position += Vector3.right * direction;
        }
    }


    //Rotate block on z-axis
    private void OnRotateLeft()
    {
        IEnumerator coroutine = Rotate(90f);
        StartCoroutine(coroutine);
    }

    private void OnRotateRight()
    {
        IEnumerator coroutine = Rotate(-90f);
        StartCoroutine(coroutine);
    }

    //Changes between current and next tile
    private void OnSwitchDown()
    {
        GameObject aux = currentBlock;
        GetNextBlock();
        nextBlock.SwitchBlock(aux);
    }

    //Rotates block in z-axis with specified angle
    private IEnumerator Rotate(float rotationAngle)
    {
        if (!isRotating)
        {
            isRotating = true;

            Vector3 targetRotation = transform.rotation * new Vector3(0, 0, Mathf.Round(transform.eulerAngles.z + rotationAngle));

            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

            //Percentage of rotation progress
            float progress = 0f;

            while (progress != 1)
            {
                progress += blockControllerData.blockTurningSpeed;

                //Overflow protection
                if (progress > 1)
                    progress = 1;

                transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, progress);

                yield return null;
            }
            isRotating = false;
        }
    }

    //Verify all block tiles movement possibility
    private bool CanBlockMove(Vector2Int moveDirection)
    {
        bool successAll = false;

        //All tiles have to be able to move in order to confirm movement
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                successAll = tile.CanMove(moveDirection, game.gameData.boardSize);
                //If failed, interrupt loop
                if (!successAll)
                {
                    break;
                }
            }
        }

        return successAll;
    }

    //Get random block from block pool
    private GameObject GetRandomBlock()
    {
        int rand = Random.Range(0, blockControllerData.blockPool.Count - 1);

        return blockControllerData.blockPool[rand];
    }

    //Get next block and place it into current block
    private void GetNextBlock()
    {
        currentBlock = nextBlock.block;
        currentBlock.transform.position = transform.position;
        currentBlock.transform.SetParent(transform);
        tiles = currentBlock.GetComponentsInChildren<BlockTile>().ToList();
    }
}
