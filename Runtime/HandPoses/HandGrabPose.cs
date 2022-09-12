using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandPoser;

public class HandGrabPose : MonoBehaviour
{
    // [SerializeField] HandType handType;
    // [SerializeField] Vector3Int mirrors;
    [SerializeField] HandGrabData[] grabsData = new HandGrabData[1];

    [SerializeField] Vector3 localPosition;
    [SerializeField] Quaternion localRotation;
    [SerializeField] Pose pose;


    [System.Serializable]
    public class HandGrabData
    {
        public HandType handType;
        public Vector3 localPosition;
        public Quaternion localRotation;
    }


    public Vector3 Position => transform.TransformPoint(localPosition);
    //public Quaternion Rotation => transform.TransformRotation(localRotation);
    public bool TryGet_Rotation(HandType handType, out Quaternion result)
    {
        bool isFound = false;
        result = transform.rotation;

        foreach (var grabData in grabsData)
        {
            if (!grabData.handType.Equals(handType))
                continue;

            result = transform.TransformRotation(grabData.localRotation);
            isFound = true;
            break;
        }

        return isFound;
    }

    public bool GetNearestPositionTo(Vector3 pos, HandType handType, out Vector3 result)
    {
        bool isFound = false;
        result = transform.position;

        foreach (var grabData in grabsData)
        {
            if (!grabData.handType.Equals(handType))
                continue;

            result = transform.TransformPoint(grabData.localPosition);
            isFound = true;
            break;
        }

        if (!isFound)
            return false;

        if(GetComponent<AHandGrabSurface>() is AHandGrabSurface slider)
            result = slider.GetPoint(result, pos);

        return true;
    }

  //  public Quaternion GetNearestRotationTo(Quaternion rot, HandType handType)
  //  {
  //
  //      float minAngle = 360;
  //      Quaternion[] rotations = new Quaternion[]
  //      {
  //          Rotation,
  //          Rotation * transform.TransformRotation(Quaternion.Euler(0,180,0))
  //      };
  //      
  //
  //
  //      //if (GetComponent<AHandGrab_Slider>() is AHandGrab_Slider slider)
  //      //    return slider.GetPoint(Position, pos);
  //      //else
  //          return Rotation;
  //  }

    public Pose Get_pose => pose;

    [System.Serializable]
    public class Pose
    {
        public Vector3[] positions;
        public Quaternion[] rotations;

        public Pose(Transform[] points)
        {
            positions = new Vector3[points.Length];
            rotations = new Quaternion[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                positions[i] = points[i].localPosition;
                rotations[i] = points[i].localRotation;
            }
        }
    }



    [Header("---EDITOR ONLY----")]
    [SerializeField] HandPoser EDITOR_reader;
    [ContextMenu("Save pose")]
    public void SavePose() => SavePose(EDITOR_reader);

    public void SavePose(HandPoser reader)
    {
        localPosition = transform.InverseTransformPoint(reader.transform.position);
        localRotation = transform.InverseTransformRotation(reader.transform.rotation);
        pose = new Pose(reader.Points);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
    }

    private void OnDrawGizmosSelected()
    {
        if (GetComponent<AHandGrabSurface>() is AHandGrabSurface slider)
        {
            foreach (var grabData in grabsData)
            {
                Gizmos.color = Color.yellow;
                Vector3 pos = transform.TransformPoint(grabData.localPosition);
                Gizmos.DrawWireSphere(pos, 0.01f);
                slider.DrawGizmos(pos);

                float rayLength = 0.05f;

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(pos, transform.TransformRotation(grabData.localRotation) * Vector3.forward * rayLength);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(pos, transform.TransformRotation(grabData.localRotation) * Vector3.right * rayLength);
                Gizmos.color = Color.green;
                Gizmos.DrawRay(pos, transform.TransformRotation(grabData.localRotation) * Vector3.up * rayLength);

#if UNITY_EDITOR
                UnityEditor.Handles.Label(pos, grabData.handType.ToString());
#endif
            }
        }
    }
}
