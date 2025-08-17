using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class BossController : MonoBehaviour
{
    public float maxHealth = 500;
    private float currentHealth;

    public BossHealthUI healthUI;
    private Animator animator;
    private AudioSource audioSource;

    public float moveSpeed = 10f;
    public float attackCooldown = 0.5f;
    private float attackTimer;

    private bool isDead = false;
    private GameObject currentTarget;

    private float targetSwitchTimer;
    private float targetSwitchTime;

    public Transform[] minionSpawnPoints;
    public GameObject[] minionPrefabs;

    public float minionSpawnCooldown = 100f;
    private float minionSpawnTimer;

    public int damage = 5; // اضافه کردم

    private bool isAttacking = false;

    public int maxMinions = 5;
    private List<GameObject> activeMinions = new List<GameObject>();

    public GameObject ob;


    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        healthUI.UpdateHealthBar(1f);
        SetRandomTargetSwitchTime();
        SwitchTarget();
    }

    void Update()
    {
        Debug.Log("Time until next switch: " + (targetSwitchTime - targetSwitchTimer));

        if (isDead) return;

        targetSwitchTimer += Time.deltaTime;
        if (targetSwitchTimer >= targetSwitchTime)
        {
            SwitchTarget();
            SetRandomTargetSwitchTime();
            targetSwitchTimer = 0f;
        }

        if (currentTarget != null)
        {
            MoveTowardsTarget();
            AttackTarget();
        }

        minionSpawnTimer += Time.deltaTime;
        if (minionSpawnTimer >= minionSpawnCooldown)
        {
            StartCoroutine(SpawnMinions());
            minionSpawnTimer = 0f;
        }
    }

    void SetRandomTargetSwitchTime()
    {
        targetSwitchTime = 20f;
    }
    void SwitchTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        // فقط یک بازیکن هست
        if (players.Length == 1)
        {
            currentTarget = players[0];
            return;
        }

        // ایجاد لیست بازیکن‌ها و حذف هدف فعلی
        List<GameObject> possibleTargets = new List<GameObject>(players);
        possibleTargets.Remove(currentTarget);

        // انتخاب هدف جدید از لیست باقی‌مانده
        if (possibleTargets.Count > 0)
        {
            currentTarget = possibleTargets[Random.Range(0, possibleTargets.Count)];
        }

        // قطع حمله و شروع حرکت به سمت هدف جدید
        isAttacking = false;

        if (animator != null)
        {
            animator.ResetTrigger("attack");
            animator.SetBool("isMoving", true);
        }

        Debug.Log("Switched to target: " + currentTarget.name);
    }




    void MoveTowardsTarget()
    {
        float distance = Vector2.Distance(transform.position, currentTarget.transform.position);

        if (distance <= 5f)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                if (animator != null)
                {
                    animator.SetTrigger("attack");
                    animator.SetBool("isMoving", false);
                }
            }
            return;
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                if (animator != null)
                    animator.SetBool("isMoving", true);
            }

            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    void AttackTarget()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            if (animator != null)
                animator.SetTrigger("attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playermovement1 pm1 = collision.gameObject.GetComponent<playermovement1>();
            if (pm1 != null)
            {
                pm1.TakeDamage(damage);
                return;
            }

            playermovement2 pm2 = collision.gameObject.GetComponent<playermovement2>();
            if (pm2 != null)
            {
                pm2.TakeDamage(damage);
            }
            p1offline pmp = collision.gameObject.GetComponent<p1offline>();
            if (pmp != null)
            {
                pmp.TakeDamage(damage);
            }
            p2offline p2Off = collision.gameObject.GetComponent<p2offline>();
            if(p2Off != null)
            {
                p2Off.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        healthUI.UpdateHealthBar(currentHealth / maxHealth);
        if (animator != null)
            animator.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (animator != null)
            animator.SetTrigger("die");

        Destroy(gameObject, 1f);
        DestroyAllEnemies();
        Destroy(ob);
    }

    IEnumerator SpawnMinions()
    {
        activeMinions.RemoveAll(m => m == null);

        if (activeMinions.Count >= maxMinions) yield break;

        int count = Mathf.Min(minionPrefabs.Length, minionSpawnPoints.Length, maxMinions - activeMinions.Count);

        List<GameObject> shuffledMinions = new List<GameObject>(minionPrefabs);
        for (int i = 0; i < shuffledMinions.Count; i++)
        {
            int rnd = Random.Range(i, shuffledMinions.Count);
            var temp = shuffledMinions[rnd];
            shuffledMinions[rnd] = shuffledMinions[i];
            shuffledMinions[i] = temp;
        }

        for (int i = 0; i < count; i++)
        {
            if (Vector2.Distance(minionSpawnPoints[i].position, transform.position) < 1.5f)
                continue;

            bool tooClose = false;
            foreach (GameObject other in activeMinions)
            {
                if (other == null) continue;
                float dist = Vector2.Distance(minionSpawnPoints[i].position, other.transform.position);
                if (dist < 50f)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose) continue;

            GameObject minion = Instantiate(shuffledMinions[i], minionSpawnPoints[i].position, Quaternion.identity);
            activeMinions.Add(minion);

            yield return new WaitForSeconds(4f);
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in activeMinions)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        activeMinions.Clear();
    }


}
