using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI throwCommand;
    [SerializeField] private TextMeshProUGUI catchCommand;
    [SerializeField] private TextMeshProUGUI questLabel;
    [SerializeField] private TextMeshProUGUI turnCounter;
    [SerializeField] private TextMeshProUGUI stepsCounter;
    [SerializeField] private TextMeshProUGUI[] movementCommands;
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;
    [SerializeField] private UnityEngine.UI.Button resetButton;
    [SerializeField] private UnityEngine.UI.Button congratzButton;
    [SerializeField] private EnemyTurnProgressUI enemyTurnUI;

    private int maxTurns;
    private int maxSteps;

    public System.Action OnEnemyTurnFinished;

    private void Start()
    {
        resetButton.gameObject.SetActive(true);
        congratzButton.gameObject.SetActive(false);
    }

    public void UpdateTurnCounterLabel(int currentTurn, int maxTurns)
    {
        this.maxTurns = maxTurns;
        turnCounter.SetText($"{currentTurn} / {maxTurns}");
    }

    public void UpdateCounterLabel(int currentTurn)
    {
        turnCounter.SetText($"{currentTurn} / {maxTurns}");
    }

    public void UpdateStepsCounterLabel(int current, int max)
    {
        this.maxSteps = max;
        stepsCounter.SetText($"{current} / {maxSteps}");
    }

    public void UpdateStepsCounterLabel(int current)
    {
        stepsCounter.SetText($"{current} / {maxSteps}");
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

    public void DisableMovementCommands()
    {
        foreach (var item in movementCommands)
        {
            item.color = disabledColor;
        }
    }

    public void EnableMovementCommands()
    {
        foreach (var item in movementCommands)
        {
            item.color = enabledColor;
        }
    }

    public void UpdateControlButton()
    {
        resetButton?.gameObject?.SetActive(false);
        congratzButton?.gameObject?.SetActive(true);
        questLabel?.SetText("");
    }

    public void StartProgressBar(float duration)
    {
        enemyTurnUI.OnComplete += OnCompleteProgressBar;
        enemyTurnUI.StartProgress(duration);
    }

    private void OnCompleteProgressBar()
    {
        OnEnemyTurnFinished?.Invoke();
    }

    private void OnDestroy()
    {
        enemyTurnUI.OnComplete -= OnCompleteProgressBar;
    }
}
