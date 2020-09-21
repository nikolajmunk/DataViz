using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircleOnTransform : MonoBehaviour
{
    public int vertexCount = 100;
    public float radius = 3.0f;

    public Material circleMaterial;

    public void OnRenderObject()
    {
        // Apply the line material
        circleMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
        for (int i = 0; i < vertexCount; ++i)
        {
            float a = i / (float)vertexCount;
            float angle = a * Mathf.PI * 2;
            // Vertex colors change from red to green
            GL.Color(new Color(a, 1 - a, 0, 0.8F));
            // One vertex at transform position
            GL.Vertex3(0, 0, 0);
            // Another vertex at edge of circle
            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            a = i + 1 / (float)vertexCount;
            angle = a * Mathf.PI * 2;
            // Another vertex at edge of circle
            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        GL.End();
        GL.PopMatrix();
    }
}
