using UnityEngine;
[ExecuteInEditMode]
public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = new Vector3(0, 0 , 0);

    void Update()
    {
        transform.LookAt(lookAtPoint);
        lookAtPoint.x ++;
        lookAtPoint.y ++;
        lookAtPoint.z ++;
    }
}