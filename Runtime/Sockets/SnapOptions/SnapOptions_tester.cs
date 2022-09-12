using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapOptions_tester : MonoBehaviour
{
    [SerializeField] ASocket_snapOption snapOption;
    [SerializeField] Transform toSnap = null;
    [SerializeField] Transform connector = null;
    [SerializeField] Transform endConnector = null;

    private void OnDrawGizmos()
    {
        if (snapOption && toSnap && connector && endConnector)
        {
            snapOption.Snap(toSnap.gameObject, connector, gameObject, endConnector);
        }
    }
}
