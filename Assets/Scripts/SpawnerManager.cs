using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : Singleton<SpawnerManager>
{
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject weaponPrefab;

    public PlayerUnit SpawnPlayerAt(int x, int y)
    {
        PlayerUnit playerUnit = Instantiate(playerPrefab).GetComponent<PlayerUnit>();
        GridManager.Instance.PlacePlayer(playerUnit, x, y);
        return playerUnit;
    }

    public void SpawnEnemyAt(int x, int y)
    {
        EnemyUnit enemyUnit = Instantiate(enemyPrefab).GetComponent<EnemyUnit>();
        GridManager.Instance.PlaceEnemy(enemyUnit, x, y);
    }

    public void SpawnWeapon(Tile tile)
    {
        Boomerang weapon = Instantiate(weaponPrefab).GetComponent<Boomerang>();
        GridManager.Instance.PlaceWeapon(weapon, tile);
    }
}
