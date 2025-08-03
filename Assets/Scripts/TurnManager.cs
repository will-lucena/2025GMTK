using System.Collections;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public enum GamePhase { PlayerTurn, EnemyTurn, None }
    public GamePhase phase {  get; private set; }
    [SerializeField] private EnemyTurnProgressUI enemyTurnUI;
    [SerializeField] private PlayerUnit player;
    [SerializeField] private int turnCount = 0;

    public void StartPlayerTurn()
    {
        turnCount++;
        UIManager.Instance.UpdateCounterLabel(turnCount);

        if (turnCount >= 4)
        {
            SceneManager.Instance.loadLevel(3);
        }

        phase = GamePhase.PlayerTurn;
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
        phase = GamePhase.None;
        turnCount = 0;
    }

    private void StartEnemyTurn()
    {
        if (GridManager.Instance.HasEnemiesAlive()) {
            phase = GamePhase.EnemyTurn;
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
