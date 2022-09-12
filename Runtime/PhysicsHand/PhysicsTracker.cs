using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsTracker : MonoBehaviour
{
    [SerializeField] Transform followTarget = null;
    [Space]
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    [Space]
    [SerializeField] float followSpeed = 30f;
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] float mass = 20f;
    [SerializeField] float maxAngularVelocity = 20f;
   // [SerializeField] bool useGravity = false;
    [SerializeField] RigidbodyInterpolation rigidbodyInterpolation = RigidbodyInterpolation.Interpolate;
    [SerializeField] CollisionDetectionMode collisionDetectionMode = CollisionDetectionMode.Continuous;


    Rigidbody m_rigidBody;
    Rigidbody RBody => m_rigidBody ??= GetComponent<Rigidbody>();

    void Start()
    {
        RBody.collisionDetectionMode = collisionDetectionMode;
        RBody.interpolation = rigidbodyInterpolation;
        RBody.mass = mass;
        RBody.maxAngularVelocity = maxAngularVelocity;
        RBody.useGravity = false;
        RBody.isKinematic = false;
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }


    void Move()
    {
        Vector3 positionWithOffset = followTarget.TransformPoint(positionOffset);
        float distance = Vector3.Distance(positionWithOffset, transform.position);
        RBody.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance * Time.fixedDeltaTime);
    }

    void Rotate()
    {
        Quaternion rotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion q = rotationWithOffset * Quaternion.Inverse(RBody.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        if (Mathf.Abs(axis.magnitude) != Mathf.Infinity)
        {
            if (angle > 180.0f)  
                angle -= 360.0f;
            RBody.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed * Time.fixedDeltaTime);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Rigidbody RigidBody = GetComponent<Rigidbody>();
        if (RigidBody.collisionDetectionMode != collisionDetectionMode)
            RigidBody.collisionDetectionMode = collisionDetectionMode;
        if (RigidBody.interpolation != rigidbodyInterpolation)
            RigidBody.interpolation = rigidbodyInterpolation;
        if (RigidBody.mass != mass)
            RigidBody.mass = mass;
        if (RigidBody.maxAngularVelocity != maxAngularVelocity)
            RigidBody.maxAngularVelocity = maxAngularVelocity;
        if (RigidBody.useGravity != false)
            RigidBody.useGravity = false;
        if (RigidBody.isKinematic != false)
            RigidBody.isKinematic = false;
    }
#endif
}
