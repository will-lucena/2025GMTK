using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyUnit : EnemyUnit
{
    public override void TakeTurn()
    {
        Vector2Int playerPos = GridManager.Instance.player.currentTile.ToVector2Int();
        Vector2Int boomerangPos = GridManager.Instance.player.activeBoomerang.currentTile.ToVector2Int();

        Vector2Int awayDir = GetOppositeDirection(playerPos, boomerangPos);
        Vector2Int targetPos = currentTile.ToVector2Int() + awayDir;

        Tile targetTile = GridManager.Instance.GetTileAtPosition(targetPos.x, targetPos.y);

        base.TryMove(targetTile);
    }

    private Vector2Int GetOppositeDirection(Vector2Int playerPos, Vector2Int boomPos)
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
