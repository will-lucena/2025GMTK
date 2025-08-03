using System.Collections;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public enum GamePhase { PlayerTurn, EnemyTurn, None }
    public GamePhase phase {  get; private set; }
    [SerializeField] private EnemyTurnProgressUI enemyTurnUI;
    [SerializeField] private int turnCount = 0;

    public System.Action<GamePhase> onPhaseChange;

    public void StartPlayerTurn()
    {
        turnCount++;
        UIManager.Instance.UpdateCounterLabel(turnCount);

        if (turnCount >= 4)
        {
            SceneManager.Instance.loadLevel(3);
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
            enemyTurnUI.StartProgress(2f); // 2 seconds duration
            enemyTurnUI.OnComplete += RunEnemyAI;
        } else
        {
            turnCount--;
            StartPlayerTurn();
        }
        
    }

    private void RunEnemyAI()
    {
        enemyTurnUI.OnComplete -= RunEnemyAI;
        StartPlayerTurn();
    }
}
