using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationGrid : MonoBehaviour
{
    public Transform prefab;
    public int gridResolution = 10;

    private Transform[] grid;
    private List<Transformation> transformations;
    private Matrix4x4 transformation;

    private void Awake()
    {
        transformations = new List<Transformation>();

        grid = new Transform[gridResolution * gridResolution * gridResolution];
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    grid[i] = CreateGridPoint(x, y, z);
                }
            }
        }
    }

    private void Update()
    {
        UpdateTransformation();

        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    grid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }
    }

    private void UpdateTransformation()
    {
        // 为什么使用List而不是数组？
        // GetComponents方法的最直接的版本只是返回一个包含请求类型的所有组件的数组。 这意味着每次调用都会创建一个新数组，在本例中是每次Update。 替代版本具有列表参数。 这样做的好处是它将把组件放到列表中，而不是创建一个新的数组。
        // 这不是一个关键的优化，但是当需要经常获取组件时，使用list是个好习惯。
        GetComponents<Transformation>(transformations);
        if (transformations.Count > 0)
        {
            transformation = transformations[0].Matrix;
            for (int i = 1; i < transformations.Count; i++)
            {
                transformation = transformations[i].Matrix * transformation;
            }
        }
    }

    /// <summary>
    /// 创建网格上的点
    /// </summary>
    private Transform CreateGridPoint(int x, int y, int z)
    {
        Transform point = Instantiate<Transform>(prefab);
        point.GetComponent<MeshRenderer>().material.color = new Color(
            (float)x / gridResolution,
            (float)y / gridResolution,
            (float)z / gridResolution
        );
        return point;
    }

    /// <summary>
    /// 获得网格点的坐标
    /// </summary>
    private Vector3 GetCoordinates(int x, int y, int z)
    {
        Vector3 coordinates = new Vector3(
            x - (gridResolution - 1) * 0.5f,
            y - (gridResolution - 1) * 0.5f,
            z - (gridResolution - 1) * 0.5f
        );
        return coordinates;
    }

    /// <summary>
    /// 转换坐标
    /// 通过获取原始坐标，然后应用每个变换来完成每个点的变换。 但不能依靠每个点的实际位置，因为已经对它们进行了变换，并且我们不想在每个帧上累积变换。
    /// </summary>
    private Vector3 TransformPoint(int x, int y, int z)
    {
        Vector3 coordinates = GetCoordinates(x, y, z);
        //// 当存在多个变换的组件时，需要应用所有变换后的值才行
        //for (int i = 0; i < transformations.Count; i++)
        //{
        //    coordinates = transformations[i].Apply(coordinates);
        //}
        //return coordinates;
        return transformation.MultiplyPoint(coordinates);
    }
}
