using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Rigidbody2D rigidBody;
    private Collider2D collider2D;
    public Transform target;
    public Collider2D targetCollider;
    public float speed = 7.0f;
    public float minDistanceFromTarget = 0.09f;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        targetCollider = target.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        LinearlyMoveTowardsTarget();
    }

    void LinearlyMoveTowardsTarget()
    {   
        bool isTouchingTarget = collider2D.IsTouching(targetCollider);
        if (!isTouchingTarget && Vector3.Distance(target.transform.position, rigidBody.transform.position) > minDistanceFromTarget)
        {
            Vector3 direction = (target.transform.position - rigidBody.transform.position).normalized;
            Vector3 moveVector = direction * speed * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.transform.position + moveVector);
        }
    }
    
}