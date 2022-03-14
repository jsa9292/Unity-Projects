using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSphere : MonoBehaviour
{
    public float radius;
    public int v_count;
    public Vector3 offset;
    private List<Vector3> vertices;
    private SplineMesh.Spline spline { get => GetComponent<SplineMesh.Spline>(); }
    // Start is called before the first frame update
    private void OnValidate()
    {
        Vector3 center = Vector3.zero + Vector3.up * radius;
        float deltaAngle = Mathf.PI*2/ v_count;
        vertices = new List<Vector3>(new Vector3[v_count]);
        for (int i = 0; i < vertices.Count; i++)
        {
            float vertex_x = offset.x + transform.position.x + Mathf.Cos(deltaAngle * i) * radius;
            float vertex_y = offset.y + transform.position.y + Mathf.Sin(deltaAngle * i) * radius;
            vertices[i] = new Vector3(vertex_x, vertex_y, 0);
        }
        if (vertices.Count == spline.nodes.Count-1)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                int v_start = i;
                int v_end = (i + 1) % vertices.Count;
                spline.nodes[i].Position = vertices[v_start];
                spline.nodes[i].Direction = vertices[v_end];
                spline.nodes[i].Up = offset;
            }
            for (int i = 0; i < 1; i++)
            {
                int v_start = i;
                int v_end = (i + 1) % vertices.Count;
                spline.nodes[i].Position = vertices[v_start];
                spline.nodes[i].Direction = vertices[v_end];
                spline.nodes[i].Up = -Vector3.up;
            }

        }
    }
    
}
