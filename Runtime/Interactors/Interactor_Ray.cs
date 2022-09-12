using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Ray : AInteractor
{
    [SerializeField] LayerMask rayMask;

    [SerializeField] Transform m_raycastOrigin;
    [SerializeField] Vector3 raycastDirection = Vector3.forward;
    [SerializeField] float rayDistance = 10;

    public Transform RaycastOrigin => m_raycastOrigin ??= transform;
    public Vector3 RaycastDirection => raycastDirection;
    public float RayDistance => rayDistance;
    
    protected virtual Ray Get_Ray() => new Ray(RaycastOrigin.position, RaycastOrigin.TransformDirection(raycastDirection));

    IInteractible lastInteractible;

    protected virtual void Update()
    {
        if (IsPickedUp)
            return;

        RaycastHit[] hit = Physics.RaycastAll(Get_Ray(), rayDistance, rayMask);

        for (int i = 0; i < hit.Length; i++)
        {
            IInteractible inter = Get_InteractibleFromCollider(hit[i].collider);
            if (inter != null && inter.IsInteractible())
            {
                if (inter.Equals(lastInteractible))
                    break;
                else
                {
                    RemoveInteractible(lastInteractible);
                    AddInteractible(inter);
                    lastInteractible = inter;
                    break;
                }   
            }
        }

        if(hit == null || hit.Length == 0 && lastInteractible != null)
        {
            RemoveInteractible(lastInteractible);
            lastInteractible = null;
        }

    }

    public override void Drop()
    {
        RemoveInteractible(lastInteractible);
        lastInteractible = null;
        base.Drop();
    }

    private void OnDrawGizmosSelected()
    {
        if (!RaycastOrigin)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(RaycastOrigin.position, 0.01f);
        Gizmos.DrawRay(RaycastOrigin.position, RaycastOrigin.TransformDirection(raycastDirection) * rayDistance);
    }
}
