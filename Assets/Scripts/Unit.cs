using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected int x;
    [SerializeField] protected int y;
    [SerializeField] protected int moveRange = 1;
    [SerializeField] protected int maxHp;
    public int currentHp { get; protected set; }
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        currentHp = maxHp;
    }

    public virtual void Init(int startX, int startY)
    {
        x = startX;
        y = startY;
        move(x, y);
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

    private void move(int x, int y)
    {
        Tile tile = GridManager.Instance.GetTileAtPosition(x, y);
        tile.unit = this;
        Vector3 tilePosition = tile.transform.position;
        transform.position = tilePosition;
    }
}
