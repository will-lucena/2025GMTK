using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandInvoker
{
    void InvokeCommand(ICommand command);
    IEnumerator InvokeCommandWithDelay(ICommand command, float delay);
    void InvokeCommandWithoutValidation(ICommand command);
    void FinishedCommandExecution(ICommand command);
}