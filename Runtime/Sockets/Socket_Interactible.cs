using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISocket_callbacks
{
    void OnEnter(ASocket socket);
    void OnExit(ASocket socket);
    void OnConnected(ASocket socket);
    void OnDisconected(ASocket socket);
}

//TODO: CLEAN! thats noo need anymore
public interface IInteractibleConnection
{
    GameObject ReferenceToConnection();
}

public class Socket_Interactible : ASocket_Interactible
{
    public override void DisconnectFromSockets(GameObject interactor)
    {
        if (tempSockets.Count > 0 && tempSockets[tempSockets.Count - 1]) //if this item currently in the socket
            tempSockets[tempSockets.Count - 1].Disconnect(gameObject);   // disconnect it
    }

    protected override GameObject Get_GhostReference() => gameObject;
}
