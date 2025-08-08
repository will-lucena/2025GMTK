using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyUnit : EnemyUnit
{
    public override void TakeTurn()
    {
        Vector2Int playerPos = GridManager.Instance.playerTile.ToVector2Int();
        Vector2Int boomerangPos = GridManager.Instance.weaponTile.ToVector2Int();

        Tile currentTile = transform.parent.GetComponent<Tile>();

        Vector2Int awayDir = GetOppositeDirection(currentTile, playerPos, boomerangPos);
        Vector2Int targetPos = currentTile.ToVector2Int() + awayDir;

        Tile targetTile = GridManager.Instance.GetTileAtPosition(targetPos.x, targetPos.y);

        base.TryMove(targetTile);
    }

    private Vector2Int GetOppositeDirection(Tile currentTile, Vector2Int playerPos, Vector2Int boomPos)
    {
        Vector2Int toPlayer = playerPos - currentTile.ToVector2Int();
        Vector2Int toBoom = boomPos - currentTile.ToVector2Int();

        Vector2 awayVector = (new Vector2(toPlayer.x + toBoom.x, toPlayer.y + toBoom.y) * -1).normalized;

        // Convert to closest cardinal direction (up/down/left/right/diagonal)
        int x = Mathf.RoundToInt(awayVector.x);
        int y = Mathf.RoundToInt(awayVector.y);

        return new Vector2Int(x, y);
    }
}
