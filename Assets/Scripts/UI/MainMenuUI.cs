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

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Debug.Log("Play it again Sam!");
            Loader.Load(Loader.Scene.Playground);
        }
        );
        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Hit (it and) Quit (it) :() ");
            Application.Quit();
        }
        );
        lightSwitch.onClick.AddListener(() =>
        {
            Debug.Log("Hit the Splights!");
            ceilingLight.enabled = !ceilingLight.enabled;
        }
        );


        Time.timeScale = 1f;
    }
}
