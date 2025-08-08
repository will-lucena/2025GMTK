using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private ActionsManager actionsManager;

    private Tile currentHoverTile;
    public Boomerang activeWeapon { get; private set; }
    public bool boomerangHeld { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        WatchGrid();
        UIManager.Instance.DisableCatchCommandLabel();
        UIManager.Instance.UpdateStepsCounterLabel(stepsAvailable, maxMovementAmount);
        TurnManager.Instance.onPhaseChange += OnTurnPhaseChange;
    }

    private void LateUpdate()
    {
        if (IsBoomerangInAir())
        {
            BoomerangLinePreview.Instance.ClearPath();
            BoomerangLinePreview.Instance.ShowPath(activeWeapon.transform.position, rightHandTransform.position);
        }
    }

    private void WatchGrid()
    {
        Tile.OnTileHovered += OnTileHover;
        Tile.OnTileUnHovered += OnTileUnhover;
    }

    private bool CanCallBoomerang()
    {
        if (!activeWeapon) return false;
        if (!activeWeapon.isMoving) return true;
        return IsBoomerangInAir();
    }

    public Transform WeaponParent()
    {
        return rightHandTransform;
    }

    public bool IsBoomerangInAir()
    {
        return activeWeapon?.transform.parent == null;
    }

    public void BoomerangeThrew()
    {
        boomerangHeld = false;
        TurnManager.Instance.EndPlayerTurn();
    }

    public void BoomerangeCatch()
    {
        animator.SetTrigger("Catch");
        UIManager.Instance.EnableThrowCommandLabel();
        UIManager.Instance.DisableCatchCommandLabel();

        activeWeapon.transform.parent = rightHandTransform;
        boomerangHeld = true;
        GridManager.Instance.AssignWeaponToTile(GridManager.Instance.playerTile);
    }

    public override void ResetMovement()
    {
        base.ResetMovement();
        UIManager.Instance.UpdateStepsCounterLabel(stepsAvailable);
        UIManager.Instance.EnableMovementCommands();
    }

    private bool CanMove()
    {
        if (!TurnManager.Instance.IsPlayerTurn()) return false;
        if (!activeWeapon) return true;
        return !activeWeapon.isReturning;
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

    protected override bool TryMove(int targetX, int targetY)
    {
        if (base.TryMove(targetX, targetY))
        {
            OnTileHover(currentHoverTile);
            stepsAvailable--;
            UIManager.Instance.UpdateStepsCounterLabel(stepsAvailable);
            return true;
        }
        return false;
    }

    private void OnTurnPhaseChange(TurnManager.GamePhase gamePhase)
    {
        if (gamePhase == TurnManager.GamePhase.PlayerTurn) {
            ResetMovement();
        }
    }

    /*private void OnTileSelected(Tile targetTile)
    {
        ThrowCommand command = new ThrowCommand(this, activeWeapon, targetTile);
        actionsManager.InvokeCommand(command);
        if (CanThrowBoomerang(targetTile))
        {
            Tile currentTile = GridManager.Instance.GetTileAtPosition(x, y);

            if (currentTile == targetTile) return;

            activeWeapon.ExecuteThrow(targetTile, actionsManager);
            animator.SetTrigger("Throw");
            UIManager.Instance.DisableThrowCommandLabel();
            UIManager.Instance.EnableCatchCommandLabel();
        }
    }*/

    public bool IsThrowEnabled(Tile targetTile)
    {
        return CanThrowBoomerang(targetTile);
    }

    private bool CanThrowBoomerang(Tile targetTile)
    {
        if (!activeWeapon) return true;
        if (activeWeapon.isMoving || activeWeapon.isReturning) return false;
        if (IsBoomerangInAir()) return false;
        return CalculateDistance(targetTile) <= activeWeapon.MaxDistance;
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
        if (CalculateDistance(targetTile) <= activeWeapon.MaxDistance)
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
/*        Tile.OnTileClicked -= OnTileSelected;
*/        Tile.OnTileHovered -= OnTileHover;
        Tile.OnTileUnHovered -= OnTileUnhover;
    }

    public void Throw(Tile targetTile, ICommandInvoker invoker)
    {
        Tile currentTile = GridManager.Instance.GetTileAtPosition(x, y);

        if (currentTile == targetTile) return;

        activeWeapon.ExecuteThrow(targetTile, invoker);
        animator.SetTrigger("Throw");
        UIManager.Instance.DisableThrowCommandLabel();
        UIManager.Instance.EnableCatchCommandLabel();
    }

    public void CallWeapon(ICommandInvoker invoker)
    {
        activeWeapon.ExecuteReturn(rightHandTransform.position, invoker);
    }

    public void AssignWeapon(Boomerang weapon)
    {
        activeWeapon = weapon;
        activeWeapon.transform.parent = rightHandTransform;
        activeWeapon.Initialize(this);
        boomerangHeld = true;
        UIManager.Instance.EnableThrowCommandLabel();
        UIManager.Instance.DisableCatchCommandLabel();
    }
}
