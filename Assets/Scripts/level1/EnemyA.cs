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
    
    
   //دو خط زیر واسه شناسایی پلیر و حمله است
    public float detectionRange = 5f;
    private Transform targetPlayer;

    void Start()
    {
        centerX = transform.position.x;
        leftX = centerX - patrolDistance;
        rightX = centerX + patrolDistance;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        FindTargetPlayer();

        if (targetPlayer != null && Vector2.Distance(transform.position, targetPlayer.position) <= detectionRange)
        {
            // انیمیشن حمله و حرکت به سمت پلیر
            anim.SetBool("attack", true);
            anim.SetBool("isMoving", false);

            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // چرخش درست به سمت هدف
            if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            {
                Flip();
            }

            return; // متوقف کردن گشت‌زنی
        }
        else
        {
            anim.SetBool("attack", false);
            anim.SetBool("isMoving", true);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
    
        if (other.CompareTag("player1"))
        {
            other.GetComponent<playermovement1>()?.TakeDamage(damageToPlayer);
        }
        else if (other.CompareTag("player2"))
        {
            other.GetComponent<playermovement2>()?.TakeDamage(damageToPlayer);
        }
    }

    
    void FindTargetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player1");
        GameObject player2 = GameObject.FindGameObjectWithTag("player2");

        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p.transform;
            }
        }

        if (player2 != null)
        {
            float dist2 = Vector2.Distance(transform.position, player2.transform.position);
            if (dist2 < minDist)
            {
                closest = player2.transform;
            }
        }

        targetPlayer = closest;
    }

}