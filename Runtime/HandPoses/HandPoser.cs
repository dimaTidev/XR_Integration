using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoser : MonoBehaviour
{
    public enum HandType { right, left }
    [SerializeField] HandType handType;

    [SerializeField] Transform[] points;
    public Transform[] Points => points;

    HandGrabPose.Pose pose;

    private void LateUpdate() => ApplyPose();

    void ApplyPose()
    {
        if (pose == null)
            return;

        for (int i = 0; i < points.Length; i++)
        {
            points[i].localPosition = pose.positions[i];
            points[i].localRotation = pose.rotations[i];
        }
    }

    /// <summary>
    /// If you want to snap gameObject, use this method
    /// </summary>
    /// <param name="interactible"></param>
    public void Set_Interactible(GameObject interactible)
    {
        if(interactible.GetComponent<HandGrabPose_provider>() is HandGrabPose_provider provider)
        {
            HandGrabPose handGrabPose = provider.FindNearestPose(transform.position, handType);

            if(handGrabPose.GetNearestPositionTo(transform.position, handType, out Vector3 handGrabPos))
            {
                transform.position = handGrabPos;

                if (!handGrabPose.TryGet_Rotation(handType, out Quaternion rotation))
                    rotation = transform.rotation;

                transform.rotation = rotation;
                Set_Pose(handGrabPose.Get_pose);
            }
        }
    }
    void Set_Pose(HandGrabPose.Pose pose) => this.pose = pose;
    public void ClearPose() => Set_Pose(null);
    //------------------------------------------------------------------------------------------------------
    [Header("---EDITOR ONLY----")]
    [SerializeField] HandGrabPose EDITOR_testHandGrabPose;
    [ContextMenu("Set_Pose")]
    public void Set_Pose() => Set_Pose(EDITOR_testHandGrabPose?.Get_pose);
}
