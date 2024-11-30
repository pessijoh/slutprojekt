using System;
using UnityEngine;

public class svagningsrorelse : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    
    Vector3 initialPosition;
    float swayAmount = 0.00015f; 

    void Start()
    {
        initialPosition = transform.position;
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void Update()
    {
        
        for (var i = 0; i < vertices.Length; i++)
        {
            
            float sway = Mathf.Sin(Time.time + vertices[i].x + vertices[i].z) * swayAmount;
            
            
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + sway, vertices[i].z);
        }

      
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        
        transform.position = initialPosition;
    }
}
