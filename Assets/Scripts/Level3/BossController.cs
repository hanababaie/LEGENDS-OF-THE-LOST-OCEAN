using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        targetSwitchTime = Random.Range(5f, 10f);
    }

    void SwitchTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        if (players.Length == 1)
        {
            currentTarget = players[0];
            return;
        }

        GameObject newTarget = currentTarget;
        int attempts = 0;
        while (newTarget == currentTarget && attempts < 10)
        {
            newTarget = players[Random.Range(0, players.Length)];
            attempts++;
        }

        currentTarget = newTarget;

        // ✅ بعد از سوییچ هدف، Boss باید از حالت attack خارج بشه و شروع به حرکت کنه
        isAttacking = false;

        if (animator != null)
        {
            animator.ResetTrigger("attack");     // اطمینان از ریست حمله
            animator.SetBool("isMoving", true);  // شروع حرکت
        }

        // (اختیاری) برای دیباگ
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
                if (dist < 30f)
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
