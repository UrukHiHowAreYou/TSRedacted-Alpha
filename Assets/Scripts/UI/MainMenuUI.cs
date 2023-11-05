using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Debug.Log("Hit Play!");
            Loader.Load(Loader.Scene.Playground);
        }
        );
        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Hit Quit :( ");
            Application.Quit();
        }
        );

        Time.timeScale = 1f;
    }
}
