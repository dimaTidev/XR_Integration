using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactible_retransmiter : MonoBehaviour, IInteractible_callbacks, IInteractible, IInteractibleConnection
{
    public GameObject reference;
    public event Func<GameObject> getRoot;
    public event Func<bool> isInteractible;
    public event Func<InteractionType, bool> isInteractible_byInteractionType;
    public event Action<GameObject>
       onDrop,
       onPickup;


    public GameObject Get_Root() => getRoot?.Invoke();
    public bool IsInteractible()
    {
        if (isInteractible != null)
            return isInteractible.Invoke();
        else
            return false;
    }

    public bool IsInteractible_byInteractionType(InteractionType interactionType) => isInteractible_byInteractionType.Invoke(interactionType);

    public void OnDrop(GameObject interactor)
    {
        Debug.Log("OnDrop: " + name);
        onDrop?.Invoke(interactor);
    }

    public void OnPickup(GameObject interactor) => onPickup?.Invoke(interactor);

    public GameObject ReferenceToConnection() => reference != null ? reference : gameObject;
}
