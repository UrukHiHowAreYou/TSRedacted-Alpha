using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private TextMeshProUGUI playersInGameText;
    
    private void Awake() {
        serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            Logger.Instance.LogInfo("Server started...");
        });
        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            Logger.Instance.LogInfo("Host started...");
        });
        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            Logger.Instance.LogInfo("Client started...");
        });
    }

    private void Update() {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }
}
