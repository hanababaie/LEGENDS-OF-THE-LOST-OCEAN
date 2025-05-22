using UnityEngine;

public class SplashToMenu : MonoBehaviour
{
    public GameObject pic;  // پنل عکس
    public GameObject mainmenu;    // پنل منو

    public float splashTime = 2f;   // مدت زمان نمایش عکس

    void Start()
    {
        // اول فقط پنل عکس فعال باشه، منو غیر فعال
        pic.SetActive(true);
        mainmenu.SetActive(false);

        // بعد از splashTime، نمایش منو
        Invoke("ShowMenu", splashTime);
    }

    void ShowMenu()
    {
        pic.SetActive(false);
        mainmenu.SetActive(true);
    }
}