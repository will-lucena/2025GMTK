using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CallCommand : ICommand
{
    private PlayerUnit _player;

    public CallCommand(PlayerUnit player)
    {
        _player = player;
    }

    public void Execute(ICommandInvoker invoker)
    {
        _player.CallWeapon(invoker);
    }


    public bool Validate()
    {
        // TODO: Add more complex validation logic here if needed like range, ap available, etc.
        if (_player.boomerangHeld) return false;
        return true;
    }
}
