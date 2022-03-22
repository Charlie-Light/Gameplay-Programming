using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineScript : MonoBehaviour
{
    public List<Vector3> spline_points;
    public bool draw_spline = true;
    public Camera spline_camera;
    public GameObject spline_object;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if(draw_spline)
        {
            lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = spline_points.Count;
            lineRenderer.useWorldSpace = true;
        }
        lineRenderer.transform.parent = spline_object.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(draw_spline)
        {
            DrawSpline();
        }
    }

    private void DrawSpline()
    {

        for(int x = 0; x < spline_points.Count; x++)
        {
            var point = spline_object.transform.TransformPoint(spline_points[x]);
            lineRenderer.SetPosition(x, point);
        }
    }
}
