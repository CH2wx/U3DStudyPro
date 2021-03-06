﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义球体网格
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MySphere : MonoBehaviour
{
    /// <summary>
    /// 网格大小，用于顶点的数量，网格越大，顶点越多，弧度越柔和。与球的大小无关
    /// </summary>
    public int gridSize;
    /// <summary>
    /// 球的半径，控制球的大小
    /// </summary>
    public float radius;

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
        int edgeVertices = (gridSize + gridSize + gridSize - 3) * 4;
        int faceVertices = (
                (gridSize - 1) * (gridSize - 1) +
                (gridSize - 1) * (gridSize - 1) +
                (gridSize - 1) * (gridSize - 1)
            ) * 2;
        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        _normals = new Vector3[_vertices.Length];
        _cubeUV = new Color32[_vertices.Length];

        int v = 0;
        // 盖四个面
        for (int y = 0; y <= gridSize; y++)
        {
            for (int x = 0; x <= gridSize; x++)
            {
                SetVertex(v++, x, y, 0);
            }
            for (int z = 1; z <= gridSize; z++)
            {
                SetVertex(v++, gridSize, y, z);
            }
            for (int x = gridSize - 1; x >= 0; x--)
            {
                SetVertex(v++, x, y, gridSize);
            }
            for (int z = gridSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }

        // 盖上下两面
        for (int z = 1; z < gridSize; z++)
        {
            for (int x = 1; x < gridSize; x++)
            {
                SetVertex(v++, x, gridSize, z);
            }
        }
        for (int z = 1; z < gridSize; z++)
        {
            for (int x = 1; x < gridSize; x++)
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
        Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;     // 另球的半径保持在[-1, 1]
        // 对v的正方体上的顶点进行映射，映射到球的表面上
        float x2 = v.x * v.x;
        float y2 = v.y * v.y;
        float z2 = v.z * v.z;
        Vector3 sphereNormal;
        sphereNormal.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
        sphereNormal.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        sphereNormal.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

        _normals[i] = sphereNormal;
        _vertices[i] = _normals[i] * radius;
        _cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
    }

    /// <summary>
    /// 创建Mesh的三角片
    /// </summary>
    private void CreateTriangles()
    {
        //int quads = (gridSize * gridSize + gridSize * gridSize + gridSize * gridSize) * 2;
        //int[] triangles = new int[quads * 6];
        int[] trianglesX = new int[gridSize * gridSize * 12];
        int[] trianglesY = new int[gridSize * gridSize * 12];
        int[] trianglesZ = new int[gridSize * gridSize * 12];

        // 四个面
        int ring = (gridSize + gridSize) * 2;
        int v = 0, tX = 0, tY = 0, tZ = 0;
        for (int y = 0; y < gridSize; y++, v++)
        {
            for (int q = 0; q < gridSize; q++, v++)
            {
                tZ = SetQuad(trianglesZ, tZ, v, v + ring, v + 1, v + ring + 1);
            }
            for (int q = 0; q < gridSize; q++, v++)
            {
                tX = SetQuad(trianglesX, tX, v, v + ring, v + 1, v + ring + 1);
            }
            for (int q = 0; q < gridSize; q++, v++)
            {
                tZ = SetQuad(trianglesZ, tZ, v, v + ring, v + 1, v + ring + 1);
            }
            for (int q = 0; q < gridSize - 1; q++, v++)
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
        int v = ring * gridSize;
        // 顶面的第一排
        for (int x = 0; x < gridSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + ring - 1, v + 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + ring - 1, v + 1, v + 2);

        // 中间的网格
        int vMin = ring * (gridSize + 1) - 1;  // 左边的顶点
        int vMid = vMin + 1;                // 中间的顶点
        int vMax = v + 2;                   // 右边的顶点
        for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMin - 1, vMid, vMid + gridSize - 1);
            for (int x = 1; x < gridSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + gridSize - 1, vMid + 1, vMid + gridSize);
            }
            t = SetQuad(triangles, t, vMid, vMid + gridSize - 1, vMax, vMax + 1);
        }

        // 最后一排
        int vTop = vMin - 2;                // 最后一条边的顶点
        t = SetQuad(triangles, t, vMin, vTop + 1, vMid, vTop);
        for (int x = 1; x < gridSize - 1; x++, vMid++, vTop--)
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
        int vMid = _vertices.Length - (gridSize - 1) * (gridSize - 1);
        //int vMid = ring * (gridSize + 1) + (gridSize - 1) * (gridSize - 1);

        // 底面的第一排
        t = SetQuad(triangles, t, ring - 1, 0, vMid, 1);
        for (int x = 1; x < gridSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, v, vMid + 1, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v, v + 2, v + 1);

        // 中间的网格
        int vMin = ring - 1;                // 左边的顶点
        int vMax = v + 2;                   // 右边的顶点
        vMid -= (gridSize - 2);
        for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin - 1, vMin, vMid + gridSize - 1, vMid);
            for (int x = 1; x < gridSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid + gridSize - 1, vMid, vMid + gridSize, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + gridSize - 1, vMid, vMax + 1, vMax);
        }

        // 最后一排
        int vTop = vMin - 2;                // 最后一条边的顶点
        t = SetQuad(triangles, t, vTop + 1, vMin, vTop, vMid);
        for (int x = 1; x < gridSize - 1; x++, vMid++, vTop--)
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
        gameObject.AddComponent<SphereCollider>();
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
