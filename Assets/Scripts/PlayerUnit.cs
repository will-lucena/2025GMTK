using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private Transform rightHandTransform;

    private Tile currentHoverTile;
    private Boomerang activeBoomerang;
    
    protected override void Awake()
    {
        base.Awake();
        WatchGrid();
        activeBoomerang = Instantiate(boomerangPrefab, rightHandTransform).GetComponent<Boomerang>();
        activeBoomerang.Initialize(this); // ignore range for now
        UIManager.Instance.DisableCatchCommandLabel();
        UIManager.Instance.UpdateStepsCounterLabel(stepsAvailable, maxMovementAmount);
        TurnManager.Instance.onPhaseChange += OnTurnPhaseChange;
    }

    private void Update()
    {
        HandleKeyboardInputs();
        if (CanCallBoomerang() && TryGetInput(KeyCode.Space))
        {
            ReceiveBoomerangReturn();
        } 
    }

    private void LateUpdate()
    {
        if (IsBoomerangInAir())
        {
            BoomerangLinePreview.Instance.ClearPath();
            BoomerangLinePreview.Instance.ShowPath(activeBoomerang.transform.position, rightHandTransform.position);
        }
    }

    private void WatchGrid()
    {
        Tile.OnTileClicked += OnTileSelected;
        Tile.OnTileHovered += OnTileHover;
        Tile.OnTileUnHovered += OnTileUnhover;
    }

    private bool CanCallBoomerang()
    {
        if (!activeBoomerang) return false;
        if (!activeBoomerang.isMoving) return true;
        return IsBoomerangInAir();
    }

    public Transform WeaponParent()
    {
        return rightHandTransform;
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

    public void BoomerangeThrew()
    {
        TurnManager.Instance.EndPlayerTurn();
    }

    public void BoomerangeCatch()
    {
        animator.SetTrigger("Catch");
        UIManager.Instance.EnableThrowCommandLabel();
        UIManager.Instance.DisableCatchCommandLabel();
    }

    public override void ResetMovement()
    {
        base.ResetMovement();
        UIManager.Instance.UpdateStepsCounterLabel(stepsAvailable);
        UIManager.Instance.EnableMovementCommands();
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

    private bool CanMove()
    {
        if (!TurnManager.Instance.IsPlayerTurn()) return false;
        if (!activeBoomerang) return true;
        return !activeBoomerang.isReturning;
    }

    private bool TryGetInput(KeyCode keyCode)
    {
        if (!TurnManager.Instance.IsPlayerTurn()) return false;
        if (keyCode != KeyCode.Space && stepsAvailable == 0)
        {
            UIManager.Instance.DisableMovementCommands();
            return false;
        }
        return Input.GetKeyDown(keyCode);
    }

    private void TryMove(int targetX, int targetY)
    {
        if (GridManager.Instance.GetTileAtPosition(targetX, targetY) != null)
        {
            MoveTo(targetX, targetY);
            OnTileHover(currentHoverTile);
            stepsAvailable--;
            UIManager.Instance.UpdateStepsCounterLabel(stepsAvailable);
        }
    }

    private void OnTurnPhaseChange(TurnManager.GamePhase gamePhase)
    {
        if (gamePhase == TurnManager.GamePhase.PlayerTurn) {
            ResetMovement();
        }
    }

    private void OnTileSelected(Tile targetTile)
    {
        if (CanThrowBoomerang(targetTile))
        {
            Tile currentTile = GridManager.Instance.GetTileAtPosition(x, y);

            if (currentTile == targetTile) return;

            activeBoomerang.Initialize(this, targetTile);
            activeBoomerang.ExecuteThrow();
            animator.SetTrigger("Throw");
            UIManager.Instance.DisableThrowCommandLabel();
            UIManager.Instance.EnableCatchCommandLabel();
        }
    }

    private bool CanThrowBoomerang(Tile targetTile)
    {
        if (!activeBoomerang) return true;
        if (activeBoomerang.isMoving || activeBoomerang.isReturning) return false;
        if (IsBoomerangInAir()) return false;
        return CalculateDistance(targetTile) <= activeBoomerang.MaxDistance;
    }

    private void OnTileUnhover(Tile targetTile)
    {
        currentHoverTile = null;
        targetTile.SetHighlight(false);

        BoomerangLinePreview.Instance.ClearPath();
    }

    private void OnTileHover(Tile targetTile)
    {
        currentHoverTile = targetTile;

        if (IsBoomerangInAir() || targetTile == null) return;

        Color hightlightColor;
        if (CalculateDistance(targetTile) <= activeBoomerang.MaxDistance)
        {
            hightlightColor = targetTile.SetHighlight(true, true);
        } else
        {
            hightlightColor = targetTile.SetHighlight(true, false);
        }
        BoomerangLinePreview.Instance.ShowPath(rightHandTransform.position, targetTile.transform.position, hightlightColor);
    }

    private float CalculateDistance(Tile targetTile)
    {
        if (targetTile == null) return Mathf.Infinity;
        return Vector2.Distance((Vector2)transform.position, (Vector2)targetTile.transform.position);
    }

    private void OnDestroy()
    {
        Tile.OnTileClicked -= OnTileSelected;
        Tile.OnTileHovered -= OnTileHover;
        Tile.OnTileUnHovered -= OnTileUnhover;
    }
}
