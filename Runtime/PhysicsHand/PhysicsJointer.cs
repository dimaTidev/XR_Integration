using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsJointer : MonoBehaviour
{
    [SerializeField] bool isUseBreakForce = true;
    [SerializeField] private float breakForce = 10;
    [SerializeField] private float massScale = 1;
    [SerializeField] private float connectedMassScale = 1;
    [SerializeField] UnityEvent onBreakJoint = null;
    [SerializeField] bool isUseDoubleJoint = true;
    FixedJoint m_attachJoint = null;
    FixedJoint m_attachJoint_oposite = null;
    FixedJoint attachJoint
    {
        get
        {
            if (!m_attachJoint)
                m_attachJoint = CreateJoint(gameObject);
            return m_attachJoint;
        }
       // set
       // {
       //     if (m_attachJoint)
       //         Destroy(m_attachJoint);
       //     m_attachJoint = value;
       // }
    }

    FixedJoint CreateJoint(GameObject jointFor)
    {
        FixedJoint joint = jointFor.AddComponent<FixedJoint>();
        if (isUseBreakForce)
            joint.breakForce = breakForce;
        joint.massScale = massScale;
        joint.connectedMassScale = connectedMassScale;
        return joint;
    }

    public void DestroyJoint()
    {
        //ConfigureJoint(null);
        if (m_attachJoint)
            Destroy(m_attachJoint);
        if (m_attachJoint_oposite)
            Destroy(m_attachJoint_oposite);
    }

    /// <param name="go">Null will destroy exist joint</param>
    public void ConfigureJoint(GameObject go)
    {
        if (!go)
        {
            DestroyJoint();
            //attachJoint = null;
            return;
        }
            
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (!rb)
            return;

        attachJoint.connectedBody = rb;
        if (isUseDoubleJoint)
        {
            if(!m_attachJoint_oposite)
                m_attachJoint_oposite = CreateJoint(rb.gameObject);
            m_attachJoint_oposite.connectedBody = GetComponent<Rigidbody>();
        }

    }

    void OnJointBreak(float breakForce) => onBreakJoint?.Invoke(); //that fix bug when you pickup by two hands and break joint without release button and data about object still in the disconnected hand
}
