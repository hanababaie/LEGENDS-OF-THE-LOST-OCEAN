using UnityEngine;

public class EnemyA : EnemyController
{
    public float moveSpeed = 2f;
    public int damageToPlayer = 1;
    public float patrolDistance = 3f; // نصف مسیری که می‌خواهی دشمن طی کند

    private float centerX;
    private float leftX;
    private float rightX;
    private bool movingRight = true;
    private Animator anim;

    void Start()
    {
        centerX = transform.position.x;
        leftX = centerX - patrolDistance;
        rightX = centerX + patrolDistance;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("isMoving", true);

        Vector3 pos = transform.position;

        if (movingRight)
        {
            pos.x += moveSpeed * Time.deltaTime;
            if (pos.x >= rightX)
            {
                pos.x = rightX;
                movingRight = false;
                Flip();
            }
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;
            if (pos.x <= leftX)
            {
                pos.x = leftX;
                movingRight = true;
                Flip();
            }
        }

        transform.position = pos;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player1"))
        {
            collision.GetComponent<playermovement1>()?.TakeDamage(damageToPlayer);
        }
        else if (collision.CompareTag("player2"))
        {
            collision.GetComponent<playermovement2>()?.TakeDamage(damageToPlayer);
        }
    }
}