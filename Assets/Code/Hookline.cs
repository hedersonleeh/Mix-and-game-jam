using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Hookline : MonoBehaviour
{
    [SerializeField] private Transform endPos;
    private LineRenderer lineRenderer;
    public Transform StartPos { get; set; }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (lineRenderer.GetPosition(0) != null)
        {
            lineRenderer.SetPosition(0, StartPos.position);
            lineRenderer.SetPosition(1, endPos.position);
        }
    }
}