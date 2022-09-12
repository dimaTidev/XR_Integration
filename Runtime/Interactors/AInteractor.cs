using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionType { grip, trigger };

public abstract class AInteractor : MonoBehaviour, IInteractor
{
    List<IInteractible> interactibles = new List<IInteractible>();
    IInteractible pickedInteractible = null;
    public bool IsPickedUp => pickedInteractible != null;

    [SerializeField] UnityEvent_GameObject onPickup;
    [SerializeField] UnityEvent_GameObject onDrop;

    [System.Serializable] public class UnityEvent_GameObject : UnityEvent<GameObject> { }

    void OnDisable()
    {
        Drop();
        interactibles.Clear();
    }

    #region PickupDrop
    //------------------------------------------------------------------------------------
    
    void Pickup(InteractionType interactionType)
    {
        if (interactibles.Count > 0 && pickedInteractible == null)
        {
            pickedInteractible = Get_ClosestInteractible(transform.position, interactibles);
            if (pickedInteractible.IsInteractible_byInteractionType(interactionType))
            {
                pickedInteractible.OnPickup(gameObject);
                onPickup?.Invoke(pickedInteractible.Get_Root());
                //Debug.Log($"pickedUp: {pickedInteractible.Get_Root().name}");
            }
        }
    }

    protected void AddInteractible(IInteractible interactible)
    {
        if (interactible != null && interactible.IsInteractible() && !interactibles.Contains(interactible)) // && !interactibles.Contains(interactible) //because if composit collider then we have a bug when you exit from sub collider it remove entire object from list
        {
            interactibles.Add(interactible);
            //return true;
            //Debug.Log($"add interactible[{other.name}]: {interactibles.Count}");
        }
        //return false;
    }

    protected void RemoveInteractible(IInteractible interactible)
    {
        if (interactible != null)
        {
            interactibles.Remove(interactible);
            //Debug.Log($"remove interactible[{other.name}]: {interactibles.Count}");
        }
    }

    [ContextMenu("Pickup_byGrip")]
    public void Pickup_byGrip() => Pickup(InteractionType.grip);

    [ContextMenu("Pickup_byTrigger")]
    public void Pickup_byTrigger() => Pickup(InteractionType.trigger);

    [ContextMenu("Drop")]
    public virtual void Drop()
    {
        if (pickedInteractible == null)
            return;

        //Debug.Log($"Drop: {pickedInteractible.Get_Root().name}");
        pickedInteractible.OnDrop(gameObject);
        onDrop?.Invoke(gameObject);
       // onPickup?.Invoke(null);
        pickedInteractible = null;
    }
    //------------------------------------------------------------------------------------
    #endregion


    /// <summary>
    /// Find closest interactible from list
    /// </summary>
    /// <param name="point">Point for check distance to interactible</param>
    /// <returns>Closest interactible</returns>
    IInteractible Get_ClosestInteractible(Vector3 point, List<IInteractible> interactibles)
    {
        IInteractible closest = null;

        if (interactibles != null)
        {
            float closestDistance = float.MaxValue;
            foreach (var interactible in interactibles)
            {
                float dist = Vector3.Distance(point, interactible.Get_Root().transform.position);
                if (dist <= closestDistance)
                {
                    closestDistance = dist;
                    closest = interactible;
                }
            }
        }
        return closest;
    }

    protected IInteractible Get_InteractibleFromCollider(Collider collider)
    {
        IInteractible interactible = collider.GetComponent<IInteractible>();
        if (interactible == null && collider.attachedRigidbody)
            interactible = collider.attachedRigidbody.GetComponent<IInteractible>();

        return interactible;
    }

#if UNITY_EDITOR

    [ContextMenu("Log possible count")]
    void Context_PossibleCount() => Debug.Log($"interactibles: {interactibles.Count}");

    [ContextMenu("Log possible names")]
    void Context_PossibleAllNames()
    {
        string deb = "";
        for (int i = 0; i < interactibles.Count; i++)
            deb += interactibles[i].Get_Root().name + "  /";
        Debug.Log($"interactibles: {deb}");
    }

    [ContextMenu("Log IsPickedUp now")]
    void Context_IsPickedUp() => Debug.Log(gameObject.name + " IsPickedUp: " + IsPickedUp);

#endif

}
