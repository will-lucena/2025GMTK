using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public GameObject tilePrefab;
    public Transform tilesParent;
    public PlayerUnit player;
    public EnemyUnit enemy;

    private Tile[,] tiles;

    void Start()
    {
        GenerateGrid();
        SpawnPlayer();
        SpawnEnemy();
    }

    void GenerateGrid()
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
                    tileComponent.Init(x, y);
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

    void SpawnPlayer()
    {
        player.Init(this, width / 2, 0); // Initialize at the starting position (0, 0)
    }

    void SpawnEnemy()
    {
        enemy.Init(this, width / 2, height / 2); // Initialize at the opposite corner (width-1, height-1) 
    }

    public void HighlightAllTiles(bool active)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y].SetHighlight(true, Color.yellow); // For available tiles

    }

}
