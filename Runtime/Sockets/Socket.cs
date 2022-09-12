using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Socket : ASocket
{
    [SerializeField] Transform socketPoint;
    GameObject connectedObject;

    [SerializeField] Vector3 localScale;



    protected override void OnConnected(GameObject interactible)
    {
        base.OnConnected(interactible);
        connectedObject = interactible;
    }
    protected override void OnDisconected(GameObject interactible)
    {
        base.OnDisconected(interactible);
        connectedObject = null;
    }

    public override void DisconnectAll() => Disconnect(connectedObject);

    public bool IsOccupied => connectedObject;

    protected override bool IsCanConnect(GameObject interactible) => !connectedObject;

    protected override bool IsConnectedObjectsContaineInteractible(GameObject interactible) => connectedObject && interactible && connectedObject.GetInstanceID() == interactible.GetInstanceID();

    protected override void Set_InteractibleObject(GameObject interactible, bool isAdd)
    {
        if(!isAdd)
        {
          if (interactible.GetComponent<Rigidbody>() != null)
              interactible.GetComponent<Rigidbody>().isKinematic = false;
            interactible.transform.SetParent(null);
            interactible.transform.localScale = Vector3.one;
            //connectedObject = null;
            return;
        }
        else
        {
           //connectedObject = interactible;
           Set_InteractiblePosition(interactible);
           interactible.transform.SetParent(socketPoint ? socketPoint : transform);
           interactible.transform.localScale = localScale;
           if (interactible.GetComponent<Rigidbody>() != null)
               interactible.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    protected override void Set_InteractiblePosition(GameObject interactible)
    {
        interactible.transform.position = socketPoint ? socketPoint.position : transform.position;
        interactible.transform.rotation = socketPoint ? socketPoint.rotation : transform.rotation;
    }
}
