using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Socket_interactible_retransmiter : MonoBehaviour, ISocket_callbacks
{
    public event Action<ASocket>
        onConnected,
        onDisconected,
        onEnter,
        onExit;

    public void OnConnected(ASocket socket) => onConnected?.Invoke(socket);
    public void OnDisconected(ASocket socket) => onDisconected?.Invoke(socket);
    public void OnEnter(ASocket socket) => onEnter?.Invoke(socket);
    public void OnExit(ASocket socket) => onExit?.Invoke(socket);
}
