using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public Unit unit;

    public SpriteRenderer highlightRenderer;
    private Color insideRangeHighglightColor;
    private Color outOFRangeHighglightColor;

    public static System.Action<Tile> OnTileClicked;
    public static System.Action<Tile> OnTileHovered;
    public static System.Action<Tile> OnTileUnHovered;

    public void Init(int xPos, int yPos, Color insideRangeHighglightColor, Color outOFRangeHighglightColor)
    {
        x = xPos;
        y = yPos;
        this.insideRangeHighglightColor = insideRangeHighglightColor;
        this.outOFRangeHighglightColor = outOFRangeHighglightColor;
    }

    private void OnMouseDown()
    {
        OnTileClicked?.Invoke(this);
    }

    private void OnMouseEnter()
    {
        OnTileHovered?.Invoke(this);
    }

    private void OnMouseExit()
    {
        OnTileUnHovered?.Invoke(this);
    }

    public void SetHighlight(bool active, bool reachable = false)
    {
        if (highlightRenderer != null)
        {
            highlightRenderer.enabled = active;
            highlightRenderer.color = reachable ? insideRangeHighglightColor : outOFRangeHighglightColor;
        }
    }
}
