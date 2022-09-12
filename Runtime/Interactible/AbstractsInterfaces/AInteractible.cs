using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AInteractible : MonoBehaviour, IInteractible_callbacks, IInteractible
{
    public virtual GameObject Get_Root() => gameObject;

    [SerializeField] protected bool isOneHandGrabOnly = false;
    [SerializeField] bool isInteractible;
    [SerializeField] UnityEvent_GameObject onPickup;
    [SerializeField] UnityEvent_GameObject onDrop;

    List<GameObject> interactors = new List<GameObject>();
    protected int InteractorsCount => interactors.Count;

    [System.Serializable] public class UnityEvent_GameObject : UnityEvent<GameObject> { }

    [SerializeField] bool isInteractibleByTrigger = true;
    [SerializeField] bool isInteractibleByGrip = true;

    public bool IsInteractible() => isInteractible;
    public bool IsInteractible_byInteractionType(InteractionType interactionType)
    {
        if (interactionType == InteractionType.trigger && isInteractibleByTrigger)
            return true;
        else
             if (interactionType == InteractionType.grip && isInteractibleByGrip)
            return true;
        return false;
    }

    public virtual void OnDrop(GameObject interactor)
    {
        interactors.Remove(interactor);
        //this.interactor = null;
        onDrop?.Invoke(interactor);
    }

    public virtual void OnPickup(GameObject interactor)
    {
        if (isOneHandGrabOnly)
            Set_Drop();

        if (!interactors.Contains(interactor))
            interactors.Add(interactor);
        onPickup?.Invoke(interactor);
    }

    public void Set_Drop()
    {
        for (int i = 0; i < interactors.Count; i++)
        {
            if (interactors[i] != null && interactors[i].GetComponent<IInteractor>() != null)
            {
                interactors[i].GetComponent<IInteractor>().Drop(); //it will call OnDrop
                OnDrop(interactors[i]); //But it is for sure if some did not call OnDrop
            }
                    
        }
    }
}
