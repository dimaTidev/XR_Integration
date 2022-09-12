using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnapConterDir_IncrementalRotation", menuName = "ScriptableObjects/VR/Sockets/SnapConterDir_IncrementalRotation")]
public class Socket_snapCounterDirIncremental : ASocket_snapOption
{
    [SerializeField] float angleDivider = 8;
    [SerializeField] bool isEnableIncrementalRotationSnap = false;
    [SerializeField] bool isUsePositionSnap = false;
    [SerializeField] bool isDebugVisuals = false;

    public override void Snap(GameObject toSnap, Transform connector, GameObject endParent, Transform endConnector)
    {
        if (isUsePositionSnap)
            Utils_Rotations.PlaceAll(connector, endConnector, toSnap.transform);

        if (isEnableIncrementalRotationSnap)
        {
            Utils_Rotations.Get_LocalDirections(connector, toSnap.transform, out Vector3 forward, out Vector3 upwards);

            Transform oldParent = connector.parent;
            Vector3 oldPos = connector.localPosition;
            Quaternion oldRot = connector.localRotation;

            connector.SetParent(null);

            connector.transform.rotation = Utils_Rotations.IncrementalRotation_z(endConnector, endConnector.transform.InverseTransformRotation(connector.rotation).eulerAngles.z, angleDivider);
            toSnap.transform.rotation = Utils_Rotations.RotationInPlace(forward, upwards, connector); //rotationGameObject
            
            connector.SetParent(oldParent);
            connector.localPosition = oldPos;
            connector.localRotation = oldRot;
        }

        if (isDebugVisuals)
        {
            Utils_Rotations.Debug_DrawAllRays(endConnector.transform, angleDivider);
            Utils_Rotations.Debug_DrawTransform(endParent.transform, 0.1f);
            Utils_Rotations.Debug_DrawTransform(connector.transform, 0.1f);
            Utils_Rotations.Debug_DrawRayByAngle(endConnector, endConnector.transform.InverseTransformRotation(connector.rotation).eulerAngles.z);
            Utils_Rotations.Debug_DrawSnappedDirection(endConnector, endConnector.transform.InverseTransformRotation(connector.rotation).eulerAngles.z, angleDivider);

        }
    }
}
