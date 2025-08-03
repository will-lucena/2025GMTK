using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public int width = 8;
    public int height = 8;
    public GameObject tilePrefab;
    public Transform tilesParent;
    public PlayerUnit player;
    public List<EnemyUnit> enemies;
    public Color insideRangeHighglightColor;
    public Color outOFRangeHighglightColor;

    private Tile[,] tiles;

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

    public Tile GetTileAtPosition(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return tiles[x, y]?.GetComponent<Tile>();
    }

    public Tile GetTileAtPosition(Vector2Int position)
    {
        return GetTileAtPosition(position.x, position.y);
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
