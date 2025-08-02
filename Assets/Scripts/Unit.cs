using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int x;
    public int y;
    public int moveRange = 1;

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
        Destroy(gameObject);
        SceneManager.Instance.loadLevel(2);
    }

    private void move(int x, int y)
    {
        Tile tile = GridManager.Instance.GetTileAtPosition(x, y);
        tile.unit = this;
        Vector3 tilePosition = tile.transform.position;
        transform.position = tilePosition;
    }
}
