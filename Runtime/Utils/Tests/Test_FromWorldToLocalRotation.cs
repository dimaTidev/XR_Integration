using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FromWorldToLocalRotation : MonoBehaviour
{
    [SerializeField] Transform pivot = null;
    [SerializeField] Transform ghost = null;
    [SerializeField] float gizmos_raysLength = 0.2f;


   // public Vector3 pivotRotation;
   // public Vector3 myRotation;
   // public Vector3 ghostRotation;
   // public Vector3 resultRotation;


    [ContextMenu("Setup test")]
    void SetupTest()
    {
        if (!pivot)
            pivot = new GameObject("toSnap").transform;
        if (!ghost)
            ghost = new GameObject("toSnapGhost").transform;

        ghost.SetParent(pivot);

        pivot.position = new Vector3(0, 0, 0.8f);
        ghost.position = new Vector3(0, 0, 0.4f);
    }

    private void OnDrawGizmos()
    {
        if (!ghost || !pivot)
            return;
        // ghost.rotation = Utils_Rotations.WorldToLocal(transform.rotation, pivot.rotation);
        ghost.localRotation = pivot.InverseTransformRotation(transform.rotation);

        Gizmos_drawRays(pivot);
        Gizmos_drawRays(transform);
        Gizmos_drawRays(ghost);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(pivot.position, ghost.position);
        Gizmos.DrawLine(ghost.position, transform.position);
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
