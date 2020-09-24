using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPolygonTriangulationExample : MonoBehaviour
{
    public Material material;
    public Transform[] outlinePointTransforms = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (outlinePointTransforms == null) return;

        for (int i = 0; i < outlinePointTransforms.Length; i++)
        {
            Vector3 position = outlinePointTransforms[i].position;
            Gizmos.DrawSphere(position, .03f);
            #if UNITY_EDITOR //Only include this code in the editor.
            UnityEditor.Handles.Label(position, i.ToString());
            #endif
        }
    }
}
