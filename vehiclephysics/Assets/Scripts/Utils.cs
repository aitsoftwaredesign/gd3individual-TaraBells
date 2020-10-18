using UnityEditor;
using UnityEngine;

public static class Utils
{
    public static void DrawArrow(in Vector3 pos, in Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction);
        DrawTip(pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
    }

    public static void DrawArrow(in Vector3 pos, in Vector3 direction, in Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction, color);
        DrawTip(pos, direction, color, arrowHeadLength, arrowHeadAngle);
    }

    private static void DrawTip(in Vector3 pos, in Vector3 direction, in Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        var right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back * arrowHeadLength;
        var left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back * arrowHeadLength;
        var up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back * arrowHeadLength;
        var down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back * arrowHeadLength;
        var end = pos + direction;
        Debug.DrawRay(end, right, color);
        Debug.DrawRay(end, left, color);
        Debug.DrawRay(end, up, color);
        Debug.DrawRay(end, down, color);
    }
}