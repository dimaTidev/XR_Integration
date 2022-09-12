using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHandGrabSurface : MonoBehaviour
{
    public abstract Vector3 GetPoint(Vector3 originPos, Vector3 targetPos);

    public abstract void DrawGizmos(Vector3 originPos);
}
