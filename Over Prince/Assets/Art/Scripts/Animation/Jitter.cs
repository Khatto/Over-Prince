using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    public Vector3 jitterRange;
    public bool active = false;
    private float direction = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {
            Vector3 jitterDisplacement = new Vector3(
                jitterRange.x * direction,
                jitterRange.y * direction,
                jitterRange.z * direction
            );
            transform.position += jitterDisplacement;
            direction *= -1;
        }
    }
}
