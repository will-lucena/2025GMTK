using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public GameObject boomerangPrefab;
    public bool boomerangInAir = false;

    private Boomerang activeBoomerang;
    private bool targetingBoomerang = false;
    private List<Tile> previewPath = new List<Tile>();

    void Update()
    {
        if (!boomerangInAir && !targetingBoomerang && Input.GetKeyDown(KeyCode.Space))
        {
            targetingBoomerang = true;
            gridManager.HighlightAllTiles(true);
            Tile.OnTileClicked += OnTileSelected;
            Tile.OnTileHovered += OnTileHover;

            /*boomerangInAir = true;
            gridManager.HighlightAllTiles(true);
            Tile.OnTileClicked += OnTileSelected;

            Vector2Int dir = new Vector2Int(1, 0); // TEMP: Always throws right
            Boomerang boomerang = Instantiate(boomerangPrefab).GetComponent<Boomerang>();
            activeBoomerang = boomerang;

            Tile myTile = gridManager.GetTileAtPosition(x, y);
            activeBoomerang.Initialize(myTile, dir, 5, gridManager);
            activeBoomerang.ExecuteThrow();

            targetingBoomerang = false;
            gridManager.HighlightAllTiles(false);
            Tile.OnTileClicked -= OnTileSelected;l*/
        }

        if (!boomerangInAir)
        {
            if (Input.GetKeyDown(KeyCode.W)) TryMove(x, y + 1);
            if (Input.GetKeyDown(KeyCode.S)) TryMove(x, y - 1);
            if (Input.GetKeyDown(KeyCode.A)) TryMove(x - 1, y);
            if (Input.GetKeyDown(KeyCode.D)) TryMove(x + 1, y);
        }
        else
        {
            ReceiveBoomerangReturn();
        }
    }

    void TryMove(int targetX, int targetY)
    {
        if (gridManager.GetTileAtPosition(targetX, targetY) != null)
        {
            MoveTo(targetX, targetY);
        }
    }

   /* void ThrowBoomerang(int dx, int dy)
    {
        int maxRange = 4;
        for (int i = 1; i <= maxRange; i++)
        {
            int tx = x + dx * i;
            int ty = y + dy * i;
            Tile tile = gridManager.GetTileAtPosition(tx, ty);
            if (tile == null) break;

            Debug.Log($"Boomerang hits tile: {tx}, {ty}");
            // You could add logic here to hit enemies or trigger tile effects.

            returnTarget = new Vector2Int(tx, ty);
        }

        boomerangInAir = true;
    }*/

    public void ReceiveBoomerangReturn()
    {
        if (activeBoomerang != null)
            activeBoomerang.ExecuteReturn();
    }

    private void OnTileSelected(Tile targetTile)
    {
        Tile myTile = gridManager.GetTileAtPosition(x, y);
        Vector2Int dir = new Vector2Int(
            Mathf.Clamp(targetTile.x - x, -1, 1),
            Mathf.Clamp(targetTile.y - y, -1, 1)
        );

        if (dir == Vector2Int.zero) return;

        GameObject b = Instantiate(boomerangPrefab);
        Boomerang boomerang = b.GetComponent<Boomerang>();
        boomerang.Initialize(myTile, dir, 99, gridManager); // ignore range for now
        boomerang.ExecuteThrow();

        boomerangInAir = true;
        activeBoomerang = boomerang;

        // Cleanup
        targetingBoomerang = false;
        gridManager.HighlightAllTiles(false);
        Tile.OnTileClicked -= OnTileSelected;
        Tile.OnTileHovered -= OnTileHover;
        ClearPathPreview();
        TurnManager.Instance.EndPlayerTurn();
    }

    private void OnTileHover(Tile targetTile)
    {
        if (!targetingBoomerang) return;

        ClearPathPreview();

        Vector2Int dir = new Vector2Int(
            Mathf.Clamp(targetTile.x - x, -1, 1),
            Mathf.Clamp(targetTile.y - y, -1, 1)
        );

        if (dir == Vector2Int.zero) return;

        Vector2Int pos = new Vector2Int(x, y);

        while (true)
        {
            pos += dir;
            Tile tile = gridManager.GetTileAtPosition(pos.x, pos.y);
            if (tile == null) break;

            previewPath.Add(tile);
            tile.SetHighlight(true); // Optional: use a different color for preview?
        }
    }

    private void ClearPathPreview()
    {
        foreach (Tile t in previewPath)
            t.SetHighlight(false);

        previewPath.Clear();
    }

}
