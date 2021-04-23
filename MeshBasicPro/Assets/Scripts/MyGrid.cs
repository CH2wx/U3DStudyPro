using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MyGrid : MonoBehaviour
{
    public int xSize, ySize;
    public Vector3[] vertices;
    public Material[] materials;
    private Mesh _mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate ()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

        _mesh = new Mesh();
        _mesh.name = "Procedural Mesh";
        GetComponent<MeshFilter>().mesh = _mesh;

        GetComponent<MeshRenderer>().materials = materials;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // 如果不提供UV坐标，那么它们都是默认的零。那么即使使用了albedo纹理，依然只存在一个颜色
        Vector2[] uvList = new Vector2[vertices.Length];
        // 切线是一个三维向量，但是Unity实际上使用了一个4D向量。它的第四个分量总是−1或1，用于控制第三切线空间维的方向--前向或后向。
        // 这方便对法线映射进行镜像，这种映射经常用于像人这样具有双边对称性三维模型中。
        // Unity的着色器执行此计算的方式要求我们使用−1。
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1, 0, 0, -1);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i ++)
            {
                vertices[i] = new Vector3(x, y, 0);
                // mesh的确切外观取决于纹理的包装模式是设置为clamp 还是repeat。
                // 因为目前正在用整数除以整数，这会产生另一个整数。为了在整个网格中获得零到一之间的正确坐标，必须确保使用的是浮点数。
                uvList[i] = new Vector2((float)x / xSize, (float)y / ySize);
                // 一个平面的切线都指向相同的方向
                tangents[i] = tangent;
            }
        }
        _mesh.vertices = vertices;
        _mesh.uv = uvList;
        _mesh.tangents = tangents;
        
        // 默认情况下，如果它们按顺时针方向排列，则三角形被认为是前向的和可见的
        int[] triangles = new int[xSize * ySize * 6];
   
        // 计算每行的三角形顶点顺序
        for (int y = 0, ti = 0, vi = 0; y < ySize; y++, vi++)
        {
            // 计算1行的三角形顶点顺序
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 5] = vi + xSize + 2;

                // 每次计算完三角形都刷新一次
                _mesh.triangles = triangles;
                yield return waitForSeconds;
            }
        }

        // 计算每个顶点的法线
        _mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null || vertices.Length <= 0)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i] != null)
            {
                Gizmos.color = Color.blue;//为随后绘制的gizmos设置颜色。
                Gizmos.DrawSphere(vertices[i], 0.1f);//使用center和radius参数，绘制一个线框球体。
            }
        }
    }
}
