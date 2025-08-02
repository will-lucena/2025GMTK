using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public enum GamePhase { PlayerTurn, EnemyTurn, None }
    public GamePhase phase = GamePhase.PlayerTurn;
    public EnemyTurnProgressUI enemyTurnUI;
    public PlayerUnit player;
    [SerializeField] private int turnCount = 0;

    private void StartEnemyTurn()
    {
        phase = GamePhase.EnemyTurn;
        enemyTurnUI.StartProgress(2f); // 2 seconds duration
        enemyTurnUI.OnComplete += RunEnemyAI;
    }
    public void StartPlayerTurn()
    {
        turnCount++;

        if (turnCount >= 2)
        {
            SceneManager.Instance.loadLevel(3);
        }

        phase = GamePhase.PlayerTurn;
    }

    private void RunEnemyAI()
    {
        enemyTurnUI.OnComplete -= RunEnemyAI;

        // Perform enemy actions here
        // Then call EndEnemyTurn();

        StartPlayerTurn();
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
}
