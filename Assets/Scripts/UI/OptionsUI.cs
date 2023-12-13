using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using StarterAssets;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; }

    private Action onCloseButtonAction;

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    //[SerializeField] private Button moveUpButton;
    //[SerializeField] private Button moveDownButton;
    //[SerializeField] private Button moveRightButton;
    //[SerializeField] private Button moveLeftButton;
    //[SerializeField] private Button interactButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button secondaryFireButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button sprintButton;
    [SerializeField] private Button aimButton;
    //[SerializeField] private Button pauseButton;
    //[SerializeField] private Button gamepadInteractButton;
    //[SerializeField] private Button gamepadInteractAlternateButton;
    //[SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    //[SerializeField] private TextMeshProUGUI moveUpText;
    //[SerializeField] private TextMeshProUGUI moveDownText;
    //[SerializeField] private TextMeshProUGUI moveLeftText;
    //[SerializeField] private TextMeshProUGUI moveRightText;
    //[SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI shootText;
    [SerializeField] private TextMeshProUGUI secondaryFireText;
    [SerializeField] private TextMeshProUGUI jumpText;
    [SerializeField] private TextMeshProUGUI sprintText;
    [SerializeField] private TextMeshProUGUI aimText;
    //[SerializeField] private TextMeshProUGUI pauseText;
    //[SerializeField] private TextMeshProUGUI gamepadInteractText;
    //[SerializeField] private TextMeshProUGUI gamepadInteractAlternateText;
    //[SerializeField] private TextMeshProUGUI gamepadPauseText;
    //[SerializeField] private Transform pressToRebindKeyTransform;

    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });
        //moveUpButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Move_Up); });
        //moveDownButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Move_Down); });
        //moveLeftButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Move_Left); });
        //moveRightButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Move_Right); });
        //interactButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Interact); });
        shootButton.onClick.AddListener(() => { RebindBinding(StarterAssetsInputs.PlayerActions.Shoot); });
        secondaryFireButton.onClick.AddListener(() => { RebindBinding(StarterAssetsInputs.PlayerActions.SecondaryFire); });
        aimButton.onClick.AddListener(() => { RebindBinding(StarterAssetsInputs.PlayerActions.Aim); });
        sprintButton.onClick.AddListener(() => { RebindBinding(StarterAssetsInputs.PlayerActions.Sprint); });
        jumpButton.onClick.AddListener(() => { RebindBinding(StarterAssetsInputs.PlayerActions.Jump); });
        //pauseButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Pause); });
        //gamepadInteractButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Gamepad_Interact); });
        //gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Gamepad_InteractAlternate); });
        //gamepadPauseButton.onClick.AddListener(() => { RebindBinding(PlayerInput.Binding.Gamepad_Pause); });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnpaused;
        // Debug.Log("------------- yo ongameunpaused ongameunpaused bitch! -------------");
        Hide();
        // Debug.Log("------------- yo hide me bitch! -------------");
        UpdateVisual();
        // WARNING: THE BELOW ISN'T SHOWING
        // Debug.Log("------------- yo update me visual bitch! -------------");
        
        //HidePressToRebindKeyTransform();
    }

    public void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        // Debug.Log("++++///////////////////// UPDATING Visual ////////////////////++++");
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        //moveUpText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Up);
        //moveDownText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Down);
        //moveLeftText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Left);
        //moveRightText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Right);
        //interactText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Interact);
        //interactAlternateText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.InteractAlternate);
        //pauseText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Pause);
        //gamepadInteractText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Gamepad_Interact);
        shootText.text = StarterAssetsInputs.Instance.GetBindingText(StarterAssetsInputs.PlayerActions.Shoot);
        // WARNING: THE BELOW ISN'T SHOWING
        Debug.Log("++++///////////////////// THIS AIN'T SHOWING ////////////////////++++");
        secondaryFireText.text = StarterAssetsInputs.Instance.GetBindingText(StarterAssetsInputs.PlayerActions.SecondaryFire);
        aimText.text = StarterAssetsInputs.Instance.GetBindingText(StarterAssetsInputs.PlayerActions.Aim);
        sprintText.text = StarterAssetsInputs.Instance.GetBindingText(StarterAssetsInputs.PlayerActions.Sprint);
        jumpText.text = StarterAssetsInputs.Instance.GetBindingText(StarterAssetsInputs.PlayerActions.Jump);
        //gamepadPauseText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    //public void ShowPressToRebindKeyTransform()
    //{
    //    pressToRebindKeyTransform.gameObject.SetActive(true);
    //}

    //private void HidePressToRebindKeyTransform()
    //{
    //    pressToRebindKeyTransform.gameObject.SetActive(false);
    //}

    private void RebindBinding(StarterAssetsInputs.PlayerActions playerAction)
    {
        //soundEffectsText.text =
        //ShowPressToRebindKeyTransform();
        Debug.Log("REBIND BINDING");
        StarterAssetsInputs.Instance.RebindBinding(playerAction, () => {
            //HidePressToRebindKeyTransform();
            UpdateVisual();
        }
        );
    }
}
