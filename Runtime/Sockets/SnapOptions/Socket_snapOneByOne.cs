using UnityEngine;

[CreateAssetMenu(fileName = "snapOneByOne", menuName = "ScriptableObjects/VR/Sockets/SnapOneByOne")]
public class Socket_snapOneByOne : ASocket_snapOption
{
    public override void Snap(GameObject toSnap, Transform toSnapConnector, GameObject parent, Transform parentConnector)
    {
        if(!toSnap || !parentConnector)
        {
            Debug.LogError("Wrong arguments!");
            return;
        }

        toSnap.transform.position = parentConnector.position;
        toSnap.transform.rotation = parentConnector.rotation;
    }
}
