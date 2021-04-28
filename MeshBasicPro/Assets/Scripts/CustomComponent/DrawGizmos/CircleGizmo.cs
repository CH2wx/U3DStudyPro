using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 把一个正方形到一个圆的映射进行可视化
/// </summary>
public class CircleGizmo : MonoBehaviour {
    public int resolution = 10;

    private void OnDrawGizmosSelected()
    {
        float step = 2f / resolution;
        for (int i = 0; i <= resolution; i++)
        {
            // 上下边
            ShowPoint(i * step - 1f, -1f);
            ShowPoint(i * step - 1f, 1f);
            // 左右边
            ShowPoint(-1f, i * step - 1f);
            ShowPoint(1f, i * step - 1f);
        }
    }

    private void ShowPoint (float x, float y)
    {
        Vector2 square = new Vector2(x, y);
        // 通过Unity的归一化进行映射（直接拉伸导致的三角片规则不均匀）
        //Vector2 circle = square.normalized;

        // 为了让三角片分布的更均匀，通过计算得到如下坐标
        // 因为圆的半径为1，正方形上边的x,y,z总有一个值为1，所以可以通过 1 - (1 - x^2) * (1 - y^2) 得到一个圆上的顶点
        // 简化后有：circle = (x^2 - x^2*y^2 / 2) + (y^2 - x^2*y^2 / 2)
        // 对circle平均分割成两个坐标，得到：(x * √(1 - y^2 / 2), y * √(1 - x^2 / 2))
        Vector2 circle;
        circle.x = square.x * Mathf.Sqrt(1 - Mathf.Pow(square.y, 2) / 2);
        circle.y = square.y * Mathf.Sqrt(1 - Mathf.Pow(square.x, 2) / 2);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(square, 0.025f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(circle, 0.025f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(square, circle);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(circle, Vector2.zero);
    }
}
