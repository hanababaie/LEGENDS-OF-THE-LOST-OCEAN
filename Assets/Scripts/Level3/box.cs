using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // برای رفتن به منوی اصلی

public class box : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false;
    private bool isBoxUIActive = false;

    public GameObject boxUI;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isBoxUIActive && Input.GetKeyDown(KeyCode.Escape)) // بستن با ESC
        {
            CloseBoxUI();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            isOpened = true;
            Debug.Log("Box opened!");

            StartCoroutine(OpenBox());
        }
    }

    IEnumerator OpenBox()
    {
        animator.SetTrigger("open");
        yield return new WaitForSeconds(3f);

        if (boxUI != null)
        {
            boxUI.SetActive(true);
            isBoxUIActive = true;
        }

        if (sencemanager.Instance != null)
        {
            sencemanager.Instance.Resetafterfinisfh();
            sencemanager.Instance.ResetPlayerPositions();
        }

    }

    void CloseBoxUI()
    {
        if (boxUI != null)
        {
            boxUI.SetActive(false);
        }
        isBoxUIActive = false;
    }


    public void mainmenu()
    {
        Time.timeScale = 1f;
        if (sencemanager.Instance != null)
        {
            sencemanager.Instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("Sencemanager.Instance is null in pausemenu");
        }

        SceneManager.LoadScene("mianmenu");

        CloseBoxUI();
    }
    

}
