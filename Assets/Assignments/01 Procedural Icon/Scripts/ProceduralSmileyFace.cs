using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSmileyFace : MonoBehaviour
{
    public Material material = null;
    //public int randomSeed = 0;
    [Range(-1, 1)]
    public float valence, activation;
    public float mouthWidth = 4;
    public float mouthHeightFactor = 1.6f;
    public float eyeDistance = 2f;
    public float eyeHeight = 2.5f;
    public Vector2 minEyeSize = new Vector2(.6f, .2f);
    public Vector2 maxEyeSize = new Vector2(2, 2);
    public Color backgroundColor = Color.white;
    public Color foregroundColor = Color.black;


    const int circleResolution = 64;
    const int mouthResolution = 32;


    void OnRenderObject()
    {
        material.SetPass(0);

        //Draw background circle
        GL.PushMatrix();
        Matrix4x4 circleTransformation = Matrix4x4.Translate(new Vector3(0, 1.5f, 0));
        GL.MultMatrix(circleTransformation);
        GLCircle(3.2f, backgroundColor);
        GL.PopMatrix();

        //Draw eyes
        float _width = Mathf.Lerp(minEyeSize.x, maxEyeSize.x, Mathf.InverseLerp(-1, 1, activation));
        float _height = Mathf.Lerp(minEyeSize.y, maxEyeSize.y, Mathf.InverseLerp(-1, 1, activation));
        float _eyesXPos = Mathf.Abs(eyeDistance / 2);

        GL.PushMatrix();
        Matrix4x4 transformation = Matrix4x4.Translate(new Vector3(-_eyesXPos, eyeHeight, 0));
        GL.MultMatrix(transformation);
        GLEllipse(_width, _height, foregroundColor);
        GL.PopMatrix();

        GL.PushMatrix();
        transformation = Matrix4x4.Translate(new Vector3(_eyesXPos, eyeHeight, 0));
        GL.MultMatrix(transformation);
        GLEllipse(_width, _height, foregroundColor);
        GL.PopMatrix();

        GL.PushMatrix();
        transformation = Matrix4x4.Translate(new Vector3(0, valence * 0.8f, 0));
        GL.MultMatrix(transformation);
        GLMouth(mouthWidth, mouthHeightFactor);
        GL.PopMatrix();

    }

    void GLEllipse(float width, float height, Color color)
    {
        GL.Begin(GL.LINE_STRIP);
        GL.Color(color);
        for (int i = 0; i < circleResolution; i = i + 1)
        {
            float t = Mathf.InverseLerp(0, circleResolution - 1, i); // Normalized value of i.
            float angle = t * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * width;
            float y = Mathf.Sin(angle) * height;
            GL.Vertex3(x, y, 0);
        }
        GL.End();
    }
    void GLCircle(float radius, Color color)
    {
        GL.Begin(GL.TRIANGLE_STRIP);
        GL.Color(color);
        for (int i = 0; i < circleResolution; i = i + 1)
        {
            float t = Mathf.InverseLerp(0, circleResolution - 1, i); // Normalized value of i.
            float angle = t * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            GL.Vertex3(x, y, 0);
            GL.Vertex3(0, 0, 0);
        }
        GL.End();
    }

    void GLMouth(float width, float heightFactor)
    {
        GL.Begin(GL.LINE_STRIP);
        GL.Color(foregroundColor);
        float _xPos = Mathf.Abs(width / 2);
        Vector3 mouthStartPoint = new Vector3(-_xPos, 0, 0);
        Vector3 controlPointA = new Vector3(-_xPos, -valence * heightFactor, 0);
        Vector3 controlPointB = new Vector3(_xPos, -valence * heightFactor, 0);
        Vector3 mouthEndPoint = new Vector3(_xPos, 0, 0);
        GL.Vertex3(mouthStartPoint.x, mouthStartPoint.y, mouthStartPoint.z);
        for (int i = 0; i<mouthResolution; i++)
        {
            float t = Mathf.InverseLerp(0, mouthResolution - 1, i);
            float _x = CubicInterpolation(mouthStartPoint.x, controlPointA.x, controlPointB.x, mouthEndPoint.x, t);
            float _y = CubicInterpolation(mouthStartPoint.y, controlPointA.y, controlPointB.y, mouthEndPoint.y, t);
            GL.Vertex3(_x, _y, 0);
        }
        GL.Vertex3(mouthEndPoint.x, mouthEndPoint.y, mouthEndPoint.z);
        GL.End();
    }

    /// <summary>
    /// Evalutes quadratic bezier at point t for points a, b, c, d.
    ///	t varies between 0 and 1, and a and d are the curve points,
    ///	b and c are the control points. this can be done once with the
    ///	x coordinates and a second time with the y coordinates to get
    ///	the location of a bezier curve at t.
    /// </summary>
    static float CubicInterpolation(float a, float b, float c, float d, float t)
    {
        float t1 = 1f - t;
        return a * t1 * t1 * t1 + 3 * b * t * t1 * t1 + 3 * c * t * t * t1 + d * t * t * t;
    }

    float QuadraticInterpolation(float start, float control, float end, float t)
    {
        return (((1 - t) * (1 - t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
    }
}
