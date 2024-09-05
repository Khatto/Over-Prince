using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool active = false;
    public float followSpeed = 0.1f;
    public float yOffset = 1f;
    public Transform target;
    public float cameraZPos = -10f;

    public Range2D cameraRangeX = new Vector2(-1000f, 1000f);
    public Range2D cameraRangeY = new Vector2(-1000f, 1000f);
    
    // Update is called once per frame
    void Update()
    {
        if (active && target != null) {
            FollowTarget();
        }
    }

    void FollowTarget() {
        Vector3 newPos = new Vector3(
            Mathf.Clamp(target.position.x, cameraRangeX.min, cameraRangeX.max), 
            Mathf.Clamp(target.position.y + yOffset, cameraRangeY.min, cameraRangeY.max), 
            cameraZPos
        );
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed);
    }
}
