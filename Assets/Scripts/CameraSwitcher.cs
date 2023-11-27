using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Array of cameras to switch between
    public float switchInterval = 5f; // Time interval between camera switches

    private void Start()
    {
        StartCoroutine(SwitchCameraRoutine());
    }

    IEnumerator SwitchCameraRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);

            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        // Randomly select a camera from the array
        int randomIndex = Random.Range(0, cameras.Length);

        // Disable all cameras
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }

        // Enable the selected camera
        cameras[randomIndex].enabled = true;
    }
}