using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Interactor_Ray))]
[RequireComponent(typeof(LineRenderer))]
public class Interactor_Ray_visuals : MonoBehaviour
{
    Interactor_Ray m_Interactor_Ray;
    Interactor_Ray Interactor_Ray => m_Interactor_Ray ??= GetComponent<Interactor_Ray>();

    LineRenderer m_LineRenderer;
    LineRenderer LineRenderer => m_LineRenderer ??= GetComponent<LineRenderer>();

    Vector3[] points = new Vector3[2];

    private void OnEnable() => LineRenderer.enabled = true;
    private void OnDisable() => LineRenderer.enabled = false;

    private void Start() => LineRenderer.positionCount = points.Length;
    private void Update() => UpdateLineRenderer(Interactor_Ray, LineRenderer);
  
    void UpdateLineRenderer(Interactor_Ray interactor, LineRenderer lineRenderer)
    {
        points[0] = interactor.RaycastOrigin.position;
        points[1] = interactor.RaycastOrigin.position + Interactor_Ray.RaycastOrigin.TransformDirection(Interactor_Ray.RaycastDirection) * Interactor_Ray.RayDistance;
        lineRenderer.SetPositions(points);
    }

    [ContextMenu("Update Line renderer")]
    void Context_UpdateLineRenderer()
    {
        Interactor_Ray interactor_Ray = GetComponent<Interactor_Ray>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Length;
        UpdateLineRenderer(interactor_Ray, lineRenderer);
    }
}
