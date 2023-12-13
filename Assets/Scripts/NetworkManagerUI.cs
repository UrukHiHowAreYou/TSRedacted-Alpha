using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private TextMeshProUGUI playersInGameText;
    // [SerializeField] private TextMeshProUGUI ipAddressText;
    // [SerializeField] private TMP_InputField ip;
    // [SerializeField] string ipAddress;
    // [SerializeField] UnityTransport transport;
    
    
    private void Awake() {
        serverBtn.onClick.AddListener(() => {
            if(NetworkManager.Singleton.StartServer()) {
                Logger.Instance.LogInfo("Server started...");
            } else {
                Logger.Instance.LogInfo("Server did not start...");
            }
        });
        hostBtn.onClick.AddListener(() => {
            if(NetworkManager.Singleton.StartHost()) {
                // GetLocalIPAddress();
                Logger.Instance.LogInfo("Host started...");
            } else {
                Logger.Instance.LogInfo("Host did not start...");
            }
        });
        clientBtn.onClick.AddListener(() => {
            if(NetworkManager.Singleton.StartClient()) {
                // ipAddress = ip.text;
                // Debug.Log("ipAddress has been updated upon clicking the client and now equals = "+ipAddress);
                // SetIpAddress();
                Logger.Instance.LogInfo("Client started...");
                Debug.Log("I clicked the client and the, client connected..");
            } else {
                Logger.Instance.LogInfo("Client did not start...");
            }
        });
    }

    // void Start()
	// {
	// 	ipAddress = "0.0.0.0";
	// 	SetIpAddress(); // Set the Ip to the above address
	// }

    private void Update() {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    /* Gets the Ip Address of your connected network and
	shows on the screen in order to let other players join
	by inputing that Ip in the input field */
	// ONLY FOR HOST SIDE 
	// public string GetLocalIPAddress() {
	// 	var host = Dns.GetHostEntry(Dns.GetHostName());
	// 	foreach (var ip in host.AddressList) {
	// 		if (ip.AddressFamily == AddressFamily.InterNetwork) {
	// 			ipAddressText.text = ip.ToString();
	// 			ipAddress = ip.ToString();
    //             Debug.Log("ipAddress = "+ip.ToString());
	// 			return ip.ToString();
	// 		}
	// 	}
	// 	throw new System.Exception("No network adapters with an IPv4 address in the system!");
	// }

    /* Sets the Ip Address of the Connection Data in Unity Transport
	to the Ip Address which was input in the Input Field */
	// ONLY FOR CLIENT SIDE
	// public void SetIpAddress() {
	// 	transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
	// 	transport.ConnectionData.Address = ipAddress;
	// }
}
