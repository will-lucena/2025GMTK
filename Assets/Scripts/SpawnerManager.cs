using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }
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

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    [Header("References")]
    public GridManager gridManager;

    [Header("Runtime Units")]
    public PlayerUnit playerUnit;
    public List<EnemyUnit> enemies = new List<EnemyUnit>();

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
