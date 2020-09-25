using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralPrism : MonoBehaviour
{
    public Material material;
    public int numberOfSegments = 3;
    //public float radius = 1;
    public float width = 1, depth = 1;
    public float height;

    [Header("Debug Settings")]
    [SerializeField]
    [Tooltip("Show vertices in gizmos mode.")]
    bool showVertices = true;
    [SerializeField]
    [Tooltip("Vertex point size.")]
    float vertexRadius = 0.03f;
    [SerializeField]
    [Tooltip("Show each vertex's index.")]
    bool showLabels = true;

    Mesh _mesh;
    Vector3[] vertices;
    int[] triangleIndices;


    void Awake()
    {
    }

    private void Generate()
    {
        vertices = new Vector3[(2 * numberOfSegments) + 2];
        triangleIndices = new int[2 * numberOfSegments * 3 + 6 * numberOfSegments];

        // Draw endcaps
        AddEllipse(new Vector3(0, -height / 2, 0), numberOfSegments, width, depth, 0, vertices, triangleIndices, true);
        AddEllipse(new Vector3(0, height / 2, 0), numberOfSegments, width, depth, numberOfSegments + 1, vertices, triangleIndices);

        // Draw side faces
        for (int ti = 6 * numberOfSegments, vi = 1; vi <= numberOfSegments; ti += 6, vi++)
        {
            int neighboringVertex;
            if (vi % numberOfSegments == 0) { neighboringVertex = 1; } else { neighboringVertex = vi + 1; }

            triangleIndices[ti] = vi;
            triangleIndices[ti + 1] = vi + numberOfSegments + 1;
            triangleIndices[ti + 2] = neighboringVertex;
            triangleIndices[ti + 3] = neighboringVertex;
            triangleIndices[ti + 4] = vi + numberOfSegments + 1;
            triangleIndices[ti + 5] = neighboringVertex + numberOfSegments + 1;

        }

        _mesh = new Mesh();
        _mesh.SetVertices(vertices);
        _mesh.SetIndices(triangleIndices, MeshTopology.Triangles, 0);
        _mesh.RecalculateNormals();
    }

    void Update()
    {
        Generate();
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }

    void AddCircle(Vector3 position, int resolution, float radius, int vertexIndex, Vector3[] vertices, int[] triangleIndices, bool drawCounterClockwise=false)
    {
        AddEllipse(position, resolution, radius, radius, vertexIndex, vertices, triangleIndices, drawCounterClockwise);
    }

    void AddEllipse(Vector3 position, int resolution, float width, float depth, int vertexIndex, Vector3[] vertices, int[] triangleIndices, bool drawCounterClockwise=false)
    {
        // Place center vertex.
        vertices[vertexIndex] = position;

        // Calculate starting triangle index.
        float tri = vertexIndex * 3 * resolution / (resolution + 1);
        int triangleIndex = (int)tri;

        // Place vertices around ellipse.
        for (int i = 1; i <= resolution; i = i + 1)
        {
            float t = Mathf.InverseLerp(0, resolution, i-1); // Normalized value of i.
            float angle = t * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * width;
            float z = Mathf.Sin(angle) * depth;
            Vector3 point = position + new Vector3(x, 0, z); // Offset x and z by the ellipse position.
            vertices[vertexIndex + i] = point;

        }

        // Set triangle indices.
        for (int ti = triangleIndex, vi = vertexIndex; ti < triangleIndex+resolution*3; ti = ti + 3, vi++)
        {

            int n = 0;
            triangleIndices[ti] = vertexIndex;
            if (drawCounterClockwise) { triangleIndices[ti + 1 + n] = vi + 1; n++; } // Set this vertex first to draw counterclockwise, e.g. on the bottom.
            if ((vi - vertexIndex + 1) % resolution == 0) { int v = vertexIndex + 1;  triangleIndices[ti + 1 + n] = v; } else { int v = vi + 2;  triangleIndices[ti + 1 + n] = v;}
            if (!drawCounterClockwise) { triangleIndices[ti + 2] = vi + 1; } // If drawing counterclockwise, we already set this vertex and won't set it again.

        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (showVertices) Gizmos.DrawSphere(vertices[i], vertexRadius);
            #if UNITY_EDITOR // Only include this code in the editor. UnityEditor class does not exist in builds.
            if (showLabels) UnityEditor.Handles.Label(vertices[i], i.ToString());
            #endif
        }


    }
}
