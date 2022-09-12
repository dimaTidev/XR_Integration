using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class ASocket_Interactible : AInteractible, ISocket_callbacks, IInteractible_callbacks
{
    [SerializeField] bool isUseGhost;
    GameObject ghost;

    protected List<ASocket> tempSockets = new List<ASocket>();
    public List<ASocket> TempSockets => tempSockets;
    public ASocket LastTempSocket => tempSockets.Count > 0 ? tempSockets[tempSockets.Count - 1] : null;

    [SerializeField] UnityEvent_GameObject onEnter;
    [SerializeField] UnityEvent_GameObject onExit;
    [SerializeField] UnityEvent_GameObject onConnected;
    [SerializeField] UnityEvent_GameObject onDisconected;

    public event Action 
        OnDrop_event,
        OnPickup_event,
        OnDisconnected_event;
    //[System.Serializable] public class UnityEvent_GameObject : UnityEvent<GameObject> { }

    void OnDisable()
    {
        if (ghost)
            DestroyImmediate(ghost);
    }

    void Update_ghostTracking() //execute only when subscibed
    {
        if (InteractorsCount != 0 && ghost && tempSockets.Count > 0)
        {
            ghost.transform.position = transform.position; //reset position for fix Bug - defining wrong connection point after previous snap
            ghost.transform.rotation = transform.rotation; //reset rotation for fix Bug - defining wrong connection point after previous snap
            foreach (var socket in tempSockets) //because we have to going through list of sockets
                socket.Snap(ghost);
        }
    }

    //OnEnter/OnExit/OnConnected/OnDisconected
    #region ISocket_callbacks
    //------------------------------------------------------------------------------------------------------------------------------
    public virtual void OnEnter(ASocket socket)
    {
        if (InteractorsCount == 0)
            return;

        if (!tempSockets.Contains(socket))
            tempSockets.Add(socket);

        onEnter?.Invoke(socket.gameObject);

        if (ghost) ghost.SetActive(true);
    }
    public virtual void OnExit(ASocket socket)
    {
        if (InteractorsCount == 0)
            return;

        tempSockets.Remove(socket);

        onExit?.Invoke(socket.gameObject);

        if (ghost && tempSockets.Count == 0) ghost.SetActive(false);
    }
    public virtual void OnConnected(ASocket socket)
    {
        tempSockets.Clear();
        tempSockets.Add(socket); //that will fix missing tempSockets

        onConnected?.Invoke(socket.gameObject);
    }

    public virtual void OnDisconected(ASocket socket)
    {
        tempSockets.Remove(socket);
        onDisconected?.Invoke(socket.gameObject);
        OnDisconnected_event?.Invoke();
    }

    //------------------------------------------------------------------------------------------------------------------------------
    #endregion

    #region IInteractible_callbacks
    //------------------------------------------------------------------------------------------------------------------------------
    public override void OnDrop(GameObject interactor)
    {
        base.OnDrop(interactor);
        if (InteractorsCount == 0) //if no interactors then try to connect
        {
            if (tempSockets.Count > 0)
                tempSockets[tempSockets.Count - 1].TryConnectToSocket(gameObject);
       
            UpdateSingleton.RemoveFrom_Update(Update_ghostTracking);  //Unsubscribe For Updates
            Destroy_Ghost();
        }

        OnDrop_event?.Invoke();
    }
    public override void OnPickup(GameObject interactor)
    {
        base.OnPickup(interactor);

        DisconnectFromSockets(interactor);  //if this item currently in the socket, disconnect it. BUT!! some scripts make decision do it now or not!

        tempSockets.Clear(); //we have to clear previous list before we use new interaction

        if (isUseGhost)  //Create Ghost
        {
            UpdateSingleton.ExecuteIn_Update(Update_ghostTracking); //Subscribe For Updates, only when item is picked up
            Create_Ghost();
            ghost.SetActive(false);
        }

        OnPickup_event?.Invoke();
    }
    //------------------------------------------------------------------------------------------------------------------------------
    #endregion


   // protected abstract GameObject Get_SocketObject(GameObject interactor);
    public abstract void DisconnectFromSockets(GameObject interactor = null);
    protected abstract GameObject Get_GhostReference();

    #region Ghost
    protected void Create_Ghost()
    {
        if (ghost)
            Destroy(ghost);

        ghost = Utils_Sockets.Clone(Get_GhostReference()); //make full clone without colliders
        ghost.name += "_GHOST";
        //ghost.hideFlags = HideFlags.HideAndDontSave;
    }

    void Destroy_Ghost()
    {
        if (ghost)
            Destroy(ghost);
    }
    #endregion 
}
