using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI throwCommand;
    [SerializeField] private TextMeshProUGUI catchCommand;
    [SerializeField] private TextMeshProUGUI[] movementCommands;
    [SerializeField] private TextMeshProUGUI turnCounter;
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;

    private int maxTurns;

    public void UpdateCounterLabel(int currentTurn, int maxTurns)
    {
        this.maxTurns = maxTurns;
        turnCounter.SetText($"{currentTurn} / {maxTurns}");
    }

    public void UpdateCounterLabel(int currentTurn)
    {
        turnCounter.SetText($"{currentTurn} / {maxTurns}");
    }

    public void DisableThrowCommandLabel()
    {
        throwCommand.color = disabledColor;
    }

    public void DisableCatchCommandLabel()
    {
        catchCommand.color = disabledColor;
    }

    public void EnableThrowCommandLabel()
    {
        throwCommand.color = enabledColor;
    }

    public void EnableCatchCommandLabel()
    {
        catchCommand.color = enabledColor;
    }
}
