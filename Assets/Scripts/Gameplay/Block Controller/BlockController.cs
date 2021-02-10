using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void BlockTilesCallback(List<Vector2Int> positions, bool speed);

public class BlockController : MonoBehaviour
{
    public event BlockTilesCallback OnBlockSettle;

    public Game game;
    public BlockControllerData blockControllerData;
    public Board board;
    public NextBlock nextBlock;

    [HideInInspector]
    public List<BlockTile> tiles;

    private PlayerInput playerInput;
    private GameObject currentBlock;

    private BlockHorizontalController horizontalController;
    private BlockVerticalController verticalController;
    private BlockRotationController rotationController;

    //Timer to trigger block descent
    [HideInInspector]
    public float verticalTimer = 0;

    [HideInInspector]
    public bool isRotating = false;
    [HideInInspector]
    public bool allowRotation = true;
    private bool hasSpeed = false;

    void Awake()
    {
        //Generate individual behaviour scripts in the same GameObject
        horizontalController = gameObject.AddComponent<BlockHorizontalController>();
        verticalController = gameObject.AddComponent<BlockVerticalController>();
        rotationController = gameObject.AddComponent<BlockRotationController>();

        playerInput = PlayerInput.instance;

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
        playerInput.OnSwitchDown -= OnSwitchDown;
        playerInput.OnSpeedDown -= OnSpeedDown;
    }

    void Update()
    {
        verticalController.UpdateVertical();
    }


    #region Events

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

    //Get random block from block pool
    private GameObject GetRandomBlock()
    {
        int rand = Random.Range(0, blockControllerData.blockPool.Count - 1);

        return blockControllerData.blockPool[rand];
    }

    //Get next block and place it into current block
    public void GetNextBlock()
    {
        //Reset rotation of this component
        transform.rotation = Quaternion.identity;

        currentBlock = nextBlock.block;
        currentBlock.transform.localScale = new Vector3(1, 1, 1);
        currentBlock.transform.position = transform.position;
        currentBlock.transform.SetParent(transform);
        tiles = currentBlock.GetComponentsInChildren<BlockTile>().ToList();
    }

    //Settle block complete routine
    public void SettleBlockRoutine()
    {
        SettleCurrentBlock();

        //Fetches next block and triggers a new block generation from NextBlock
        GetNextBlock();
        nextBlock.GenerateNewBlock();

        //Resets position to the top
        transform.position = blockControllerData.blockStartingPosition;
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
