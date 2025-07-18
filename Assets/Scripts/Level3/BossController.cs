using UnityEngine;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    public float maxHealth = 500;
    private float currentHealth;

    public BossHealthUI healthUI;
    private Animator animator;
    private AudioSource audioSource;

    public float moveSpeed = 3f;
    public float attackCooldown = 0.5f;
    private float attackTimer;

    private bool isDead = false;
    private GameObject currentTarget;

    private float targetSwitchTimer;
    private float targetSwitchTime;

    public Transform[] minionSpawnPoints;
    public GameObject[] minionPrefabs;

    public float minionSpawnCooldown = 10f;
    private float minionSpawnTimer;

    public int damage = 5; // اضافه کردم

    private bool isAttacking = false;

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
            SpawnMinions();
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

        currentTarget = players[Random.Range(0, players.Length)];
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

        Destroy(gameObject, 2f);
    }

    void SpawnMinions()
    {
        if (minionPrefabs.Length == 0 || minionSpawnPoints.Length == 0) return;

        int count = Mathf.Min(minionPrefabs.Length, minionSpawnPoints.Length);

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
            Instantiate(shuffledMinions[i], minionSpawnPoints[i].position, Quaternion.identity);
        }
    }
}
