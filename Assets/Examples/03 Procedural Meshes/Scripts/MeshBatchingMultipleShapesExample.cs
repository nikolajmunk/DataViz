using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBatchingMultipleShapesExample : MonoBehaviour
{
    public Material material;
    public int ellipseCount = 16;
    public int ellipseResolution = 64; // Number of points to place on circle.

    Mesh _mesh;

    /// <summary>
    /// Creates a circle.
    /// </summary>
    ///
    void AddCircle(Vector2 position, float radius, Color color, List<Vector3> vertices, List<int> triangleIndices, List<Color> colors)
    {
        AddEllipse(position, radius, radius, color, vertices, triangleIndices, colors);
    }

    /// <summary>
    /// Creates an ellipse. Adds points to existing lists of vertices, indices, and colors.
    /// </summary>
    ///
    void AddEllipse(Vector2 position, float width, float height, Color color, List<Vector3> vertices, List<int> triangleIndices, List<Color> colors)
    {
        Vector2 prevPoint = position + new Vector2(width, 0); // Set first point of angle 0.
        for (int i = 1; i < ellipseResolution; i = i + 1)
        {
            float t = Mathf.InverseLerp(0, ellipseResolution - 1, i); // Normalized value of i.
            float angle = t * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * width;
            float y = Mathf.Sin(angle) * height;
            Vector2 point = position + new Vector2(x, y); // Offset x and y by the ellipse position.

            vertices.Add(position); // Add vertex for the ellipse's center point.
            colors.Add(Color.white);
            triangleIndices.Add(vertices.Count - 1); // Add this vertex's index for mesh.

            vertices.Add(point); // Add vertex for the next point on the ellipse.
            colors.Add(color);
            triangleIndices.Add(vertices.Count - 1); // Add this vertex's index for mesh.

            vertices.Add(prevPoint); // Add point for the previous point on the ellipse.
            colors.Add(color);
            triangleIndices.Add(vertices.Count - 1); // Add this vertex's index for mesh.

            //GL.Vertex3(0, 0, 0);
            //GL.Vertex3(x, y, 0);

            prevPoint = point; // Make sure the next loop accesses this loop's point on the ellipse.
        }
    }

    void Awake()
    {
        // Initialization.
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangleIndices = new List<int>();
        List<Color> colors = new List<Color>();

        // Create (circleCount) circles.
        for (int c = 0; c < ellipseCount; c++)
        {
            Vector2 position = Random.insideUnitCircle * 5;
            float radius = Random.Range(0.2f, 0.5f);
            Color color = Color.HSVToRGB(Random.value, 1, 1);

            //AddCircle(position, radius, color, vertices, triangleIndices, colors);
            AddEllipse(position, radius, Random.Range(0.2f, 0.5f), color, vertices, triangleIndices, colors);
        }

        _mesh = new Mesh();
        _mesh.SetVertices(vertices);
        _mesh.SetIndices(triangleIndices, MeshTopology.Triangles, 0);
        _mesh.SetColors(colors);
    }

    void Update()
    {
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }
}
