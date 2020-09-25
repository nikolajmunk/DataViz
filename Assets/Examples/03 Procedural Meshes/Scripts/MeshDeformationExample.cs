using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformationExample : MonoBehaviour
{
    public Material material;
    public Mesh originalMesh;
    public float waveCount = 2;
    public float waveAmplitude = .1f;
    public float waveSpeed = 1;
    public float twistAmount = 0;

    Mesh _mesh;
    Vector3[] _originalVertices;
    Vector3[] _deformedVertices;

    float waveAngleOffset;

    void Awake()
    {
        // Save data of original mesh.
        _originalVertices = originalMesh.vertices;
        int[] triangleIndices = originalMesh.triangles;

        _mesh = new Mesh();
        _mesh.vertices = _originalVertices;
        _mesh.triangles = triangleIndices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _deformedVertices = new Vector3[_originalVertices.Length];
    }

    void Update()
    {
        waveAngleOffset += Time.deltaTime * waveSpeed;

        for (int v = 0; v < _originalVertices.Length; v++)
        {
            Vector3 vertexPosition = _originalVertices[v];

            float angle = vertexPosition.x * Mathf.PI * 2 * waveCount + waveAngleOffset;
            vertexPosition.y += Mathf.Sin(angle) * waveAmplitude;

            //Twist
            vertexPosition = Quaternion.Euler(0, vertexPosition.y * twistAmount, 0) * vertexPosition;

            _deformedVertices[v] = vertexPosition;
        }

        _mesh.vertices = _deformedVertices;
        _mesh.RecalculateNormals();

        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, material, gameObject.layer);
    }
}
