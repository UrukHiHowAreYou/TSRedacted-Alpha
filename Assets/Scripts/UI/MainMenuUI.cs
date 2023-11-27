using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button lightSwitch;
    [SerializeField] private Light ceilingLight;
    [SerializeField] private Light[] spotlights; // Add spotlights here

    private bool isTemperatureHigh = true; // Flag to track current temperature state

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Debug.Log("Play it again Sam!");
            Loader.Load(Loader.Scene.Playground);
        });

        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Hit (it and) Quit (it) :() ");
            Application.Quit();
        });

        lightSwitch.onClick.AddListener(() =>
        {
            Debug.Log("Hit the Splights!");
            ToggleCeilingLight();
            SwitchSpotlightTemperature();
        });

        Time.timeScale = 1f;
    }

    private void ToggleCeilingLight()
    {
        ceilingLight.enabled = !ceilingLight.enabled;
    }

    private void SwitchSpotlightTemperature()
    {
        // Switch temperature for each spotlight
        foreach (Light spotlight in spotlights)
        {
            if (spotlight != null)
            {
                // Toggle between high and low temperatures
                if (isTemperatureHigh)
                {
                    spotlight.colorTemperature = 1500f; // Set to low temperature
                }
                else
                {
                    spotlight.colorTemperature = 6570f; // Set to high temperature
                }
            }
        }

        // Toggle the temperature state
        isTemperatureHigh = !isTemperatureHigh;
    }
}