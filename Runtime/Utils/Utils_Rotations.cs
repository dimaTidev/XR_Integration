using UnityEngine;

public static class Utils_Rotations
{

    public static void PlaceAll(Transform connector, Transform endConnector, Transform rotationGameObject)
    {
        rotationGameObject.position = SnapPosition(connector, endConnector, rotationGameObject);
        Get_LocalDirections(connector, rotationGameObject, out Vector3 forward, out Vector3 upwards);

        Transform oldParent = connector.parent;
        Vector3 oldPos = connector.localPosition;
        Quaternion oldRot = connector.localRotation;
        PlaceConnector(connector, endConnector);
        rotationGameObject.rotation = RotationInPlace(forward, upwards, connector); //rotationGameObject
        connector.SetParent(oldParent);
        connector.localPosition = oldPos;
        connector.localRotation = oldRot;
    }

    public static Vector3 SnapPosition(Transform connector, Transform endConnector, Transform placedGameObject)
    {
        Vector3 existingLocalPos = connector.InverseTransformPoint(placedGameObject.position) * -1;
        return endConnector.transform.TransformPoint(existingLocalPos);
    }

    //[ContextMenu("_Rotation InPlace")]
    public static Quaternion RotationInPlace(Vector3 forward, Vector3 upwards, Transform connector) //Transform rotationGameObject
    {
        Vector3 forward_glob = connector.TransformDirection(forward);
        Vector3 upwards_glob = connector.TransformDirection(upwards);

        return Quaternion.LookRotation(forward_glob, upwards_glob);
    }

    //[ContextMenu("_Place Connector")]
    public static void PlaceConnector(Transform connector, Transform endConnector)
    {
        connector.transform.SetParent(endConnector);
        connector.transform.position = endConnector.transform.position;

        Quaternion rotation = Quaternion.LookRotation(-endConnector.forward, connector.up);
        connector.rotation = rotation;
    }

    public static void Get_LocalDirections(Transform connector, Transform rotationGameObject, out Vector3 forward, out Vector3 upwards)
    {
        forward = connector.InverseTransformDirection(rotationGameObject.forward);
       // upwards = connector.InverseTransformDirection(-connector.up);
        //upwards = connector.InverseTransformDirection(-connector.forward);
        upwards = connector.InverseTransformDirection(rotationGameObject.up);
    }

    #region Incremental Rotations
    //-----------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Example of use -> transform.rotation = Utils_Rotations.IncrementalRotation_z(...);
    /// </summary>
    /// <param name="pivot">Rotate around in his local -Z axis</param>
    /// <param name="angle">angle of current rotation, it will snap</param>
    /// <param name="angleDivider">how much divides contains 360* circle</param>
    /// <returns></returns>
    public static Quaternion IncrementalRotation_z(Transform pivot, float angle, float angleDivider)
    {
        Vector3 forward = -pivot.forward;

        Quaternion rotation = Quaternion.Euler(0, 0, RoundAngle(-angle, angleDivider));
        Vector3 dir = rotation * Vector3.up;
        dir = pivot.TransformDirection(dir);

        Vector3 up = dir;
        return Quaternion.LookRotation(forward, up);
    }

  //  public static Quaternion IncrementalRotation_z(Transform pivot, float angle, float angleDivider, Transform parent)
  //  {
  //      Vector3 forward = parent.forward;
  //
  //      Quaternion rotation = Quaternion.Euler(0, 0, RoundAngle(-angle, angleDivider));
  //      Vector3 dir = rotation * Vector3.up;
  //      dir = pivot.TransformDirection(dir);
  //
  //      Vector3 up = dir;
  //      return Quaternion.LookRotation(forward, up);
  //  }

    public static void Debug_DrawSnappedDirection(Transform pivot, float angle, float angleDivider)
    {
        Vector3 forward = -pivot.forward;

        Quaternion rotation = Quaternion.Euler(0, 0, RoundAngle(-angle, angleDivider));
        Vector3 dir = rotation * Vector3.up;
        dir = pivot.TransformDirection(dir);

        Debug.DrawRay(pivot.position, dir, Color.green);
    }

    public static void Debug_DrawAllRays(Transform pivot, float angleDivider)
    {
        float stepAngle = 360f / angleDivider;
        if (stepAngle < 1)
            stepAngle = 1;

        float angle = 0;
        while (angle < 360)
        {
            angle += stepAngle;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 dir = rotation * Vector3.up;
            dir = pivot.TransformDirection(dir);
            Debug.DrawRay(pivot.position, dir);
           // Gizmos.DrawRay(pivot.position, dir);
        }
    }

    public static void Debug_DrawRayByAngle(Transform pivot, float rotationAngle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, -rotationAngle);
        Vector3 dir = rotation * Vector3.up;
        dir = pivot.TransformDirection(dir);
        Debug.DrawRay(pivot.position, dir, Color.yellow);
       // Gizmos.DrawRay(pivot.position, dir);
    }

    public static void Debug_DrawTransform(Transform pivot, float length)
    {
        Debug.DrawRay(pivot.position, pivot.forward * length, Color.blue);
        Debug.DrawRay(pivot.position, pivot.up * length, Color.green);
        Debug.DrawRay(pivot.position, pivot.right * length, Color.red);
    }

    //18/5=3.6
    static float RoundAngle(float angle, float angleDivider)
    {
        float stepAngle = 360f / angleDivider;
        return Mathf.RoundToInt(angle / stepAngle) * stepAngle;
    }
    //-----------------------------------------------------------------------------------------------------------------------------------
    #endregion
}