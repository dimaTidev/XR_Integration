using UnityEngine;

public interface IInteractible : IInteractible_callbacks
{
    bool IsInteractible();
    bool IsInteractible_byInteractionType(InteractionType interactionType);
    GameObject Get_Root();
}
