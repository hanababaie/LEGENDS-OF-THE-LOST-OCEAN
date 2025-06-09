using UnityEngine;

public class mianmenu : MonoBehaviour
{
    public GameObject pic;
    public GameObject mainmenu;

    public float time = 2f;

    void Start()
    {
        
        pic.SetActive(true); // show the pic
        mainmenu.SetActive(false); // do not show the main menu
        
        Invoke("ShowMenu", time); // after a time we show the menu
    }

    void ShowMenu()
    {
        pic.SetActive(false); // do not show the pic anymore
        mainmenu.SetActive(true);
    }
}