using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (isBoxUIActive && Input.GetKeyDown(KeyCode.Escape)) // pressing the  key and close it
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

            StartCoroutine(OpenBox()); // start the process
        }
    }

    IEnumerator OpenBox()
    {
        animator.SetTrigger("open");
        yield return new WaitForSeconds(3f); // showing the open animation
        
        if (boxUI != null)
        {
            boxUI.SetActive(true);
            
            isBoxUIActive = true;
        }

        
        float timer = 0f;
        while (timer < 8f && isBoxUIActive)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // wait for a time to close the ui
        CloseBoxUI();

        Destroy(gameObject);
    }

    void CloseBoxUI()
    {
        if (boxUI != null)
        {
            boxUI.SetActive(false);
        }
        isBoxUIActive = false;
    }

    
}
