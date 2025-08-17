using UnityEngine;

public class mianmenu : MonoBehaviour
{
    public GameObject pic;
    public GameObject mainmenu;

    public float time = 2f;

    void OnEnable()
    {
        pic.SetActive(true);
        mainmenu.SetActive(false);
        Invoke("ShowMenu", time);
    }

    void ShowMenu()
    {
        pic.SetActive(false); // do not show the pic anymore
        mainmenu.SetActive(true);
    }
}