/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using UnityEngine;

public class GLTranslationRotationScaleExample : MonoBehaviour
{
	public Material material = null;
    public int rectCount = 16;
    public float rectAngle = -45;
    public float rectScale = 1;
    public float rectTranslateXFactor = 1;


	void OnRenderObject()
	{
		material.SetPass( 0 );

        for (int i = 0; i < rectCount; i++)
        {
            GL.PushMatrix(); // Remember the current transformation state.
            Matrix4x4 transformation = Matrix4x4.Translate(Vector3.right * i * rectTranslateXFactor); // Create transformation for next matrix.
            transformation *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, rectAngle));
            transformation *= Matrix4x4.Scale(Vector3.one * rectScale);
            transformation *= Matrix4x4.Translate(new Vector2(-.5f, -1));
            GL.MultMatrix(transformation); // Apply transformation.
            GLRect(.9f, 2); // Draw rect.
            GL.PopMatrix(); // Return to saved transformation state.
        }

    }

	void GLRect(float width, float height)
	{
		GL.Begin( GL.QUADS );
		GL.Vertex3( 0, 0, 0 );
		GL.Vertex3( 0, height, 0 );
		GL.Vertex3( width, height, 0 );
		GL.Vertex3( width, 0, 0 );
		GL.End();
	}
}