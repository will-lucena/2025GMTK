using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCommand : ICommand
{
    private PlayerUnit _player;
    private Boomerang _weapon;
    private Tile _targetTile;

    public ThrowCommand(PlayerUnit player, Boomerang weapon, Tile targetTile)
    {
        _player = player;
        _weapon = weapon;
        _targetTile = targetTile;
    }


    public void Execute(ICommandInvoker invoker)
    {
        _player.Throw(_targetTile, invoker);
    }

    public bool Validate()
    {
        // TODO: Add more complex validation logic here if needed like range, ap available, etc.
        if (!_player.boomerangHeld) return false;
        if (_targetTile == null) return false;
        if (!_player.IsThrowEnabled(_targetTile)) return false;
        return true;
    }
}
