using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    private CustomNetworkManager customNetworkManager;

    void Start()
    {
        // NetworkManager.singleton رو به CustomNetworkManager تبدیل کن
        customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;
    }

    public void HostGame()
    {
        customNetworkManager.StartHost();
      
    }

    public void JoinGame()
    {
        customNetworkManager.networkAddress = ipInputField.text;
        customNetworkManager.StartClient();
        // صحنه توسط سرور لود خواهد شد
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("mianmenu");
    }
}