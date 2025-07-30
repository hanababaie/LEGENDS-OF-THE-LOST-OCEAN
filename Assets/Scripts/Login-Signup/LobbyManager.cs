using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField ipInputField;

    public void HostGame()
    {
        NetworkManager.singleton.StartHost();
        SceneManager.LoadScene("level1"); // چون Online Scene خالیه
    }

    public void JoinGame()
    {
        NetworkManager.singleton.networkAddress = ipInputField.text;
        NetworkManager.singleton.StartClient();
        // صحنه level1 توسط سرور لود خواهد شد
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("mianmenu");
    }
}