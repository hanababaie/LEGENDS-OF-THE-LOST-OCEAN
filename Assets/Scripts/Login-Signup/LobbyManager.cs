using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public void StartOfflineMode()
    {
        // به صحنه بازی آفلاین برو
        SceneManager.LoadScene("OfflineGameScene");
    }

    public void StartOnlineMode()
    {
        // به صحنه بازی آنلاین برو
        SceneManager.LoadScene("OnlineGameScene");
    }

    public void BackToLogin()
    {
        SceneManager.LoadScene("LoginMenu");
    }
}