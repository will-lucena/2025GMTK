using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected int x;
    [SerializeField] protected int y;
    [SerializeField] protected int maxMovementAmount = 1;
    [SerializeField] protected int maxHp;
    protected Animator animator;
    public int currentHp { get; protected set; }
    public int stepsAvailable { get; protected set; }
    public Tile currentTile { get; protected set; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        stepsAvailable = maxMovementAmount;
    }

    public virtual void Init(int startX, int startY)
    {
        x = startX;
        y = startY;
        move(x, y);
    }

    protected virtual bool TryMove(int targetX, int targetY)
    {
        if (GridManager.Instance.GetTileAtPosition(targetX, targetY) != null)
        {
            MoveTo(targetX, targetY);
            return true;
        }
        return false;
    }

    protected virtual bool TryMove(Tile targetTile)
    {
        return TryMove(targetTile.x, targetTile.y);
    }

    public virtual void MoveTo(int targetX, int targetY)
    {
        x = targetX;
        y = targetY;
        move(x, y);
    }

    public virtual void TakeDamage(int damage = 1)
    {
        currentHp -= damage;
    }

    public virtual void ResetMovement()
    {
        stepsAvailable = maxMovementAmount;
    }

    private void move(int x, int y)
    {
        Tile tile = GridManager.Instance.GetTileAtPosition(x, y);
        tile.unit = this;
        Vector3 tilePosition = tile.transform.position;
        transform.position = tilePosition;
        currentTile = tile;
    }
}
