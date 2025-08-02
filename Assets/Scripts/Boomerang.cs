using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public PlayerUnit owner;
    public List<Tile> throwPath = new();
    private bool returning = false;
    public float moveSpeed = 5f;

    public void Initialize(Tile startTile, Vector2Int dir, int maxDistance, GridManager grid)
    {
        owner = startTile.unit as PlayerUnit;
        throwPath.Clear();
        returning = false;

        Vector2Int pos = new Vector2Int(startTile.x, startTile.y);

        for (int i = 1; i <= maxDistance; i++)
        {
            pos += dir;
            Tile tile = grid.GetTileAtPosition(pos.x, pos.y);
            if (tile == null) break;
            throwPath.Add(tile);

            if (tile.unit != null) break;
        }

        transform.position = startTile.transform.position;
    }

    public void ExecuteThrow()
    {
        StartCoroutine(MoveAlongPath(new List<Tile>(throwPath), forward: true));
    }

    public void ExecuteReturn()
    {
        List<Tile> returnPath = new List<Tile>(throwPath);
        returnPath.Reverse();
        StartCoroutine(MoveAlongPath(returnPath, forward: false));
    }

    private IEnumerator MoveAlongPath(List<Tile> path, bool forward)
    {
        foreach (var tile in path)
        {
            Vector3 start = transform.position;
            Vector3 end = tile.transform.position;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }

            // Hit detection
            if (tile.unit != null && tile.unit != owner)
            {
                tile.unit.TakeDamage(1);
                break;
            }

            yield return new WaitForSeconds(0.05f); // Small delay between tiles
        }

        if (forward)
        {
            returning = true;
            TurnManager.Instance.QueueBoomerangReturn();
        }
        else
        {
            owner.boomerangInAir = false;
            Destroy(gameObject);
        }
    }
}
