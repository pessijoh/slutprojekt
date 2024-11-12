using UnityEngine;
using System.Collections.Generic;

public class fjaderkraft : MonoBehaviour
{
    public float Mass = 1.0f;
    public float SpringConstant = 20.0f;
    public float DampingFactor = 0.9f;
    public float RestLength = 0.1f;

    private float gravity = -9.82f;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] originalVertices;
    private Vector3[] velocities;
    private int gridWidth = 10;
    private float maxMagnitude = 1.5f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        originalVertices = mesh.vertices;
        velocities = new Vector3[vertices.Length];
    }

    void FixedUpdate()
    {
        float Fg = Mass * gravity;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (IsEdgeVertex(i)) continue;

            // Get neighbor indices
            int[] neighbors = GetNeighborIndices(i);
            Vector3 totalSpringForce = Vector3.zero;

            foreach (int neighborIndex in neighbors)
            {
               
                Vector3 direction = vertices[i] - vertices[neighborIndex];
                float distance = direction.magnitude;
                direction.Normalize();
                float springForceMagnitude = -SpringConstant * (distance - RestLength);
                
                
                totalSpringForce += springForceMagnitude * direction;
            }

            Vector3 totalForce = new Vector3(0, Fg, 0) + totalSpringForce;

            Vector3 acceleration = totalForce / Mass;

            velocities[i] += acceleration * Time.deltaTime;
            velocities[i] *= DampingFactor;

            vertices[i] += velocities[i] * Time.deltaTime;

            vertices[i] = Vector3.ClampMagnitude(vertices[i] - originalVertices[i], maxMagnitude) + originalVertices[i];
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }   

    bool IsEdgeVertex(int index)
    {
        Vector3 localPosition = originalVertices[index];
        return Mathf.Abs(localPosition.x) >= 4.5f || Mathf.Abs(localPosition.z) >= 4.5f;
    }

    int[] GetNeighborIndices(int index)
    {
        var neighbors = new List<int>();

        int row = index / gridWidth;
        int col = index % gridWidth;

        if (row > 0) neighbors.Add(index - gridWidth); // Above

        if (row < gridWidth - 1) neighbors.Add(index + gridWidth); // Below

        if (col > 0) neighbors.Add(index - 1); // Left

        if (col < gridWidth - 1) neighbors.Add(index + 1); // Right

        if (row > 0 && col > 0) neighbors.Add(index - gridWidth - 1); // Top-left
        if (row > 0 && col < gridWidth - 1) neighbors.Add(index - gridWidth + 1); // Top-right
        if (row < gridWidth - 1 && col > 0) neighbors.Add(index + gridWidth - 1); // Bottom-left
        if (row < gridWidth - 1 && col < gridWidth - 1) neighbors.Add(index + gridWidth + 1); // Bottom-right

        return neighbors.ToArray();
    }
}
