using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => {
            Debug.Log("you clicked the resume Button");
            GameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            Debug.Log("you clicked the main menuButton");
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() => {
            Debug.Log("you clicked the options Button");
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }


    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

        Hide();
    }


    private void GameManager_OnGamePaused(object sender, System.EventArgs e) { Show(); }
    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e) { Hide(); }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    private void Hide() { gameObject.SetActive(false); }

}
