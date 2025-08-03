using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : Singleton<SpawnerManager>
{
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Runtime Units")]
    private PlayerUnit playerUnit;
    private List<EnemyUnit> enemies = new List<EnemyUnit>();

    public void SpawnPlayerAt(int x, int y)
    {
        playerUnit = Instantiate(playerPrefab).GetComponent<PlayerUnit>();
        GridManager.Instance.player = playerUnit;
        playerUnit.Init(x, y);
    }

    public void SpawnEnemyAt(int x, int y)
    {
        EnemyUnit enemyUnit = Instantiate(enemyPrefab).GetComponent<EnemyUnit>();
        enemyUnit.Init(x, y);
        enemies.Add(enemyUnit);
        GridManager.Instance.enemies = enemies;
    }
}
