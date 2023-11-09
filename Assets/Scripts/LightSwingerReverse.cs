using UnityEngine;

public class LightSwingerReverse : MonoBehaviour
{
    public float swingAngle = -45f; // The angle of the swing in degrees
    public float swingSpeed = 2f; // The speed of the swing

    private float initialRotationX;
    private bool isSwingingRight = true;

    void Start()
    {
        // Store the initial rotation of the light along the x-axis
        initialRotationX = transform.rotation.eulerAngles.x;
    }

    void Update()
    {
        // Calculate the rotation angle based on the swing
        float targetRotationX = isSwingingRight ? initialRotationX + swingAngle : initialRotationX - swingAngle;

        // Use EaseInOut function to achieve easing in and out
        float t = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time * swingSpeed, 1f));

        // Interpolate the rotation angle with ease in and out
        float currentRotationX = Mathf.Lerp(initialRotationX, -1*targetRotationX, t);

        // Preserve the existing rotation around the z-axis
        float currentRotationZ = transform.rotation.eulerAngles.z;

        // Apply the rotated angle
        transform.rotation = Quaternion.Euler(currentRotationX, 0f, currentRotationZ);

        // Check if the light has reached the target rotation
        if (Mathf.Abs(transform.rotation.eulerAngles.x - targetRotationX) < 0.01f)
        {
            // Change direction when the target rotation is reached
            isSwingingRight = !isSwingingRight;
        }
    }
}