using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveCommand : ICommand
{
    private PlayerUnit _player;
    private Tile _targetTile;
    private GridManager _gridManager;

    public MoveCommand(PlayerUnit player, Tile targetTile, GridManager gridManager)
    {
        _player = player;
        _targetTile = targetTile;
        _gridManager = gridManager;
    }

    public void Execute(ICommandInvoker invoker)
    {
        _player.MoveTo(_targetTile);
        invoker.FinishedCommandExecution(this);
    }

    public bool Validate()
    {
        // TODO: Add more complex validation logic here if needed like range, ap available, etc.
        return _targetTile != null;
    }
}
