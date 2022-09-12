using UnityEngine;

[CreateAssetMenu(fileName = "SnapDirection", menuName = "ScriptableObjects/VR/Sockets/SnapDirection")]
public class Socket_snapDirection : ASocket_snapOption
{
    public enum Direction {right, left, up, down, forward, back} //be careful, if you add new direction you have to add... (watch down comment)
    [SerializeField] Direction direction = Direction.back;
    Vector3[] directions = { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back }; //... actual direction here too!

  //  [SerializeField] bool support_flip = false;

    public override void Snap(GameObject toSnap, Transform toSnapConnector, GameObject parent, Transform parentConnector)
    {
        if(!toSnap || !parent)
        {
            Debug.LogError("Wrong arguments!");
            return;
        }

        if((int)direction >= directions.Length)
        {
            Debug.LogError($"Index is out of direction array. For fix this, you have to add this [{direction}] direction to directions array");
            return;
        }
        Vector3 dir = directions[(int)direction];

        //   bool isFlip = false;
        //   if (support_flip)
        //   {
        //       if (Vector3.Dot(parent.transform.up, toSnap.transform.up) < 0) //they are looking to opposite direction
        //           isFlip = true;
        //
        //      //if (Vector3.Dot(parent.transform.right, toSnap.transform.right) < 0) //they are looking to opposite direction
        //      //    isFlip |= true;
        //   }

        Vector3 upVector = parent.transform.up;

        if (toSnap.GetComponent<SnapConstrains>() is SnapConstrains constains)
            upVector = constains.UpVector;

       // toSnap.transform.rotation = Quaternion.LookRotation(dir, parent.transform.up); //isFlip ? -parent.transform.up : parent.transform.up
        toSnap.transform.rotation = Quaternion.LookRotation(dir, upVector); //isFlip ? -parent.transform.up : parent.transform.up
    }
}
