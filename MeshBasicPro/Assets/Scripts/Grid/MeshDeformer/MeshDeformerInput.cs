using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MeshDeformerInput : MonoBehaviour {
    /// <summary>
    /// 用户的输入时，给的力的大小
    /// </summary>
    public float force = 10f;
    /// <summary>
    /// 力的方向产生的偏移程度
    /// </summary>
    public float forceOffset = 0.1f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
        else
        {
        }
    }

    /// <summary>
    /// 玩家输入后的处理逻辑
    /// </summary>
    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(inputRay);

        if (hits.Length > 0)
        {
            RaycastHit hit = hits[0];
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer != null)
            {
                Vector3 point = hit.point;
                // 对不同的力产生偏移效果
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
        }
    }
}
