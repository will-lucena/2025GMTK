using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    private void Awake() => Instance = this;

    public enum GamePhase { PlayerTurn, BoomerangReturn, EnemyTurn }
    public GamePhase phase = GamePhase.PlayerTurn;

    public PlayerUnit player;

    void Update()
    {
        if (phase == GamePhase.BoomerangReturn)
        {
            if (phase == GamePhase.BoomerangReturn)
            {
                StartCoroutine(DelayedBoomerangReturn());
            }
            phase = GamePhase.EnemyTurn;
        }
        else if (phase == GamePhase.EnemyTurn)
        {
            // Add enemy AI loop here
            phase = GamePhase.PlayerTurn;
        }
    }

    public void EndPlayerTurn()
    {
        phase = GamePhase.BoomerangReturn;
    }

    public void QueueBoomerangReturn()
    {
        phase = GamePhase.BoomerangReturn;
    }

    private IEnumerator DelayedBoomerangReturn()
    {
        phase = GamePhase.BoomerangReturn;
        yield return new WaitForSeconds(0.1f);
        player.ReceiveBoomerangReturn();
        yield return new WaitUntil(() => player.boomerangInAir == false);
        phase = GamePhase.EnemyTurn;
    }

}
