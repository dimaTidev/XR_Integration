using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Socket_sloted : ASocket, IInteractible_callbacks, ISocket_callbacks
{
    [SerializeField] Rigidbody connectTo = null;
    public event Action<bool[]> onSlotsChanged;
    [SerializeField] Transform[] triggerPoints; //triggers which trigger Interactibles for searching connection points
    GameObject[] slots; //slots which contain connected GameObjects

    void Start() => slots = new GameObject[triggerPoints.Length]; //create slots equals to triggers

    #region IInteractible_callbacks
    //-------------------------------------------------------------------------------------------------------------------------
    void IInteractible_callbacks.OnPickup(GameObject interactor)
    {
        DisconnectAll();
        Set_Enable(false);
    }
    void IInteractible_callbacks.OnDrop(GameObject interactor) => Set_Enable(true); //only droped/placed object can trigger and search for connection with interactible
                                                                                    //-------------------------------------------------------------------------------------------------------------------------
    #endregion

    //--- Disconnections ------------------------------------------------------------------------------------------
    public override void DisconnectAll()
    {
        if (slots != null)
           foreach (var conn in slots)
               if(conn != null)
                   Disconnect(conn);
    }

    //Realising method of emptying slots
    protected override void OnDisconected(GameObject forDisconnection)
    {
       for (int i = 0; i < slots.Length; i++)
       {
           if (slots[i] != null && forDisconnection != null && forDisconnection.GetInstanceID() == slots[i].gameObject.GetInstanceID())
           {
               Set_ToSlot(i, null);
               break;
           }
       }

        Rigidbody endRB = forDisconnection.GetComponent<Rigidbody>();
        if (endRB != null)
        {
            FixedJoint[] joints;

            if (connectTo)
                joints = connectTo.gameObject.GetComponents<FixedJoint>();
            else
                joints = gameObject.GetComponents<FixedJoint>();

            if (joints != null)
                for (int i = 0; i < joints.Length; i++)
                {
                    if(joints[i].connectedBody == endRB)
                    {
                        Destroy(joints[i]);
                        break;
                    }
                }
        }

        base.OnDisconected(forDisconnection);
    }
    protected override bool IsConnectedObjectsContaineInteractible(GameObject interactible)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && interactible != null && interactible.GetInstanceID() == slots[i].gameObject.GetInstanceID())
                return true;
        }
        return false;
    }

    //--- Connections ------------------------------------------------------------------------------------------
    //check if slot free
    protected override bool IsCanConnect(GameObject interactible) 
    {
       // return true;
       // if (interactible.GetComponent<Socket_sloted>() && interactible.GetComponent<Socket_sloted>().IsConnected(gameObject))
       //     return true;
        int idSlot = FindIdSlot(interactible);

        if (idSlot < 0 || idSlot >= slots.Length)
            return false;
        return !slots[idSlot];
    }

  //  bool IsConnected(GameObject interactible)
  //  {
  //      Debug.Log($"check is conncted {interactible.name} to {gameObject.name}");
  //      foreach (var connected in slots)
  //      {
  //          if (connected != null && interactible != null && connected.GetInstanceID() == interactible.GetInstanceID())
  //              return true;
  //      }
  //      return false;
  //  }

    protected override void Set_InteractibleObject(GameObject interactible, bool isAdd)
    {
        int idSlot = FindIdSlot(interactible);

        if(idSlot < 0 || idSlot >= slots.Length) 
            return;

        if (isAdd)
        {
            Set_InteractiblePosition(interactible);
            Set_ToSlot(idSlot, interactible);


            //TODO: make Connections without joints


            FixedJoint joint;

            if (connectTo)
                    joint = connectTo.gameObject.AddComponent<FixedJoint>();
            else
                    joint = gameObject.AddComponent<FixedJoint>();

            joint.connectedBody = interactible.GetComponent<Rigidbody>();
        }
    }

    [SerializeField] float sphereCheckSize = 0.01f; //spere size for checking in Check_forConnections_ByRays

    //forcing connect in all possible connections in current position
    //it calls when some interacted placed to this
    public void Check_forConnections_ByRays() 
    {
        for (int i = 0; i < triggerPoints.Length; i++)
        {
            if (slots[i] != null) //small optimisation to awoid ocuppanted slots
                continue;

            Collider[] colliders = Physics.OverlapSphere(triggerPoints[i].position, sphereCheckSize);
            foreach (var col in colliders)
            {
                int instanceID = col.gameObject.GetInstanceID();

                //avoid my instanceId
                if (instanceID == gameObject.GetInstanceID())
                    continue;

                bool isMatch = false;

                //avoid my triggers instance ID
                for (int k = 0; k < triggerPoints.Length; k++)
                {
                    if (instanceID == triggerPoints[k].gameObject.GetInstanceID())
                    {
                        isMatch = true;
                        continue;
                    }
                }
                if (isMatch)
                    continue;

                if (col.attachedRigidbody && col.attachedRigidbody.gameObject.GetComponent<Socket_sloted>())
                {
                    col.attachedRigidbody.gameObject.GetComponent<Socket_sloted>().TryConnectToSocket(gameObject); //we do connection from around GameObject because it will move only this GameObject.
                    //TryConnectToSocket(col.attachedRigidbody.gameObject);
                    break;
                }
            }
        }
    }

    protected override void Set_InteractiblePosition(GameObject interactible) => Snap(interactible);
    public override void Snap(GameObject toSnap)
    {
        if (!toSnap)
            return;

        if (!SnapOptions)
        {
            Debug.LogError("No snap options! " + name);
            return;
        }

        (Transform oriSnap, Transform mySnap) = FindClosestSnapPoints(toSnap);
        if (oriSnap && mySnap)
            SnapOptions.Snap(toSnap, oriSnap, gameObject, mySnap);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in triggerPoints)
            Gizmos.DrawWireSphere(point.position, sphereCheckSize);
    }

    #region ISocket_callbacks
    //-------------------------------------------------------------------------------------------------------------------------
    //TODO: Split ISocket_callbacks because some scripts don't need too much methods!
    void ISocket_callbacks.OnEnter(ASocket socket) { } //empty because don't needed
    void ISocket_callbacks.OnExit(ASocket socket) { } //empty because don't needed

    void ISocket_callbacks.OnConnected(ASocket socket)
    {
        //It's need for two sides connection, for avoiding bugs like when you place pipe between 2 pipes, your pipe will connect only to one pipe(
        //if(socket != null)
        TryConnectToSocket(socket.gameObject, true); //make silent double connections
        Check_forConnections_ByRays(); //this will force to connect all posible connections in current positions
       
    }

    void ISocket_callbacks.OnDisconected(ASocket socket) => OnDisconected(socket.gameObject); //OnDisconnect instead Disconnect because we want silent disconnect, otherwise you will case stack overflow
    //-------------------------------------------------------------------------------------------------------------------------
    #endregion

    #region Helpers
    int FindIdSlot(GameObject interactible)
    {
        (_, Transform mySnap) = FindClosestSnapPoints(interactible);

        if (mySnap)
        {
            for (int i = 0; i < triggerPoints.Length; i++)
            {
                if (mySnap.GetInstanceID() == triggerPoints[i].GetInstanceID())
                    return i;
            }
        }
        return -1;    
    }
    (Transform oriSnap, Transform mySnap) FindClosestSnapPoints(GameObject interactible)
    {
        if (!interactible)
        {
            Debug.LogError("interactible argument is null!!");
            return (transform, transform);
        }

        Socket_sloted endSocket = interactible.GetComponent<Socket_sloted>();

        (Transform oriSnap, Transform mySnap) pointsToSnap = (interactible.transform, null);
        float minDist = float.MaxValue;

        if (endSocket != null)
        {
            for (int i = 0; i < triggerPoints.Length; i++)
            {
                for (int k = 0; k < endSocket.triggerPoints.Length; k++)
                {
                    float dist = Vector3.Distance(triggerPoints[i].position, endSocket.triggerPoints[k].position);

                    if (dist > sphereCheckSize * 3) //if this distance to large, ignore it
                        continue;

                    if (dist < minDist)
                    {
                        minDist = dist;
                        pointsToSnap.oriSnap = endSocket.triggerPoints[k];
                        pointsToSnap.mySnap = triggerPoints[i];
                    }

                }
            }
        }
        else
        {
            pointsToSnap.oriSnap = interactible.transform;

            for (int i = 0; i < triggerPoints.Length; i++)
            {
                if (slots[i] != null)
                    continue;

                float dist = Vector3.Distance(triggerPoints[i].position, interactible.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    pointsToSnap.mySnap = triggerPoints[i];
                }

            }
        }

        if(pointsToSnap.oriSnap != null && pointsToSnap.mySnap != null)
            Debug.DrawLine(pointsToSnap.oriSnap.position, pointsToSnap.mySnap.position);

        return pointsToSnap;
    }
    #endregion


    void OnSlotsChanged()
    {
        bool[] slotsState = new bool[slots.Length];
        for (int i = 0; i < slots.Length; i++)
            slotsState[i] = slots[i];

        onSlotsChanged?.Invoke(slotsState);
    }

    void Set_ToSlot(int idSlot, GameObject go)
    {
        if (idSlot >= 0 && idSlot < slots.Length)
            slots[idSlot] = go;
        else
            Debug.LogError("idSlot is out of range of slots!!!");
        OnSlotsChanged();
    }
}
