using System.Collections;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public enum GamePhase { PlayerTurn, EnemyTurn, None }
    public GamePhase phase {  get; private set; }
    [SerializeField] private int turnCount = 0;
    [SerializeField] private float enemyTurnDuration = 2f;
    [SerializeField] private int availableTurns = 4;

    public System.Action<GamePhase> onPhaseChange;

    public int AvailableTurns()
    {
        return availableTurns;
    }

    public void StartPlayerTurn()
    {
        turnCount++;
        UIManager.Instance.UpdateCounterLabel(turnCount);

        if (turnCount >= availableTurns)
        {
            SceneManager.Instance.LoadLevel((int)SceneManager.DefaultLevels.Gameover);
        }

        ChangePhase(GamePhase.PlayerTurn);
    }

    public void EndPlayerTurn()
    {
        StartEnemyTurn();
    }

    public bool IsPlayerTurn()
    {
        return phase == GamePhase.PlayerTurn;
    }

    public void Reset()
    {
        ChangePhase(GamePhase.None);
        turnCount = 0;
    }

    private void ChangePhase(GamePhase phase)
    {
        this.phase = phase;
        onPhaseChange?.Invoke(phase);
    }

    private void StartEnemyTurn()
    {
        if (GridManager.Instance.HasEnemiesAlive()) {
            ChangePhase(GamePhase.EnemyTurn);
            RunEnemyAI();
        } else
        {
            turnCount--;
            StartPlayerTurn();
        }
        
    }

    private void RunEnemyAI()
    {
        UIManager.Instance.OnEnemyTurnFinished += OnFinishEnemyTurn;
        UIManager.Instance.StartProgressBar(enemyTurnDuration);
        GridManager.Instance.RunEnemiesTurn();
    }

    private void OnFinishEnemyTurn()
    {
        UIManager.Instance.OnEnemyTurnFinished -= OnFinishEnemyTurn;
        StartPlayerTurn();
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnEnemyTurnFinished -= OnFinishEnemyTurn;
    }
}
