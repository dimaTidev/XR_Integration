using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ASocket))]
public class Socket_Loader : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    void Start()
    {
        ASocket socket = GetComponent<ASocket>();

        if (socket == null)
        {
            Debug.LogError("Socket don't contain IInteractible_callbacks on gameObject: " + name);
            return;
        }
            
        GameObject go = Instantiate(prefab);

        if (!socket.TryConnectToSocket(go))
        {
            Destroy(go);
            Debug.LogWarning("Something went wrong. Immposible connect interactible prefab on gameObject: " + name);
        }
    }


    private void OnValidate()
    {
        if(prefab != null && prefab.GetComponent<IInteractible>() == null)
        {
            prefab = null;
            Debug.LogError("Incorrect prefab. Please, select prefab with IInteractible");
        }
    }
}
