using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public int maxHealth = 3;
    private int currentHealth;

    private Transform player1;
    private Transform player2;
    private Transform targetPlayer;

    private Animator anim;
    private Rigidbody2D rb;

    private bool isDead = false;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject p2 = GameObject.FindGameObjectWithTag("Player2");

        if (p1 != null && p2 != null)
        {
            player1 = p1.transform;
            player2 = p2.transform;
        }
    }

    void Update()
    {
        if (isDead || isAttacking) return;

        FindClosestPlayer();

        if (targetPlayer != null)
        {
            float distance = Vector2.Distance(transform.position, targetPlayer.position);

            if (distance <= attackRange)
            {
                Attack();
            }
            else if (distance <= detectionRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                Idle();
            }
        }
    }

    void FindClosestPlayer()
    {
        float distanceToP1 = Vector2.Distance(transform.position, player1.position);
        float distanceToP2 = Vector2.Distance(transform.position, player2.position);

        targetPlayer = distanceToP1 < distanceToP2 ? player1 : player2;
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (targetPlayer.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

        anim.SetBool("isMoving", true);

        // Flip
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Idle()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("isMoving", false);
    }

    void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("attack");
        rb.velocity = Vector2.zero;
    }

    // این تابع با Animation Event صدا زده میشه
    public void EndAttack()
    {
        isAttacking = false;
        anim.ResetTrigger("attack");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        anim.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("die");
        rb.velocity = Vector2.zero;

        // تاخیر برای حذف
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
