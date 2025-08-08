using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ActionsManager : Singleton<ActionsManager>,  ICommandInvoker
{
    [SerializeField] private Queue<ICommand> _commandQueue = new Queue<ICommand>();
    [SerializeField] private Queue<ICommand> _executedCommandsQueue = new Queue<ICommand>();
    [SerializeField] private Queue<ICommand> _delayedCommandsQueue = new Queue<ICommand>();

    [SerializeField] private bool _isExecutingCommand = false;
    private ICommand _nextCommandToRun;

    public void InvokeCommand(ICommand command)
    {
        if (_isExecutingCommand)
        {
/*            Enqueue(command);
*/            return;
        }

        Running(command);

        if (command.Validate())
        {
            InvokeCommandWithoutValidation(command);
        } else
        {
            Idle();
        }
    }

    public IEnumerator InvokeCommandWithDelay(ICommand command, float delay)
    {
        throw new NotImplementedException("InvokeCommandWithDelay is not implemented yet.");
        yield return new WaitForSeconds(delay);
        InvokeCommand(command);
    }

    public void InvokeCommandWithoutValidation(ICommand command)
    {
        if (!ExecutionAvailable(command))
        {
            Enqueue(command);
            return;
        }
        else
        {
            command.Execute(this);
        }
    }

    public void FinishedCommandExecution(ICommand command)
    {
        if (command != null)
        {
            _executedCommandsQueue.Enqueue(command);
        }

        Idle();

        if (_commandQueue.Count > 0)
        {
            ICommand nextCommand = _commandQueue.Dequeue();
            InvokeCommand(nextCommand);
        }
    }

    private void Enqueue(ICommand command)
    {
        if (_commandQueue.Count >= 1 && IsCommandInQueue(command, _commandQueue)) throw new Exception("pode não");
        _commandQueue.Enqueue(command);
    }

    private bool IsCommandInQueue(ICommand command, Queue<ICommand> queue)
    {
        return queue.Contains(command);
    }

    private void Running(ICommand command)
    {
        _isExecutingCommand = true;
        _nextCommandToRun = command;
    }

    private void Idle()
    {
        _isExecutingCommand = false;
        _nextCommandToRun = null;
    }

    private bool ExecutionAvailable(ICommand command)
    {
        return command == _nextCommandToRun;
    }
}
