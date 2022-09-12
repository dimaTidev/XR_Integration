using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor_Direct : AInteractor
{
    #region OnTriggerEnter_Exit
    //------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other) => AddInteractible(Get_InteractibleFromCollider(other));
    void OnTriggerExit(Collider other) => RemoveInteractible(Get_InteractibleFromCollider(other));
    //------------------------------------------------------------------------------------
    #endregion
}
