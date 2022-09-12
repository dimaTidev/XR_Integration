using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabSurface_rail : AHandGrabSurface
{
    [SerializeField] Vector3 direction;
    [SerializeField] float forwardOffset;
    [SerializeField] float backwardOffset;

    Vector3 StartPosition(Vector3 originPos) => originPos + transform.TransformDirection(direction) * forwardOffset;
    Vector3 EndPosition(Vector3 originPos) => originPos - transform.TransformDirection(direction) * backwardOffset;

    public override Vector3 GetPoint(Vector3 originPos, Vector3 targetPos) => GetClosestPointOnFiniteLine(targetPos, StartPosition(originPos), EndPosition(originPos));

    public override void DrawGizmos(Vector3 originPos)
    {
        Vector3 startPoint = StartPosition(originPos);
        Vector3 endPoint = EndPosition(originPos);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPoint, endPoint);
    }


    Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end)
    {
        Vector3 line_direction = line_end - line_start;
        float line_length = line_direction.magnitude;
        line_direction.Normalize();
        float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
        return line_start + line_direction * project_length;
    }
}
