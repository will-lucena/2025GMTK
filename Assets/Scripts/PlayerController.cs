using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerUnit _playerUnit;
    private ActionsManager _actionsManager;

    private void Awake()
    {
        _actionsManager = ActionsManager.Instance;
    }

    private void Start()
    {
        Tile.OnTileClicked += OnTileSelected;
    }

    private void Update()
    {
        HandleKeyboardInputs();
    }

    private void HandleKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.W)) _actionsManager.InvokeCommand(CreateMoveCommand(new Vector2Int(0, 1)));
        if (Input.GetKeyDown(KeyCode.S)) _actionsManager.InvokeCommand(CreateMoveCommand(new Vector2Int(0, -1)));
        if (Input.GetKeyDown(KeyCode.A)) _actionsManager.InvokeCommand(CreateMoveCommand(new Vector2Int(-1, 0)));
        if (Input.GetKeyDown(KeyCode.D)) _actionsManager.InvokeCommand(CreateMoveCommand(new Vector2Int(1, 0)));
        if (Input.GetKeyDown(KeyCode.Space)) _actionsManager.InvokeCommand(CreateCallCommand());
    }

    private MoveCommand CreateMoveCommand(Vector2Int movementVector)
    {
        Tile targetTile = FindTile(GridManager.Instance.playerTile, movementVector);
        return new MoveCommand(_playerUnit, targetTile, GridManager.Instance);
    }

    private Tile FindTile(Tile tile, Vector2Int movementVector)
    {
        return GridManager.Instance.GetTileAtPosition(new Vector2Int(tile.x + movementVector.x, tile.y + movementVector.y));
    }

    private CallCommand CreateCallCommand()
    {
        return new CallCommand(_playerUnit);
    }

    private void OnTileSelected(Tile targetTile)
    {
        _actionsManager.InvokeCommand(CreateThrowCommand(targetTile));
    }

    private ThrowCommand CreateThrowCommand(Tile targetTile)
    {
        return new ThrowCommand(_playerUnit, _playerUnit.activeWeapon, targetTile);
    }

}
