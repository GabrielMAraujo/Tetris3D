using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void BlockTilesCallback(List<Vector2Int> positions, bool speed);
public delegate void BlockMovementCallback(List<BlockTile> tiles, Vector2Int position);
public delegate void BlockCallback(GameObject block, Vector2Int position);

public class BlockController : MonoBehaviour
{
    public event BlockTilesCallback OnBlockSettle;
    public event BlockMovementCallback OnMovement;
    public event BlockCallback OnNewBlock;

    public Game game;
    public BlockControllerData blockControllerData;
    public Board board;
    public NextBlock nextBlock;

    [HideInInspector]
    public List<BlockTile> tiles;
    [HideInInspector]
    public GameObject currentBlock;

    private PlayerInput playerInput;
    private BlockHorizontalController horizontalController;
    private BlockVerticalController verticalController;
    private BlockRotationController rotationController;
    private BlockSwitchController switchController;

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
        switchController = gameObject.AddComponent<BlockSwitchController>();

        playerInput = PlayerInput.instance;

        playerInput.OnSpeedDown += OnSpeedDown;
    }

    private void Start()
    {
        GameObject prefab = GetRandomBlock();

        currentBlock = Instantiate(prefab, transform.position, Quaternion.identity);
        currentBlock.transform.SetParent(transform);

        tiles = currentBlock.GetComponentsInChildren<BlockTile>().ToList();

        transform.position = blockControllerData.blockStartingPosition;

        OnNewBlock?.Invoke(prefab, Vector2Int.RoundToInt(transform.position));
    }

    private void OnDestroy()
    {
        playerInput.OnSpeedDown -= OnSpeedDown;
    }

    void Update()
    {
        verticalController.UpdateVertical();
    }


    #region Events

    private void OnSpeedDown()
    {
        hasSpeed = true;
    }

    //Triggers OnMovement event from behaviour classes
    public void TriggerOnMovement()
    {
        OnMovement?.Invoke(tiles, Vector2Int.RoundToInt(currentBlock.transform.position));
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
