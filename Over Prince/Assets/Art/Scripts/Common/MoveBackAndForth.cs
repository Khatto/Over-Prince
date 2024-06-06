using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;

public class MoveBackAndForth : MonoBehaviour
{

    public System.Func<float, float> easingFunction = EasingFuncs.Linear;

    public Vector2 movementDelta = new Vector2(0.0f, 0.0f);

    public bool active = false;

    public float totalDuration = 0.0f;

    public SimpleMovement simpleMovement;

    // Start is called before the first frame update
    void Start()
    {
        simpleMovement = gameObject.GetOrAddComponent<SimpleMovement>();
        simpleMovement.SetMovementValues(totalDuration, movementDelta, easingFunction);
        simpleMovement.Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {

        }
    }

    
}
