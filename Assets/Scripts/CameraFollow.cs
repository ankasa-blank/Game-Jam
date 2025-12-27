using UnityEngine;

public class CameraFollowAngle : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 fixedRotation;

    void LateUpdate()
    {
        transform.position = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            offset.z
        );

        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
