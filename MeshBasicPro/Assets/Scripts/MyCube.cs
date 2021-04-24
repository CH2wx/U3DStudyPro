using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MyCube : MonoBehaviour
{
    public int xSize, ySize, zSize;
    private Mesh _mesh;
    private Vector3[] _vertices;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        _mesh = new Mesh();
        _mesh.name = "Procedural Cube";
        GetComponent<MeshFilter>().mesh = _mesh;

        WaitForSeconds wait = new WaitForSeconds(0.05f);

        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = (
                (xSize - 1) * (ySize - 1) +
                (xSize - 1) * (zSize - 1) +
                (ySize - 1) * (zSize - 1)
            ) * 2;
        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;
        // 盖四个面
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                _vertices[v++] = new Vector3(x, y, 0);
                yield return wait;
            }
            for (int z = 0; z <= zSize; z++)
            {
                _vertices[v++] = new Vector3(xSize, y, z);
                yield return wait;
            }
            for (int x = xSize - 1; x >= 0; x--)
            {
                _vertices[v++] = new Vector3(x, y, zSize);
                yield return wait;
            }
            for (int z = zSize - 1; z > 0; z--)
            {
                _vertices[v++] = new Vector3(0, y, z);
                yield return wait;
            }
        }
        print(v);

        // 盖上下两面
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                _vertices[v++] = new Vector3(x, 0, z);
                yield return wait;
            }
        }
        print(v);
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                _vertices[v++] = new Vector3(x, ySize, z);
                yield return wait;
            }
        }
        print(v);

        yield return wait;
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null || _vertices.Length <= 0)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}
