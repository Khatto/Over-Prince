using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;

public class SimpleMovement : MonoBehaviour
{

    public float movementTime = 0.5f;
    public Vector2 movementDelta = new Vector2(0.0f, 0.0f);
    private System.Func<float, float> easingFunction = EasingFuncs.Linear;
    public EasingFunctionType easingFunctionType = EasingFunctionType.Linear;
    public SimpleMovementState state = SimpleMovementState.NotMoving;
    
    public IEnumerator ChangeObjectPos(Transform transform, Vector2 positionDelta, float duration, System.Func<float, float> easingFunction) {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + positionDelta;
        float timeElapsed = 0;
        state = SimpleMovementState.Moving;

        while (timeElapsed <= duration && state == SimpleMovementState.Moving) {
            transform.position = Vector2.Lerp(startPosition, endPosition, easingFunction(timeElapsed / duration));
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        transform.position = endPosition;
        state = SimpleMovementState.NotMoving;
    }

    public void Move() {
        StartCoroutine(ChangeObjectPos(transform, movementDelta, movementTime, easingFunction));
    }

    public void SetMovementTime(float movementTime) {
        this.movementTime = movementTime;
    }

    public void SetMovementDelta(Vector2 movementDelta) {
        this.movementDelta = movementDelta;
    }

    public void SetEasingFunctionType(EasingFunctionType easingFunctionType) {
        this.easingFunctionType = easingFunctionType;
        this.easingFunction = easingFunctionType.GetEasingFunction();
    }

    public void SetEasingFunction(System.Func<float, float> easingFunction) {
        this.easingFunction = easingFunction;
    }

    public void SetMovementValues(float movementTime, Vector2 movementDelta, System.Func<float, float> easingFunction) {
        this.movementTime = movementTime;
        this.movementDelta = movementDelta;
        this.easingFunction = easingFunction;
    }
    
    public void StopMovement() {
        state = SimpleMovementState.NotMoving;
    }
}

public enum SimpleMovementState {
    NotMoving,
    Moving
}