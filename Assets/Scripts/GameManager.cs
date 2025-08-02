using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        GridManager.Instance.GenerateGrid();
        SpawnerManager.Instance.SpawnPlayerAt(GridManager.Instance.width / 2, 0);
        SpawnerManager.Instance.SpawnEnemyAt(GridManager.Instance.width / 2, GridManager.Instance.height / 2);
        TurnManager.Instance.StartPlayerTurn();
    }

    private void OnDestroy()
    {
        TurnManager.Instance.Reset();
    }
}
