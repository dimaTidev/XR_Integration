using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PC_Inputs : MonoBehaviour
{
    [SerializeField] UnityEvent_Vector3 onMove;
    [System.Serializable] public class UnityEvent_Vector3 : UnityEvent<Vector3> { }

    void Update()
    {
        Vector3 moveVector = new Vector3(
            Input.GetAxis("Horizontal"), 
            Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.LeftControl) ? -1 : 0, 
            Input.GetAxis("Vertical")
            );
        onMove?.Invoke(moveVector);
    }
}
