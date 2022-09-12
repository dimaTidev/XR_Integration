using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket_LoaderMulti : MonoBehaviour
{
    [SerializeField] ASocket[] sockets;
    [SerializeField] GameObject[] prefabs;

    void Start()
    {
        RefreshSlots();
        for (int i = 0; i < sockets.Length && i < prefabs.Length; i++)
        {
            if (sockets[i] == null)
            {
                Debug.LogError("Socket don't contain IInteractible_callbacks on gameObject: " + name);
                return;
            }

            GameObject go = Instantiate(prefabs[i]);

            if (!sockets[i].TryConnectToSocket(go))
            {
                Destroy(go);
                Debug.LogWarning("Something went wrong. Immposible connect interactible prefab on gameObject: " + name);
            }
        }
    }

    void RefreshSlots()
    {
        for (int i = 0; i < sockets.Length; i++)
        {
            if (i < prefabs.Length)
                sockets[i].gameObject.SetActive(true);
            else if (i >= prefabs.Length)
                sockets[i].gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] != null && prefabs[i].GetComponent<IInteractible>() == null)
            {
                prefabs[i] = null;
                Debug.LogError("Incorrect prefab. Please, select prefab with IInteractible");
            }
        }
    }


   
}
