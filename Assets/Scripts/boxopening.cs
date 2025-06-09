using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class boxopening : MonoBehaviour
{
    public List<lootsystem> items;
    private Animator animator;

    private bool isOpened = false;
    private bool isBoxUIActive = false;

    private GameObject player;

    public GameObject boxUI;
    private Image imageBox;
    private TextMeshProUGUI textBox;

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
            player = other.gameObject; // we set the player
            isOpened = true;
            Debug.Log("Box opened!");

            StartCoroutine(OpenBox()); // start the process
        }
    }

    IEnumerator OpenBox()
    {
        animator.SetTrigger("open");
        yield return new WaitForSeconds(1.5f); // showing the open animation

        lootsystem item = GetRandomItem(); // get the item
        if (item == null)
        {
            Debug.LogWarning("No item dropped.");
            yield break;
        }

        // find which ui for which player should open

        if (player.TryGetComponent<playermovement1>(out var p1)) // if the player has the playermovement1 script
        {
            boxUI = p1.boxUI;
        }

        else if (player.TryGetComponent<playermovement2>(out var p2)) // if the player has the playermovement2 script
        {
            boxUI = p2.boxUI;
        }
        else
        {
            Debug.LogWarning("No playermovement script found!");
            yield break;
        }

        if (boxUI != null)
        {
            imageBox = boxUI.transform.Find("Image").GetComponent<Image>();
            textBox = boxUI.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            boxUI.SetActive(true);
            imageBox.sprite = item.lootsprite; //change the image sprite
            textBox.text = item.lootname; // cahnge the text
            isBoxUIActive = true;
        }

        GameObject tempLoot = new GameObject("TempLoot");
        var take = tempLoot.AddComponent<takingitems>(); // acessing the takingitem script
        take.loot = item;
        take.OnTriggerEnter2D(player.GetComponent<Collider2D>());
        Destroy(tempLoot); // destroy the loot

        Debug.Log($"Player got: {item.lootname}");

        float timer = 0f;
        while (timer < 4f && isBoxUIActive)
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

    lootsystem GetRandomItem()
    {
        int random = Random.Range(1, 101); // 1 to 100

        List<lootsystem> possible = new List<lootsystem>(); // create a list for possible loots

        foreach (lootsystem item in items)
        {
            if (random <= item.dropchance)
            {
                possible.Add(item); // add to the list
            }
        }

        if (possible.Count == 0) return null;

        return possible[Random.Range(0, possible.Count)]; // randomly chose 1 item form the list
    }
}
