using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    //احتمالا مدل پایین حذف شود//
    public void StartOfflineMode()
    {
        // به صحنه بازی آفلاین برو
        SceneManager.LoadScene("level1");
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