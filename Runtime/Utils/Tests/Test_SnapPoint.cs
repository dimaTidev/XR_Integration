using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SnapPoint : MonoBehaviour
{
    [SerializeField] Transform connector = null;
    [SerializeField] Transform endConnector = null;
    [SerializeField] float gizmosSphereSize = 0.03f;

    [SerializeField] Transform fakeObject;
    [SerializeField] Transform fakeConnector;

    Vector3 oldPlace;

    #region Main
    //-------------------------------------------------------------------------------------------------------------
    Vector3 forward, upwards;
    Vector3 connectorLocalPos;
    Quaternion connectorLocalRot;
    Quaternion startRotation;

    [ContextMenu("RecordOld")]
    void Record_OldPosRot()
    {
        oldPlace = transform.position;
        startRotation = transform.rotation;
        connectorLocalPos = connector.localPosition;
        connectorLocalRot = connector.localRotation;
    }

    [ContextMenu("PlaceAll__")]
    void PlaceAll()
    {
        Record_OldPosRot();
        Utils_Rotations.PlaceAll(connector, endConnector, transform);
        ResetConnector();
    }

    [ContextMenu("ResetAll__")]
    void ResetAll()
    {
        ResetConnector();
        ResetRotation();
        ResetPlace();
    }

    //-------------------------------------------------------------------------------------------------------------
    #endregion

    [ContextMenu("_Place Position")] void PlacePos()
    {
        oldPlace = transform.position;
        transform.position = Utils_Rotations.SnapPosition(connector, endConnector, transform);
    }
    [ContextMenu("reset position")] void ResetPlace() => transform.position = oldPlace;
    
    [ContextMenu("_Place Connector")] void PlaceConnector()
    {
        CalculateDirections();
        Utils_Rotations.PlaceConnector(connector, endConnector);
    }
    [ContextMenu("Calculate Directions")] void CalculateDirections() => Utils_Rotations.Get_LocalDirections(connector, transform, out forward, out upwards);
    [ContextMenu("reset Connector")] void ResetConnector()
    {
        connector.SetParent(transform);
        connector.localPosition = connectorLocalPos;
        connector.localRotation = connectorLocalRot;
    }

    [ContextMenu("_Place Rotation")] void RotateInPlace()
    {
        startRotation = transform.rotation;
        transform.rotation = Utils_Rotations.RotationInPlace(forward, upwards, connector); //transform
        Debug.Log("Rotated");
    }
    [ContextMenu("reset rotation")] void ResetRotation() => transform.rotation = startRotation;

    [SerializeField] bool isDrawFake = false;

    private void OnDrawGizmos()
    {
        Vector3 gizmosPoint = Utils_Rotations.SnapPosition(connector, endConnector, transform);
        Gizmos.DrawSphere(gizmosPoint, gizmosSphereSize);

      // Gizmos.color = Color.yellow;
      // Gizmos.DrawRay(transform.position, connector.TransformDirection(upwards));

        Vector3 zDir = connector.TransformDirection(forward);
        Vector3 xDir = Vector3.Cross(zDir, connector.TransformDirection(upwards)) * -1;
        Vector3 yDir = Vector3.Cross(zDir, xDir);

        //z axis
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, zDir);

        //x axis
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, xDir);

        //y axis
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, yDir);

        


        if (isDrawFake && fakeObject && fakeConnector)
         {
             Utils_Rotations.PlaceAll(fakeConnector, endConnector, fakeObject);
         }
    }
}
