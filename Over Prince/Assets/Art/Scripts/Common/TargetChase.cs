using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/***
* This script is used to make an object chase another object.
*/
public class TargetChase : MonoBehaviour
{
    public Transform target;
    public ChaseType chaseType = ChaseType.Lerp;
    public float chaseSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            ChaseTarget();
        }
    }

    private void ChaseTarget() {
        switch (chaseType) {
            case ChaseType.Lerp:
                transform.position = Vector3.Lerp(transform.position, target.position, chaseSpeed);
                break;
            case ChaseType.Slerp:
                transform.position = Vector3.Slerp(transform.position, target.position, chaseSpeed);
                break;
            case ChaseType.Instantaneous:
                transform.position = target.position;
                break;
            case ChaseType.InstantaneousOnlyX:
                Vector3 newPositionX = transform.position;
                newPositionX.x = target.position.x;
                transform.position = newPositionX;
                break;
            case ChaseType.InstantaneousOnlyY:
                Vector3 newPositionY = transform.position;
                newPositionY.y = target.position.y;
                transform.position = newPositionY;
                break;
        }
    }
}

public enum ChaseType {
    Lerp,
    Slerp,
    Instantaneous,
    InstantaneousOnlyX,
    InstantaneousOnlyY
}