using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static HandPoser;

public class HandGrabPose_provider : MonoBehaviour
{
    [SerializeField] HandGrabPose[] poses;



    public HandGrabPose FindNearestPose(Vector3 pos, HandType handType)
    {
        float minDist = float.MaxValue;
        int idPose = -1;
        for (int i = 0; i < poses.Length; i++)
        {
            if (!poses[i].GetNearestPositionTo(pos, handType, out Vector3 handGrabPos))
                continue;
            float dist = Vector3.Distance(pos, handGrabPos);
            if(dist < minDist)
            {
                minDist = dist;
                idPose = i;
            }
        }
        return poses[idPose];
    }

    [Header("---For tests---")]
    [SerializeField] Transform gizmos_target;
    [SerializeField] float gizmos_sphereSize = 0.05f;
    private void OnDrawGizmos()
    {
        if (gizmos_target)
        {
            HandGrabPose pose = FindNearestPose(gizmos_target.position, HandType.right);

            if (!pose.GetNearestPositionTo(gizmos_target.position, HandType.right, out Vector3 grabPos))
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(grabPos, gizmos_sphereSize);
            Gizmos.DrawSphere(gizmos_target.position, gizmos_sphereSize);
            Gizmos.DrawLine(gizmos_target.position, grabPos);
        }
    }
}
