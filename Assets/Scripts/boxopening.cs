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

    private GameObject boxUI;
    private Image imageBox;
    private TextMeshProUGUI textBox;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isBoxUIActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBoxUI();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            player = other.gameObject;
            isOpened = true;
            Debug.Log("Box opened!");

            StartCoroutine(OpenBox());
        }
    }

    IEnumerator OpenBox()
    {
        animator.SetTrigger("open");
        yield return new WaitForSeconds(1.5f);

        lootsystem item = GetRandomItem();
        if (item == null)
        {
            Debug.LogWarning("No item dropped.");
            yield break;
        }

        if (player.TryGetComponent<playermovement1>(out var p1))
        {
            boxUI = p1.boxUI;
        }
        else if (player.TryGetComponent<playermovement2>(out var p2))
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
            imageBox.sprite = item.lootsprite;
            textBox.text = item.lootname;
            isBoxUIActive = true;
        }

        GameObject tempLoot = new GameObject("TempLoot");
        var take = tempLoot.AddComponent<takingitems>();
        take.loot = item;
        take.OnTriggerEnter2D(player.GetComponent<Collider2D>());
        Destroy(tempLoot);

        Debug.Log($"Player got: {item.lootname}");

        // صبر تا زمانی که پلیر Escape بزنه یا 3 ثانیه بگذره
        float timer = 0f;
        while (timer < 3f && isBoxUIActive)
        {
            timer += Time.deltaTime;
            yield return null;
        }

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
        List<lootsystem> possible = new List<lootsystem>();

        foreach (lootsystem item in items)
        {
            if (random <= item.dropchance)
            {
                possible.Add(item);
            }
        }

        if (possible.Count == 0) return null;

        return possible[Random.Range(0, possible.Count)];
    }
}
