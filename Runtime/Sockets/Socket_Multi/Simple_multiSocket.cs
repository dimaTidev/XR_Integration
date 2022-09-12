using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What if I grab hanging and after this I grab origin ???
public class Simple_multiSocket : MonoBehaviour
{
    [SerializeField] ASocket_Interactible interactibleA = null;
    [SerializeField] ASocket_Interactible interactibleB = null;

    [SerializeField] Vector3 offsetHanging = new Vector3(0,0.1f,0);
    [SerializeField] Vector3 offsetFree = new Vector3(0,0.1f,0);

    private void Start()
    {
        interactibleA.OnDrop_event += () => { OnDrop(interactibleA); };
        interactibleB.OnDrop_event += () => { OnDrop(interactibleB); };

        interactibleA.OnPickup_event += () => { OnPickup(interactibleA); };
        interactibleB.OnPickup_event += () => { OnPickup(interactibleB); };

        interactibleA.OnDisconnected_event += () => { OnDisconected(interactibleA); };
        interactibleB.OnDisconnected_event += () => { OnDisconected(interactibleB); };

        HideOposite(interactibleA);
    }

    void OnPickup(ASocket_Interactible interactible)
    {
        interactible.transform.SetParent(null);
        if (!interactibleA.LastTempSocket && !interactibleB.LastTempSocket) //if no one in socket
        {
            //no one hanging all not placed
            HideOposite(interactible);
            //Disconnect opposite if grabbed
        }
    }
    void OnDisconected(ASocket_Interactible interactible) => Organize();

    void OnDrop(ASocket_Interactible interactible) => Organize();

    ASocket_Interactible Opposite_interactible(ASocket_Interactible interactible)
    {
        if (interactible.GetInstanceID() == interactibleA.GetInstanceID())
            return interactibleB;
        else
            return interactibleA;
    }

    void HideOposite(ASocket_Interactible interactible)
    {
        ASocket_Interactible opposite = Opposite_interactible(interactible);
        opposite.gameObject.SetActive(false);
        opposite.transform.SetParent(interactible.transform);
        opposite.transform.localPosition = offsetFree;// interactible.transform.TransformPoint(offsetFree); //interactible.transform.position + 
        opposite.transform.localRotation = Quaternion.identity;// interactible.transform.TransformPoint(offsetFree); //interactible.transform.position + 
    }

    void Organize()
    {
        if (interactibleA.LastTempSocket == null && interactibleB.LastTempSocket != null)
        {
            //A hanging
            interactibleA.GetComponent<Rigidbody>().isKinematic = true;
            interactibleB.GetComponent<Rigidbody>().isKinematic = true;
            interactibleA.transform.position = interactibleB.transform.position + offsetHanging;
            interactibleA.gameObject.SetActive(true);
        }
        else if (interactibleA.LastTempSocket != null && interactibleB.LastTempSocket == null)
        {
            //B hanging
            interactibleA.GetComponent<Rigidbody>().isKinematic = true;
            interactibleB.GetComponent<Rigidbody>().isKinematic = true;
            interactibleB.transform.position = interactibleA.transform.position + offsetHanging;
            interactibleB.gameObject.SetActive(true);
        }
        else if(interactibleA.LastTempSocket && interactibleB.LastTempSocket)
        {
            //no one hanging all placed
            interactibleA.GetComponent<Rigidbody>().isKinematic = true;
            interactibleB.GetComponent<Rigidbody>().isKinematic = true;
            interactibleA.gameObject.SetActive(true);
            interactibleB.gameObject.SetActive(true);
        }
    }
}
