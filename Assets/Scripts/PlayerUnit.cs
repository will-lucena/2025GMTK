using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : Unit
{
    public GameObject boomerangPrefab;

    private Boomerang activeBoomerang;
    private Tile currentHoverTile;

    private void Awake()
    {
        WatchGrid();
        activeBoomerang = Instantiate(boomerangPrefab, transform).GetComponent<Boomerang>();
        activeBoomerang.Initialize(this); // ignore range for now

    }

    void Update()
    {
        HandleKeyboardInputs();
        if (CanCallBoomerang() && TryGetInput(KeyCode.Space))
        {
            ReceiveBoomerangReturn();
        } 
    }


    private void WatchGrid()
    {
        Tile.OnTileClicked += OnTileSelected;
        Tile.OnTileHovered += OnTileHover;
        Tile.OnTileUnHovered += OnTileUnhover;
    }

    private void HandleKeyboardInputs()
    {
        if (CanMove()) {
            if (TryGetInput(KeyCode.W)) TryMove(x, y + 1);
            if (TryGetInput(KeyCode.S)) TryMove(x, y - 1);
            if (TryGetInput(KeyCode.A)) TryMove(x - 1, y);
            if (TryGetInput(KeyCode.D)) TryMove(x + 1, y);
        }
    }

    private bool CanCallBoomerang()
    {
        if (!activeBoomerang) return false;
        if (!activeBoomerang.isMoving) return true;
        return IsBoomerangInAir();
    }

    private bool CanThrowBoomerang(Tile targetTile)
    {
        if (!activeBoomerang) return true;
        if (activeBoomerang.isMoving || activeBoomerang.isReturning) return false;
        if (IsBoomerangInAir()) return false;
        return CalculateDistance(targetTile) <= activeBoomerang.maxDistance;
    }

    private bool CanMove()
    {
        if (!TurnManager.Instance.IsPlayerTurn()) return false;
        if (!activeBoomerang) return true;
        return !activeBoomerang.isReturning;
    }

    private bool TryGetInput(KeyCode keyCode)
    {
        if (!TurnManager.Instance.IsPlayerTurn()) return false;
        return Input.GetKeyDown(keyCode);
    }

    void TryMove(int targetX, int targetY)
    {
        if (GridManager.Instance.GetTileAtPosition(targetX, targetY) != null)
        {
            MoveTo(targetX, targetY);
            OnTileHover(currentHoverTile);
        }
    }

    public void ReceiveBoomerangReturn()
    {
        if (activeBoomerang != null)
        {
            activeBoomerang.ExecuteReturn();
        }
    }

    public bool IsBoomerangInAir()
    {
        return activeBoomerang?.transform.parent == null;
    }

    public void BoomerageThrew()
    {
        TurnManager.Instance.EndPlayerTurn();
    }

    private void OnTileSelected(Tile targetTile)
    {
        if (CanThrowBoomerang(targetTile))
        {
            Tile currentTile = GridManager.Instance.GetTileAtPosition(x, y);

            if (currentTile == targetTile) return;

            activeBoomerang.Initialize(this, targetTile); // ignore range for now
            activeBoomerang.ExecuteThrow();
        }
    }

    private void OnTileHover(Tile targetTile)
    {
        currentHoverTile = targetTile;

        if (IsBoomerangInAir() || targetTile == null) return;

        if (CalculateDistance(targetTile) <= activeBoomerang.maxDistance)
        {
            targetTile.SetHighlight(true, true);
        } else
        {
            targetTile.SetHighlight(true, false);
        }
    }

    private float CalculateDistance(Tile targetTile)
    {
        if (targetTile == null) return Mathf.Infinity;
        return Vector2.Distance((Vector2)transform.position, (Vector2)targetTile.transform.position);
    }

    private void OnTileUnhover(Tile targetTile)
    {
        currentHoverTile = null;
        targetTile.SetHighlight(false);
    }

    private void OnDestroy()
    {
        Tile.OnTileClicked -= OnTileSelected;
        Tile.OnTileHovered -= OnTileHover;
        Tile.OnTileUnHovered -= OnTileUnhover;
    }
}
