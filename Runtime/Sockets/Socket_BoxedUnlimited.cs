using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket_BoxedUnlimited : ASocket
{
    [SerializeField] Transform socketPoint;
    [SerializeField] bool isSnapPosition = true;
    List<GameObject> tempObjects = new List<GameObject>();

    protected override bool IsCanConnect(GameObject interactible) => true;
    protected override bool IsConnectedObjectsContaineInteractible(GameObject interactible) => tempObjects.Contains(interactible);

    protected override void Set_InteractibleObject(GameObject interactible, bool isAdd)
    {
        if (interactible && !isAdd)
        {
            if (interactible.GetComponent<Rigidbody>() != null)
                interactible.GetComponent<Rigidbody>().isKinematic = false;
            if (tempObjects.Contains(interactible))
                tempObjects.Remove(interactible);
            interactible.transform.SetParent(null);
            return;
        }

        if (isAdd)
        {
            tempObjects.Add(interactible);
            Snap(interactible);
            interactible.transform.SetParent(socketPoint ? socketPoint : transform);
            if (interactible.GetComponent<Rigidbody>() != null)
                interactible.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    protected override void Set_InteractiblePosition(GameObject interactible)
    {
        if (isSnapPosition)
        {
            Vector3 planeNormal = transform.forward;

            Vector3 pos = Vector3.ProjectOnPlane(interactible.transform.position, planeNormal);
            pos.z = socketPoint ? socketPoint.transform.position.z : transform.position.z;
            interactible.transform.position = pos;
        }
    }

    public override void Snap(GameObject toSnap)
    {
        Set_InteractiblePosition(toSnap);
        base.Snap(toSnap);
     }

    public override void DisconnectAll()
    {
        for (int i = tempObjects.Count - 1; i >= 0; i--)
            Disconnect(tempObjects[i]);
    }
}
