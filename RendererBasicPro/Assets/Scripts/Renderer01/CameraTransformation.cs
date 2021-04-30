using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformation : Transformation
{
    /// <summary>
    /// 焦距
    /// </summary>
    public float focalLength = 2f;

    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(focalLength, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, focalLength, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 1f, 0f));
            return matrix;
        }
    }

    public override Vector3 Apply(Vector3 point)
    {
        return Matrix.MultiplyPoint(point);
    }
}
