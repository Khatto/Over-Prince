using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float rotationTime = 1f; // Time in seconds to complete a full rotation
    private float currentRotation = 0f; // Current rotation value
    private float rotationSpeed; // Speed of rotation per frame
    public Axis rotationAxis = Axis.X; // Axis to rotate along

    private void Start()
    {
        // Calculate rotation speed based on rotation time
        rotationSpeed = 360f / rotationTime;
    }

    private void Update()
    {
        // Update the rotation value based on rotation speed and time
        currentRotation += rotationSpeed * Time.deltaTime;

        // Wrap the rotation value within 360 degrees
        currentRotation %= 360f;

        // Apply the rotation to the object based on the selected axis
        switch (rotationAxis)
        {
            case Axis.X:
                transform.rotation = Quaternion.Euler(currentRotation, 0f, 0f);
                break;
            case Axis.Y:
                transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
                break;
            case Axis.Z:
                transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
                break;
        }
    }
}

public enum Axis
{
    X,
    Y,
    Z
}

