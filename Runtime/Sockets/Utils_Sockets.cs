using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils_Sockets
{
    public static GameObject Clone(GameObject reference)
    {
        GameObject go = MonoBehaviour.Instantiate(reference);

        Hide_Colliders(go);
        Delete_Joints(go);
         // MonoBehaviour.Destroy(col);

         Material mat = Material();

        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        foreach (var rend in renderers)
        {
            if (rend.sharedMaterials.Length == 1)
                rend.sharedMaterial = mat;
            else if (rend.sharedMaterials.Length > 1)
            {
                Material[] materials = new Material[rend.sharedMaterials.Length];
                for (int i = 0; i < materials.Length; i++)
                    materials[i] = mat;
                rend.sharedMaterials = materials;
            }
        }

        return go;
    }

    static void Hide_Colliders(GameObject go)
    {
        Collider[] cols = go.GetComponentsInChildren<Collider>();
        foreach (var col in cols)
            col.enabled = false;
    }

    static void Delete_Joints(GameObject go)
    {
        Joint[] joints = go.GetComponentsInChildren<Joint>();
        foreach (var joint in joints)
            MonoBehaviour.Destroy(joint);
    }

    static Material Material() => new Material(Shader.Find("Unlit/Color"));
}
