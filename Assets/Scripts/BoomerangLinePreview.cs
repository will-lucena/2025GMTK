using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BoomerangLinePreview : Singleton<BoomerangLinePreview>
{
    private LineRenderer line;

    protected override void Awake()
    {
        base.Awake();
        line = GetComponent<LineRenderer>();
    }

    public void ShowPath(Vector3 initialPosition, Vector3 finalPosition, Color hightlightColor)
    {
        line.positionCount = 2;
        line.SetPosition(0, initialPosition + Vector3.up * 0.1f); // slight lift to avoid z-fighting
        line.SetPosition(1, finalPosition); // slight lift to avoid z-fighting
        line.startColor = hightlightColor;
        line.endColor = hightlightColor;
    }

    public void ShowPath(Vector3 initialPosition, Vector3 finalPosition)
    {
        line.positionCount = 2;
        line.SetPosition(0, initialPosition + Vector3.up * 0.1f); // slight lift to avoid z-fighting
        line.SetPosition(1, finalPosition); // slight lift to avoid z-fighting
        line.startColor = Color.yellow;
        line.endColor = Color.yellow;
    }

    public void ClearPath()
    {
        line.positionCount = 0;
    }
}
