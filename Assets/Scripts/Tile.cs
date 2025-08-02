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

    public static System.Action<Tile> OnTileClicked;
    public static System.Action<Tile> OnTileHovered;

    public void Init(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }

    private void OnMouseDown()
    {
/*        if (!EventSystem.current.IsPointerOverGameObject()) // Prevent UI clicks
*/            OnTileClicked?.Invoke(this);
    }

    private void OnMouseEnter()
    {
/*        if (!EventSystem.current.IsPointerOverGameObject())
*/            OnTileHovered?.Invoke(this);
    }

    public void SetHighlight(bool active, Color? color = null)
    {
        if (highlightRenderer != null)
        {
            highlightRenderer.enabled = active;
/*            highlightRenderer.color = color ?? Color.yellow;
*/        }
    }
}
