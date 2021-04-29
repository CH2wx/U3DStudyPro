using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {
    /// <summary>
    /// 变形的Mesh
    /// </summary>
    private Mesh _deformerMesh;
    /// <summary>
    /// 变形前的顶点坐标
    /// </summary>
    private Vector3[] _originalVertices;
    /// <summary>
    /// 变形后的顶点坐标
    /// </summary>
    private Vector3[] _displacedVertices;
    /// <summary>
    /// 每个顶点的速度
    /// </summary>
    private Vector3[] _vertexVelocities;
    /// <summary>
    /// 弹力，用于当顶点变形后拉回原位置的速度
    /// </summary>
    public float springForce = 20f;
    /// <summary>
    /// 阻尼，随时间的推移而降低速度（阻尼越高，物体的弹性就越小，表现的速度也就越慢。）
    /// </summary>
    public float damping = 5f;
    /// <summary>
    /// 统一缩放的值（当变形物体进行了缩放，缩放点应该也进行缩放）
    /// </summary>
    private float uniformScale = 1f;

    private void Start()
    {
        // 因为Awake的时候生成网格，所以在Start的时候进行赋值Mesh信息
        _deformerMesh = gameObject.GetComponent<MeshFilter>().mesh;
        _originalVertices = _deformerMesh.vertices;
        _displacedVertices = new Vector3[_originalVertices.Length];
        _vertexVelocities = new Vector3[_originalVertices.Length];

        for (int i = 0; i < _originalVertices.Length; i++)
        {
            _displacedVertices[i] = _originalVertices[i];
        }
    }

    private void Update()
    {
        uniformScale = transform.localScale.x;
        // 处理每个顶点的位置。然后将位移顶点分配给网格，使其实际发生变化。因为网格的形状不再是恒定的，我们也必须重新计算它的法线
        for (int i = 0; i < _displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        _deformerMesh.vertices = _displacedVertices;
        _deformerMesh.RecalculateNormals();
    }

    /// <summary>
    /// 为指定索引的顶点添加速度
    /// </summary>
    private void UpdateVertex(int i)
    {
        Vector3 velocity = _vertexVelocities[i];
        // 计算变形的差值，然后通过弹性给拉回原始位置
        Vector3 displacement = _displacedVertices[i] - _originalVertices[i];
        displacement *= uniformScale;
        // 添加弹性拉回起始位置的速度
        velocity -= displacement * springForce * Time.deltaTime;
        // 为速度添加阻尼，防止速度过快带来拉回时出现的震荡
        velocity *= 1f - damping * Time.deltaTime;
        _vertexVelocities[i] = velocity;
        // 当物体进行缩放后，速度也需要缩放
        _displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        // 通过将变形力的位置从世界空间转换到局部空间，来防止物体发生位置变换时出现的不正确计算
        point = transform.InverseTransformPoint(point);
        Debug.DrawLine(Camera.main.transform.position, point);
        for (int i = 0; i < _displacedVertices.Length; i++)
        {
            AddForceToVertex(i, point, force);
        }
    }

    /// <summary>
    /// 为顶点添加力
    /// 力是会随着距离的推移而减弱的（即衰弱力）。根据逆平方定律，只需将力除以距离平方即可。
    /// </summary>
    private void AddForceToVertex(int i, Vector3 point, float force)
    {
        // 得到缩放点
        Vector3 pointToVertex = _displacedVertices[i] - point;
        // 对缩放点进行缩放，保证正确的距离
        pointToVertex *= uniformScale;

        // 计算衰减力
        // 将力除以1 加 距离平方是为了：保证距离为0的时候，力处于全力状态。否则，力就会在距离1的位置达到最大强度，让靠近的点无穷远的飞去。
        float attenuatedForce = force / (1f + (pointToVertex.sqrMagnitude));
        
        // 把力转化为速度
        // 公式：a = F / m ； v = a * t 
        // 忽略质量，可以得到速度的公式：v = F * t
        float velocity = attenuatedForce * Time.deltaTime;

        // 为速度添加方向
        _vertexVelocities[i] += pointToVertex.normalized * velocity;
    }
}
