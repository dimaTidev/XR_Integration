using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Socket_snapConterDirection", menuName = "ScriptableObjects/VR/Sockets/SnapConterDirection")]
public class Socket_snapConterDirection : ASocket_snapOption
{
    public override void Snap(GameObject toSnap, Transform connector, GameObject endParent, Transform endConnector)
    {
      // Util_RotationSnap.PlaceAll(toSnap.transform, parentConnector, toSnapConnector);
        Utils_Rotations.PlaceAll(connector, endConnector, toSnap.transform);
    }
}
