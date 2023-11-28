using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Array of cameras to switch between
    public float switchInterval = 5f; // Time interval between camera switches

    private int[] previousIndices = { -1, -1 };

    private bool firstLoop = true;

    private void Start()
    {
        StartCoroutine(SwitchCameraRoutine());
    }

    IEnumerator SwitchCameraRoutine()
    {
        // Show the first camera initially
        ShowCamera(0);

        while (true)
        {
            yield return new WaitForSeconds(switchInterval);

            // Randomly switch to another camera by generating a random index
            int randomIndex = GetRandomCameraIndex();
            ShowCamera(randomIndex);
        }
    }

    void ShowCamera(int index)
    {
        // Disable all cameras
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }

        // Enable the specified camera
        cameras[index].enabled = true;

        // Update the previous index
        if (firstLoop) {
            previousIndices[0] = index;
            firstLoop = false;
        } else {
            previousIndices[1] = index;
            firstLoop = true;
        }

        Debug.Log("Index = "+index);
        Debug.Log("Previous Indices = "+previousIndices[0]+", "+previousIndices[1]);
    }


    int GetRandomCameraIndex()
    {
        int randomIndex;

        // Generate a random index, excluding the previous two indices
        do
        {
            randomIndex = Random.Range(0, cameras.Length);
        } while (randomIndex == previousIndices[0] || randomIndex == previousIndices[1]);

        return randomIndex;
    }
}