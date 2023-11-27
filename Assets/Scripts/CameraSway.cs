using UnityEngine;
using System.Collections;

public class CameraSway : MonoBehaviour
{
    public float swayAmount = 0.1f; // The amount of sway
    public float swaySpeed = 1f; // The speed of the sway

    private Vector3 initialPosition;

    private void Start()
    {
        // Store the initial position of the camera
        initialPosition = transform.localPosition;

        // Start the swaying coroutine
        StartCoroutine(SwayRoutine());
    }

    IEnumerator SwayRoutine()
    {
        while (true)
        {
            // Calculate the sway using Perlin noise for smooth motion
            float offsetX = Mathf.PerlinNoise(Time.time * swaySpeed, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * swaySpeed) * 2f - 1f;

            // Apply the sway to the camera's local position
            Vector3 swayOffset = new Vector3(offsetX, offsetY, 0f) * swayAmount;
            transform.localPosition = initialPosition + swayOffset;

            yield return null;
        }
    }
}