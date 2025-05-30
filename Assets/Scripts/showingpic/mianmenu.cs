using UnityEngine;

public class mianmenu : MonoBehaviour
{
    public GameObject pic;
    public GameObject mainmenu;

    public float splashTime = 2f;

    void Start()
    {
        
        pic.SetActive(true);
        mainmenu.SetActive(false);
        
        Invoke("ShowMenu", splashTime);
    }

    void ShowMenu()
    {
        pic.SetActive(false);
        mainmenu.SetActive(true);
    }
}