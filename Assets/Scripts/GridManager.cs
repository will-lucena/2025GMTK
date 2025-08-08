using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class GridManager : Singleton<GridManager>
{
    public int width = 8;
    public int height = 8;
    public GameObject tilePrefab;
    public Transform tilesParent;
    public PlayerUnit _player;
    public List<EnemyUnit> enemies;
    public Color insideRangeHighglightColor;
    public Color outOFRangeHighglightColor;

    private Tile[,] tiles;

    public Tile playerTile { get; private set; }
    public Tile weaponTile { get; private set; }

    public void GenerateGrid()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = Instantiate(tilePrefab, tilesParent).GetComponent<Tile>();
                tile.name = $"Tile_{x}_{y}";
                tile.transform.localPosition = new Vector3(x, y, 0);
                Tile tileComponent = tile.GetComponent<Tile>();
                tile.SetHighlight(false);
                if (tileComponent != null)
                {
                    tileComponent.Init(x, y, insideRangeHighglightColor, outOFRangeHighglightColor);
                }

                tiles[x, y] = tile;
            }
        }
    }

    public void PlaceWeapon(Boomerang weapon, Tile tile)
    {
        _player.AssignWeapon(weapon);
        AssignWeaponToTile(tile);
        weaponTile = tile;
    }

    public void PlacePlayer(PlayerUnit player, int x, int y)
    {
        _player = player;
        Tile tile = PlaceUnit(player, x, y);
        playerTile = tile;
    }

    public void PlaceEnemy(EnemyUnit enemy, int x, int y)
    {
        enemies.Add(enemy);
        PlaceUnit(enemy, x, y);
    }


    private Tile PlaceUnit(Unit unit, int x, int y)
    {
        unit.gameObject.SetActive(true);
        unit.Init(x, y);
        return AssignTile(unit.transform, x, y);
    }
    
    private Tile AssignTile(Transform transform, int x, int y)
    {
        Tile parentTile = GetTileAtPosition(x, y);
        AssignTile(transform, parentTile);
        return parentTile;
    }

    public void AssignWeaponToTile(Tile tile, Transform transform = null)
    {
        weaponTile = tile;
        if (transform == null) return;
        AssignTile(transform, tile);
    }

    public void AssignTile(Transform transform, Tile tile)
    {
        transform.parent = tile.transform;
        transform.localPosition = Vector3.zero; // Reset local position to center the unit on the tile
    }

    public void UnassignWeaponToTile(Transform transform)
    {
        UnassignTile(transform);
        weaponTile = null;
    }

    public void UnassignTile(Transform transform)
    {
        transform.parent = null;
    }

    public void AssignPlayerToTile(Transform transform, Tile tile)
    {
        AssignTile(transform, tile);
        playerTile = tile;
    }

    public Tile GetTileAtPosition(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return tiles[x, y]?.GetComponent<Tile>();
    }

    public Tile GetTileAtPosition(Vector2Int position)
    {
        return GetTileAtPosition(position.x, position.y);
    }

    public Tile GetTileAtPosition(Tile tile)
    {
        return GetTileAtPosition(tile.x, tile.y);
    }

    public Tile GetTileAtPosition(Vector3 position)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].transform.position == position) return tiles[x, y];
            }
        }
        return null;
    }

    public bool HasEnemiesAlive()
    {
        return enemies.Any(enemy => enemy.currentHp > 0);
    }

    public void RunEnemiesTurn()
    {
        enemies.ForEach(enemy => { enemy.TakeTurn(); });
    }
}
