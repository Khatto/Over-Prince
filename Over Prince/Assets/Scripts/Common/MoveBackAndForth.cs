using Unity.VisualScripting;
using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public EasingFunctionType easingFunctionType = EasingFunctionType.Linear;

    public Vector2 movementDelta = new Vector2(0.0f, 0.0f);

    public bool active = false;

    public float totalDuration = 0.0f;

    public SimpleMovement simpleMovement;

    void Start()
    {
        simpleMovement = gameObject.GetOrAddComponent<SimpleMovement>();
        simpleMovement.SetMovementValues(totalDuration, movementDelta, easingFunctionType.GetEasingFunction());
        if (active) {
            simpleMovement.Move();
        }
    }

    void Update()
    {
        if (active && simpleMovement.state == SimpleMovementState.NotMoving)
        {
            SwitchDirection();
        }
    }

    private void SwitchDirection() {
        simpleMovement.movementDelta = -simpleMovement.movementDelta;
        simpleMovement.Move();
    }

    public void StartMovement() {
        active = true;
        simpleMovement.Move();
    }

    public void StopMovement() {
        active = false;
        simpleMovement.StopMovement();
    }
}
