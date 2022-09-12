using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ASocket : MonoBehaviour
{
    [SerializeField] bool isEnable;
    protected bool IsEnable => isEnable;
    [SerializeField] string[] tags;
    [SerializeField] ASocket_snapOption snapOptions = null;
    protected ASocket_snapOption SnapOptions => snapOptions;

    [SerializeField] UnityEvent_GameObject onEnter;
    [SerializeField] UnityEvent_GameObject onExit;
    [SerializeField] UnityEvent_GameObject onConnected;
    [SerializeField] UnityEvent_GameObject onDisconected;
    [System.Serializable] public class UnityEvent_GameObject : UnityEvent<GameObject> { }

    public void Set_Enable(bool isEnable) => this.isEnable = isEnable; //switch trigger detection

    #region Calbacks
    //---------------------------------------------------------------------------------------------------------------------------
    protected virtual void OnConnected(GameObject interactible) => onConnected?.Invoke(interactible.gameObject);
    protected virtual void OnDisconected(GameObject interactible) => onDisconected?.Invoke(interactible.gameObject);
    protected virtual void OnEnter(GameObject interactible) => onEnter?.Invoke(interactible.gameObject);
    protected virtual void OnExit(GameObject interactible) => onExit?.Invoke(interactible.gameObject);
    //---------------------------------------------------------------------------------------------------------------------------
    #endregion

    //--- Disconnections ------------------------------------------------------------------------------------------------------------
    public void Disconnect(GameObject interactible)
    {
        if (!interactible)
            return;


        IInteractibleConnection iRefConn = interactible.GetComponent<IInteractibleConnection>();
        GameObject connReference = iRefConn != null ? iRefConn.ReferenceToConnection() : interactible;

        if (IsConnectedObjectsContaineInteractible(connReference))
        {
            ISocket_callbacks[] callbacks = interactible.GetComponents<ISocket_callbacks>();
            if (callbacks != null)
                foreach (var callback in callbacks)
                    callback.OnDisconected(this);

            OnDisconected(connReference);

            Set_InteractibleObject(interactible, false);
        }
    }
    public abstract void DisconnectAll();

    //--- Connections ------------------------------------------------------------------------------------------------------------
    protected virtual bool Check_IsObjectSuitable(GameObject go)
    {
        if (tags == null || tags.Length == 0)
            return true; 

        if(go == null)
            return false;
        

        foreach (var tag in tags)
        {
            if (go.CompareTag(tag))
                return true;
        }
        return false;
    }
    public bool TryConnectToSocket(GameObject interactible, bool silent = false)
    {
        if (!interactible || !Check_IsObjectSuitable(interactible))
        {
            if(interactible)
                Debug.LogWarning($"__Try to connect but interactible is not suitable: {interactible.name}");
            return false;
        }

        // Debug.Log($"__Try to connect {interactible.name} to {gameObject.name} silent: {silent}");

        IInteractibleConnection iRefConn = interactible.GetComponent<IInteractibleConnection>();
        GameObject connReference = iRefConn != null ? iRefConn.ReferenceToConnection() : interactible;

        if (IsCanConnect(connReference))
        {
            ISocket_callbacks[] callbacks = interactible.GetComponents<ISocket_callbacks>();
            if (callbacks != null)
            {
                Set_InteractibleObject(interactible, true);
                if(!silent)
                    foreach (var callback in callbacks)
                        callback.OnConnected(this);

                OnConnected(connReference);
                return true;
            }
        }
        Debug.LogWarning($"Slot is already occupied or Incorrect interactible (name: {interactible.name}). Please, put interactible with ISocket_callbacks. Invoker: " + name);
        return false;
    }

    //TODO: Do we realy need this method? looks like a useless method
    protected abstract bool IsConnectedObjectsContaineInteractible(GameObject interactible);
    protected abstract bool IsCanConnect(GameObject interactible);

    //TODO: Actualy we can remove argument (bool isAdd). Because we can handle disconnection objec by OnDisconected
    protected abstract void Set_InteractibleObject(GameObject interactible, bool isAdd);

    //TODO: Seems like we can replace this method by `Snap` method
    protected abstract void Set_InteractiblePosition(GameObject interactible);
    public virtual void Snap(GameObject toSnap)
    {
        if (snapOptions)
            snapOptions.Snap(toSnap, gameObject);
        else
            Set_InteractiblePosition(toSnap);
    }

    #region TriggerEvents
    private void OnTriggerEnter(Collider other)
    {
        if (!IsEnable)
            return;

        GameObject interactible = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        if (!Check_IsObjectSuitable(interactible.gameObject) || !IsCanConnect(interactible))
            return;

        ISocket_callbacks[] callbacks = interactible.GetComponents<ISocket_callbacks>();
        if (callbacks != null)
        {
            OnEnter(interactible);
            foreach (var callback in callbacks)
                callback.OnEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsEnable)
            return;

        GameObject interactible = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        if (!Check_IsObjectSuitable(interactible.gameObject))
            return;

        ISocket_callbacks[] callbacks = interactible.GetComponents<ISocket_callbacks>();
        if (callbacks != null)
        {
            OnExit(interactible);
            foreach (var callback in callbacks)
                callback.OnExit(this);
        }
    }

    #endregion
}
