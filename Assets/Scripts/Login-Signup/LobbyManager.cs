using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public CustomNetworkManager customNetworkManager; // اینجا تغییر کرد

    void Start()
    {
        // NetworkManager.singleton رو به CustomNetworkManager تبدیل کن
        customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;
    }

    public void HostGame()
    {
        customNetworkManager.StartHost();
        SceneManager.LoadScene("level1online");
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