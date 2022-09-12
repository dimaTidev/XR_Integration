using UnityEngine;

public interface IInteractible_callbacks
{
    void OnPickup(GameObject interactor);
    void OnDrop(GameObject interactor);
}
