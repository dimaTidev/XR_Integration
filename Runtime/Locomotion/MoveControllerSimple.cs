using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControllerSimple : MonoBehaviour
{
    [SerializeField] Transform moveRelativeTo = null;
    [SerializeField] float rotationSpeed = 3;
    float rotationAngle;

    public void Move(Vector3 stickDirection)
    {
        Vector3 moveDir = moveRelativeTo.TransformDirection(stickDirection);
        moveDir.y = 0;
        transform.Translate(moveDir * Time.deltaTime);
    }

    public void Rotate(Vector3 stickDirection)
    {
        rotationAngle = Mathf.Lerp(rotationAngle, stickDirection.x * rotationSpeed, Time.deltaTime * 10);
        Vector3 rotDir = new Vector3(0, rotationAngle, 0);
        if (Mathf.Abs(rotDir.y) > 0.5f)
            transform.Rotate(rotDir * Time.deltaTime);
    }
}
