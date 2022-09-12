using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Socket_snapRadial", menuName = "ScriptableObjects/VR/Sockets/SnapRadial")]
public class Socket_snapRadial : Socket_snapDirection
{
    [SerializeField] int angleCounts = 8;
    [SerializeField] float snapDistance = 1;

    public override void Snap(GameObject toSnap, Transform toSnapConnector, GameObject parent, Transform parentConnector)
    {
        base.Snap(toSnap, toSnapConnector, parent, parentConnector);
        float angleStep = 360f / 8f;
        float angle = angleStep / 2f;

        Vector3[] dirs = new Vector3[angleCounts];

        for (int i = 0; i < dirs.Length; i++)
        {
            Vector3 tempVector = Quaternion.AngleAxis(angle, Vector3.forward) * parent.transform.up;
            dirs[i] = tempVector.normalized;
            angle += angleStep;
        }

        float max = 0;
        int id = 0;
        Vector3 dirToTarget = (toSnap.transform.position - parent.transform.position).normalized;

        for (int i = 0; i < dirs.Length; i++)
        {
            float dot = Vector3.Dot(dirs[i], dirToTarget);
            //Debug.Log($"dot[{i}]: {dot} ({dirs[i]}) ");
            if (dot > max)
            {
                max = dot;
                id = i;
            }
        }
        //Debug.Log($"id:{id}----{dirToTarget}----");
        toSnap.transform.position = parent.transform.position + dirs[id] * snapDistance;

        for (int i = 0; i < dirs.Length; i++)
        {
            if(id != i)
                Debug.DrawRay(parent.transform.position, dirs[i]);
        }
        Debug.DrawLine(parent.transform.position, dirs[id], Color.yellow);
        Debug.DrawRay(parent.transform.position, dirToTarget, Color.green);
    }
}
