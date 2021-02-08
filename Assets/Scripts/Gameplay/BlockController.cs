using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FMODEvents;

public delegate void BlockTilesCallback(List<Vector2Int> positions, bool speed);

public class BlockController : MonoBehaviour
{
    public event BlockTilesCallback OnBlockSettle;

    public Game game;
    public BlockControllerData blockControllerData;
    public Board board;
    public NextBlock nextBlock;

    private PlayerInput playerInput;
    private List<BlockTile> tiles;
    private GameObject currentBlock;
    //Timer to trigger block descent
    private float timer = 0;
    private bool isRotating = false;
    private bool allowRotation = true;
    private bool hasSpeed = false;

    protected SoundEventEmitter eventEmitter;

    void Awake()
    {
        eventEmitter = SoundEventEmitter.instance;

        playerInput = PlayerInput.instance;

        playerInput.OnHorizontalInputDown += OnHorizontalInputDown;
        playerInput.OnRotateLeftDown += OnRotateLeft;
        playerInput.OnRotateRightDown += OnRotateRight;
        playerInput.OnSwitchDown += OnSwitchDown;
        playerInput.OnSpeedDown += OnSpeedDown;
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

        if(!isRotating && timer > game.currentPeriod)
        {
            //Block rotation to avoid bugs
            allowRotation = false;

            //Try down movement. If it can't be done, settle block
            if (CanBlockMove(new Vector2Int(0, -1)))
            {
                transform.position += Vector3Int.down;
            }
            else
            {
                SettleCurrentBlock();

                //Fetches next block and triggers a new block generation from NextBlock
                GetNextBlock();
                nextBlock.GenerateNewBlock();

                //Resets position to the top
                transform.position = blockControllerData.blockStartingPosition;
            }

            timer = 0;
            allowRotation = true;
        }
    }


    #region Events
    //Move the block horizontally if possible
    private void OnHorizontalInputDown(int direction)
    {
        if(!isRotating && CanBlockMove(new Vector2Int(direction, 0)))
        {
            transform.position += Vector3.right * direction;
        }
    }


    //Rotate block on z-axis
    private void OnRotateLeft()
    {
        CheckAndStartRotation(-1);
    }

    private void OnRotateRight()
    {
        CheckAndStartRotation(1);
    }

    //Changes between current and next tile
    private void OnSwitchDown()
    {
        if (!isRotating)
        {
            //Check if switch won't cause out of bounds or collisions
            List<Vector2Int> possiblePositions = nextBlock.GetPossibleNextBlockCoordinates(Vector2Int.RoundToInt(transform.position));

            //For each coordinate, verify if it would have a block. If so, cancel switch
            bool successAll = false;

            foreach (var possiblePosition in possiblePositions)
            {
                successAll = !board.HasTile(possiblePosition);
                //If failed, interrupt loop
                if (!successAll)
                {
                    break;
                }
            }

            if (successAll)
            {
                GameObject aux = currentBlock;
                GetNextBlock();
                nextBlock.SwitchBlock(aux);
            }
        }
    }

    private void OnSpeedDown()
    {
        hasSpeed = true;
    }
    #endregion

    private void CheckAndStartRotation(int direction)
    {
        //Check if rotation time is bigger than remaining time to go down. If it is, don't rotate
        float remainingTime = game.currentPeriod - timer;
        float rotationTime = (1f / blockControllerData.blockTurningSpeed) * Time.fixedDeltaTime;

        bool enoughTime = remainingTime > rotationTime;

        if (!isRotating && allowRotation && enoughTime)
        {
            int rotation = -90 * direction;

            //Check if rotation is allowed
            bool allowed = CanBlockMove(Vector2Int.zero, rotation);


            if (allowed)
            {
                //Play rotation SFX
                eventEmitter.SetFMODGlobalParameter(
                    FMODEvents.GetString<GlobalParameters>(GlobalParameters.DIRECTION),
                    direction);
                eventEmitter.PlaySFXOneShot(FMODEvents.GetString<SFX>(SFX.ROTATION));

                IEnumerator coroutine = Rotate(rotation);
                StartCoroutine(coroutine);
            }
        }
    }


    //Rotates block in z-axis with specified angle
    private IEnumerator Rotate(float rotationAngle)
    {
        if (!isRotating && allowRotation)
        {
            isRotating = true;
            allowRotation = false;

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
            allowRotation = true;
        }
    }

    //Verify all block tiles movement possibility
    private bool CanBlockMove(Vector2Int moveDirection, int rotation = 0)
    {
        bool successAll = false;

        //All tiles have to be able to move in order to confirm movement
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                successAll = tile.CanMove(moveDirection, game.gameData.boardSize, board, isRotating, rotation, transform);
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
        currentBlock.transform.localScale = new Vector3(1, 1, 1);
        currentBlock.transform.position = transform.position;
        currentBlock.transform.SetParent(transform);
        tiles = currentBlock.GetComponentsInChildren<BlockTile>().ToList();
    }


    //Removes playable components from current blocks
    private void SettleCurrentBlock()
    {
        if (tiles != null)
        {
            //Get tile positions
            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (var tile in tiles)
            {
                positions.Add(Vector2Int.RoundToInt(tile.transform.position));

                //Re-parent block tiles to board
                tile.transform.SetParent(board.blocksContainer.transform);

                //Destroy tile component to leave static tile only
                Destroy(tile);
            }

            OnBlockSettle.Invoke(positions, hasSpeed);
            //Reset speed flag after triggering event
            hasSpeed = false;

            //Destroy block parent game object and references
            Destroy(currentBlock);
            tiles = null;
        }
    }
}
