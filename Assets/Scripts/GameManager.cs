using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector2Int playerSpawnPosition;
    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        GridManager.Instance.GenerateGrid();
        
        PlayerUnit player = SpawnerManager.Instance.SpawnPlayerAt(playerSpawnPosition.x, playerSpawnPosition.y);
        SpawnerManager.Instance.SpawnEnemyAt(GridManager.Instance.width / 2, GridManager.Instance.height / 2 + 1);
        SpawnerManager.Instance.SpawnWeapon(GridManager.Instance.playerTile);

        TurnManager.Instance.StartPlayerTurn();
        UIManager.Instance.UpdateTurnCounterLabel(1, TurnManager.Instance.AvailableTurns());
    }

    private void OnDestroy()
    {
        TurnManager.Instance.Reset();
    }
}
