using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    public ParallaxType parallaxType = ParallaxType.Container;
    public float movementTime = 0.5f;
    public Vector2 movementDelta = new Vector2(0.0f, 0.0f);
    public System.Func<float, float> easingFunction = EasingFuncs.Linear;

    // TODO: Potentially move these to a movement utility class
    public static IEnumerator ChangeObjectPos(Transform transform, Vector2 positionDelta, float duration, System.Func<float, float> easingFunction) {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + positionDelta;
        float timeElapsed = 0;

        while (timeElapsed <= duration) {
            transform.position = Vector2.Lerp(startPosition, endPosition, easingFunction(timeElapsed / duration));
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        transform.position = endPosition;
    }

    public void Move() {
        if (parallaxType == ParallaxType.Container) {
            foreach (Transform child in transform) {
                if (child.GetComponent<Parallax>() != null && child.GetComponent<Parallax>().parallaxType == ParallaxType.MovingElement) {
                    Parallax childParallax = child.GetComponent<Parallax>();
                    StartCoroutine(ChangeObjectPos(child, childParallax.movementDelta, childParallax.movementTime, childParallax.easingFunction));
                }
            }
        }
    }

    public void SetMovementTime(float movementTime) {
        if (parallaxType == ParallaxType.Container) {
            foreach (Transform child in transform) {
                if (child.GetComponent<Parallax>() != null && child.GetComponent<Parallax>().parallaxType == ParallaxType.MovingElement) {
                    child.GetComponent<Parallax>().SetMovementTime(movementTime);
                }
            }
        } else {
            this.movementTime = movementTime;
        }
    }

    public void SetMovementDelta(Vector2 movementDelta) {
        if (parallaxType == ParallaxType.Container) {
            foreach (Transform child in transform) {
                if (child.GetComponent<Parallax>() != null && child.GetComponent<Parallax>().parallaxType == ParallaxType.MovingElement) {
                    child.GetComponent<Parallax>().SetMovementDelta(movementDelta);
                }
            }
        } else {
            this.movementDelta = movementDelta;
        }
    }

    public void SetEasingFunction(System.Func<float, float> easingFunction) {
        if (parallaxType == ParallaxType.Container) {
            foreach (Transform child in transform) {
                if (child.GetComponent<Parallax>() != null && child.GetComponent<Parallax>().parallaxType == ParallaxType.MovingElement) {
                    child.GetComponent<Parallax>().SetEasingFunction(easingFunction);
                }
            }
        } else {
            this.easingFunction = easingFunction;
        }
    }
}

public enum ParallaxType {
    Container,
    MovingElement
}
