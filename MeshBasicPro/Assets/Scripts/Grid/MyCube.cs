using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义方体网格
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MyCube : MonoBehaviour
{
    public int xSize, ySize, zSize;
    /// <summary>
    /// 圆角的程度
    /// </summary>
    public int roundness;

    public bool isKinematic = true;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private Vector3[] _normals;
    private Color32[] _cubeUV;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        _mesh = new Mesh();
        _mesh.name = "Procedural Cube";
        GetComponent<MeshFilter>().mesh = _mesh;

        CreateVertices();
        CreateTriangles();
        CreateColliders();
        Rigidbody r = gameObject.AddComponent<Rigidbody>();
        r.isKinematic = isKinematic;
    }

    /// <summary>
    /// 创建Mesh的顶点
    /// </summary>
    private void CreateVertices()
    {
        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = (
                (xSize - 1) * (ySize - 1) +
                (xSize - 1) * (zSize - 1) +
                (ySize - 1) * (zSize - 1)
            ) * 2;
        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        _normals = new Vector3[_vertices.Length];
        _cubeUV = new Color32[_vertices.Length];

        int v = 0;
        // 盖四个面
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                SetVertex(v++, x, y, 0);
            }
            for (int z = 1; z <= zSize; z++)
            {
                SetVertex(v++, xSize, y, z);
            }
            for (int x = xSize - 1; x >= 0; x--)
            {
                SetVertex(v++, x, y, zSize);
            }
            for (int z = zSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }

        // 盖上下两面
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(v++, x, ySize, z);
            }
        }
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(v++, x, 0, z);
            }
        }

        _mesh.vertices = _vertices;
        _mesh.normals = _normals;
        _mesh.colors32 = _cubeUV;
    }

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 inner = new Vector3(x, y, z);
        _vertices[i] = inner;

        if (inner.x < roundness)
        {
            inner.x = roundness;
        }
        else if (inner.x > xSize - roundness)
        {
            inner.x = xSize - roundness;
        }

        if (inner.y < roundness)
        {
            inner.y = roundness;
        }
        else if (inner.y > ySize - roundness)
        {
            inner.y = ySize - roundness;
        }

        if (inner.z < roundness)
        {
            inner.z = roundness;
        }
        else if (inner.z > zSize - roundness)
        {
            inner.z = zSize - roundness;
        }

        _normals[i] = (_vertices[i] - inner).normalized;
        _vertices[i] = inner + _normals[i] * roundness;
        _cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
    }

    /// <summary>
    /// 创建Mesh的三角片
    /// </summary>
    private void CreateTriangles()
    {
        //int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        //int[] triangles = new int[quads * 6];
        int[] trianglesX = new int[ySize * zSize * 12];
        int[] trianglesY = new int[xSize * zSize * 12];
        int[] trianglesZ = new int[xSize * ySize * 12];

        // 四个面
        int ring = (xSize + zSize) * 2;
        int v = 0, tX = 0, tY = 0, tZ = 0;
        for (int y = 0; y < ySize; y++, v++)
        {
            for (int q = 0; q < xSize; q++, v++)
            {
                tZ = SetQuad(trianglesZ, tZ, v, v + ring, v + 1, v + ring + 1);
            }
            for (int q = 0; q < zSize; q++, v++)
            {
                tX = SetQuad(trianglesX, tX, v, v + ring, v + 1, v + ring + 1);
            }
            for (int q = 0; q < xSize; q++, v++)
            {
                tZ = SetQuad(trianglesZ, tZ, v, v + ring, v + 1, v + ring + 1);
            }
            for (int q = 0; q < zSize - 1; q++, v++)
            {
                tX = SetQuad(trianglesX, tX, v, v + ring, v + 1, v + ring + 1);
            }
            tX = SetQuad(trianglesX, tX, v, v + ring, v - ring + 1, v + 1);
        }
        tY = CreateTopTriangle(trianglesY, tY, ring);
        tY = CreateBottomTriangle(trianglesY, tY, ring);

        _mesh.subMeshCount = 3;
        _mesh.SetTriangles(trianglesZ, 0);
        _mesh.SetTriangles(trianglesX, 1);
        _mesh.SetTriangles(trianglesY, 2);
    }

    /// <summary>
    /// 创建顶部的三角片
    /// </summary>
    private int CreateTopTriangle(int[] triangles, int t, int ring)
    {
        int v = ring * ySize;
        // 顶面的第一排
        for (int x = 0; x < xSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + ring - 1, v + 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + ring - 1, v + 1, v + 2);

        // 中间的网格
        int vMin = ring * (ySize + 1) - 1;  // 左边的顶点
        int vMid = vMin + 1;                // 中间的顶点
        int vMax = v + 2;                   // 右边的顶点
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMin - 1, vMid, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + xSize - 1, vMid + 1, vMid + xSize);
            }
            t = SetQuad(triangles, t, vMid, vMid + xSize - 1, vMax, vMax + 1);
        }

        // 最后一排
        int vTop = vMin - 2;                // 最后一条边的顶点
        t = SetQuad(triangles, t, vMin, vTop + 1, vMid, vTop);
        for (int x = 1; x < xSize - 1; x++, vMid++, vTop--)
        {
            t = SetQuad(triangles, t, vMid, vTop, vMid + 1, vTop - 1);
        }
        t = SetQuad(triangles, t, vMid, vTop, vTop - 2, vTop - 1);

        return t;
    }

    /// <summary>
    ///  创建底部的三角片
    /// </summary>
    private int CreateBottomTriangle(int[] triangles, int t, int ring)
    {
        int v = 1;
        // 下面两行都是用来计算底部中间起始顶点的位置索引的
        int vMid = _vertices.Length - (xSize - 1) * (zSize - 1);
        //int vMid = ring * (ySize + 1) + (xSize - 1) * (zSize - 1);

        // 底面的第一排
        t = SetQuad(triangles, t, ring - 1, 0, vMid, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, v, vMid + 1, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v, v + 2, v + 1);

        // 中间的网格
        int vMin = ring - 1;                // 左边的顶点
        int vMax = v + 2;                   // 右边的顶点
        vMid -= (xSize - 2);
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin - 1, vMin, vMid + xSize - 1, vMid);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid + xSize - 1, vMid, vMid + xSize, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + xSize - 1, vMid, vMax + 1, vMax);
        }

        // 最后一排
        int vTop = vMin - 2;                // 最后一条边的顶点
        t = SetQuad(triangles, t, vTop + 1, vMin, vTop, vMid);
        for (int x = 1; x < xSize - 1; x++, vMid++, vTop--)
        {
            t = SetQuad(triangles, t, vTop, vMid, vTop - 1, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vMid, vTop - 1, vTop - 2);

        return t;
    }

    /// <summary>
    /// 设置三角片的顶点索引
    /// </summary>
    /// <returns>返回下一个三角片的起始索引</returns>
    private int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v10;
        triangles[i + 2] = triangles[i + 3] = v01;
        triangles[i + 5] = v11;
        return i + 6;
    }

    /// <summary>
    /// 生成当前Mesh的触发器
    /// </summary>
    private void CreateColliders()
    {
        // 设置三对表面的Collider
        AddBoxCollider(xSize, ySize - roundness * 2, zSize - roundness * 2);
        AddBoxCollider(xSize - roundness * 2, ySize, zSize - roundness * 2);
        AddBoxCollider(xSize - roundness * 2, ySize - roundness * 2, zSize);

        // 设置边缘、角落的Collider（即每条边一个Collider）
        Vector3 min = Vector3.one * roundness;
        Vector3 half = new Vector3(xSize, ySize, zSize) * 0.5f;
        Vector3 max = new Vector3(xSize, ySize, zSize) - min;

        // 平行X轴的四条边
        AddCapsuleCollider(0, half.x, min.y, min.z);
        AddCapsuleCollider(0, half.x, max.y, min.z);
        AddCapsuleCollider(0, half.x, min.y, max.z);
        AddCapsuleCollider(0, half.x, max.y, max.z);

        // 平行Y轴的四条边
        AddCapsuleCollider(1, min.x, half.y, min.z);
        AddCapsuleCollider(1, max.x, half.y, min.z);
        AddCapsuleCollider(1, min.x, half.y, max.z);
        AddCapsuleCollider(1, max.x, half.y, max.z);

        // 平行Z轴的四条边
        AddCapsuleCollider(2, min.x, min.y, half.z);
        AddCapsuleCollider(2, max.x, min.y, half.z);
        AddCapsuleCollider(2, min.x, max.y, half.z);
        AddCapsuleCollider(2, max.x, max.y, half.z);
    }

    private void AddBoxCollider(float x, float y, float z)
    {
        BoxCollider c = gameObject.AddComponent<BoxCollider>();
        c.size = new Vector3(x, y, z);
    }

    private void AddCapsuleCollider(int direction, float x, float y, float z)
    {
        CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();
        c.center = new Vector3(x, y, z);
        c.direction = direction;                // 朝向（0、1、2分别对应X、Y、Z轴）
        c.radius = roundness;                   // 弧度
        c.height = c.center[direction] * 2;     // 高度
    }

    //private void OnDrawGizmos()
    //{
    //    if (_vertices == null || _vertices.Length <= 0)
    //    {
    //        return;
    //    }
    //    for (int i = 0; i < _vertices.Length; i++)
    //    {
    //        // 画顶点
    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawSphere(_vertices[i], 0.1f);
    //        // 画法线
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawRay(_vertices[i], _normals[i]);
    //    }
    //}
}
