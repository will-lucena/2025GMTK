using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int x;
    public int y;
    public int moveRange = 1;

    protected GridManager gridManager;

    public virtual void Init(GridManager gm, int startX, int startY)
    {
        gridManager = gm;
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

    public virtual void TakeDamage(int damage)
    {

    }

    private void move(int x, int y)
    {
        Tile tile = gridManager.GetTileAtPosition(x, y);
        tile.unit = this;
        Vector3 tilePosition = tile.transform.position;
        transform.position = tilePosition;
    }
}
