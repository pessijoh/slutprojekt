using UnityEngine;
using System.Collections.Generic;

public class fjaderkraft : MonoBehaviour
{
    public float Mass = 1.0f;
    public float SpringConstant = 50.0f;
    public float DampingFactor = 0.95f;
    public float RestLength = 0.1f;
    public float MaxDisplacement = 1.0f;

    private float gravity = -9.82f;
    private Mesh mesh;
    private Vertex[] verticesData;
    private Vector3[] originalVertices;
    private HashSet<int> fixedIndices = new HashSet<int>();
    private int gridWidth = 10;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        verticesData = new Vertex[originalVertices.Length];

        for (int i = 0; i < originalVertices.Length; i++)
        {
            verticesData[i] = new Vertex(originalVertices[i]);

            Vector3 pos = originalVertices[i];
            if (Mathf.Abs(pos.x) >= 4.5f || Mathf.Abs(pos.z) >= 4.5f)
            {
                fixedIndices.Add(i);
            }
        }

        for (int i = 0; i < verticesData.Length; i++)
        {
            if (!fixedIndices.Contains(i))
            {
                int[] neighbors = GetNeighborIndices(i);
                verticesData[i].Neighbors.AddRange(neighbors);
            }
        }
    }

    void FixedUpdate()
    {
        foreach (var vertex in verticesData)
        {
            if (vertex.Neighbors.Count == 0) continue;

            Vector3 totalSpringForce = Vector3.zero;

            foreach (int neighborIndex in vertex.Neighbors)
            {
                Vertex neighbor = verticesData[neighborIndex];
                Vector3 direction = vertex.Position - neighbor.Position;
                float distance = direction.magnitude;
                direction.Normalize();

                float springForceMagnitude = -SpringConstant * (distance - RestLength);
                totalSpringForce += springForceMagnitude * direction;
            }

            Vector3 totalForce = totalSpringForce + new Vector3(0, gravity * Mass, 0);
            Vector3 acceleration = totalForce / Mass;

            vertex.Velocity += acceleration * Time.deltaTime;
            vertex.Velocity *= DampingFactor;
            vertex.Position += vertex.Velocity * Time.deltaTime;

            vertex.Position = ClampVertexMovement(vertex.Position, vertex.OriginalPosition, MaxDisplacement);
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        Vector3[] updatedVertices = new Vector3[verticesData.Length];
        for (int i = 0; i < verticesData.Length; i++)
        {
            updatedVertices[i] = verticesData[i].Position;
        }
        mesh.vertices = updatedVertices;
        mesh.RecalculateBounds();
    }

    bool IsEdgeVertex(int index)
    {
        return fixedIndices.Contains(index);
    }

    int[] GetNeighborIndices(int index)
    {
        List<int> neighbors = new List<int>();

        int row = index / gridWidth;
        int col = index % gridWidth;

        if (row > 0) neighbors.Add(index - gridWidth);
        if (row < gridWidth - 1) neighbors.Add(index + gridWidth);
        if (col > 0) neighbors.Add(index - 1);
        if (col < gridWidth - 1) neighbors.Add(index + 1);

        return neighbors.ToArray();
    }

    Vector3 ClampVertexMovement(Vector3 currentPosition, Vector3 originalPosition, float maxDistance)
    {
        Vector3 delta = currentPosition - originalPosition;
        if (delta.magnitude > maxDistance)
        {
            delta = delta.normalized * maxDistance;
        }
        return originalPosition + delta;
    }
}

public class Vertex
{
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 OriginalPosition;
    public List<int> Neighbors;

    public Vertex(Vector3 position)
    {
        Position = position;
        Velocity = Vector3.zero;
        OriginalPosition = position;
        Neighbors = new List<int>();
    }
}
