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
    public float attackCooldown = 2f;
    private float attackTimer;

    private bool isDead = false;
    private GameObject currentTarget;

    private float targetSwitchTimer;
    private float targetSwitchTime;

    public Transform[] minionSpawnPoints;

    // اینجا آرایه مینیون‌ها
    public GameObject[] minionPrefabs;

    public float minionSpawnCooldown = 10f;
    private float minionSpawnTimer;

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

        if (distance > 1.5f)
        {
            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            if (animator != null)
                animator.SetBool("isMoving", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isMoving", false);
        }
    }

    void AttackTarget()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0;
            if (animator != null)
                animator.SetTrigger("attack");

            var playerScript = currentTarget.GetComponent<playermovement2>();
            if (playerScript != null)
                playerScript.TakeDamage(1);

            var playerScript1 = currentTarget.GetComponent<playermovement1>();
            if (playerScript1 != null)
                playerScript1.TakeDamage(1);
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
        if(minionPrefabs.Length == 0 || minionSpawnPoints.Length == 0) return;

        // تعداد مینیون ها و نقاط باید برابر باشه
        int count = Mathf.Min(minionPrefabs.Length, minionSpawnPoints.Length);

        // آرایه مینیون‌ها رو کپی و مخلوط کن (Shuffle)
        List<GameObject> shuffledMinions = new List<GameObject>(minionPrefabs);
        for (int i = 0; i < shuffledMinions.Count; i++)
        {
            int rnd = Random.Range(i, shuffledMinions.Count);
            var temp = shuffledMinions[rnd];
            shuffledMinions[rnd] = shuffledMinions[i];
            shuffledMinions[i] = temp;
        }

        // اسپاون مینیون‌ها در نقاط به ترتیب
        for (int i = 0; i < count; i++)
        {
            Instantiate(shuffledMinions[i], minionSpawnPoints[i].position, Quaternion.identity);
        }
    }
}
