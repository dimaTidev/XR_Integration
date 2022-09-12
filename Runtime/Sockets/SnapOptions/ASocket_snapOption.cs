using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ASocket_snapOption : ScriptableObject
{
    public void Snap(GameObject toSnap, GameObject endParent) => Snap(toSnap, toSnap.transform, endParent, endParent.transform);
    public abstract void Snap(GameObject toSnap, Transform connector, GameObject endParent, Transform endConnector);
}
