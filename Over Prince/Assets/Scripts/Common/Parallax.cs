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
    public SimpleMovement simpleMovement;

    
    public void Start() {
        AddSimpleMovementIfNecessary();
    }

    public void AddSimpleMovementIfNecessary() {
        if (this.simpleMovement == null) {
            this.simpleMovement = this.transform.gameObject.AddComponent<SimpleMovement>();
        } else {
            this.simpleMovement = this.transform.gameObject.GetComponent<SimpleMovement>();
        }
    }


    public void Move() {
        if (parallaxType == ParallaxType.Container) {
            foreach (Transform child in transform) {
                if (child.GetComponent<Parallax>() != null && child.GetComponent<Parallax>().parallaxType == ParallaxType.MovingElement) {
                    Parallax childParallax = child.GetComponent<Parallax>();
                    childParallax.AddSimpleMovementIfNecessary();
                    SimpleMovement simpleMovement = child.GetComponent<SimpleMovement>();
                    simpleMovement.SetMovementValues(childParallax.movementTime, childParallax.movementDelta, childParallax.easingFunction);
                    simpleMovement.Move();
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
