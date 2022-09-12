using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_IncrementalLocalRotation : MonoBehaviour
{
    [SerializeField] Transform toSnap = null;
    [SerializeField] Transform toSnapGhost = null;

    [SerializeField, Range(1,360)] int angleDivider = 8;
    [SerializeField] float gizmos_raysLength = 0.15f;

    [ContextMenu("Setup test")]
    void SetupTest()
    {
        if(!toSnap)
            toSnap = new GameObject("toSnap").transform;
        if(!toSnapGhost)
            toSnapGhost = new GameObject("toSnapGhost").transform;

        toSnap.SetParent(transform);
        toSnapGhost.SetParent(transform);

        toSnap.rotation = Quaternion.LookRotation(-transform.forward, transform.up);
        toSnapGhost.rotation = toSnap.rotation;

        toSnap.localPosition = new Vector3(0,0,0.8f);
        toSnapGhost.localPosition = new Vector3(0, 0, 0.4f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Utils_Rotations.Debug_DrawAllRays(transform, angleDivider);

        if(toSnap)
            Gizmos_drawRays(toSnap);
        if (toSnapGhost)
            Gizmos_drawRays(toSnapGhost);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * gizmos_raysLength);

        if (toSnapGhost)
        {
            Gizmos.color = Color.yellow;
            Utils_Rotations.Debug_DrawRayByAngle(transform, -toSnap.localRotation.eulerAngles.z);
            toSnapGhost.transform.rotation = Utils_Rotations.IncrementalRotation_z(transform, toSnap.localRotation.eulerAngles.z, angleDivider);
        }
    }

    void Gizmos_drawRays(Transform pivot)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pivot.position, gizmos_raysLength / 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(pivot.position, pivot.forward * gizmos_raysLength);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(pivot.position, pivot.up * gizmos_raysLength);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pivot.position, pivot.right * gizmos_raysLength);
    }
}
