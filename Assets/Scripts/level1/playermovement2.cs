using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement2 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float horizontalmovement;
    public float movespeed = 20;
    public bool facingright = true;

    public float jumpforce = 10f;
    private Vector2 movement;

    public Transform groundCheckpos;
    public Vector2 groundChecksize = new Vector2(0.5f, 0.05f);
    public LayerMask ground;

    public int maxjump = 2;
    private int jumpremaining;

    private bool isclimbing;
    public float climbspeed = 5f;

    public float dashspeed = 50f;
    public float dashtime = 0.2f;
    public float dashcoldown = 8f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trail;

    public GameObject bulletref;

    private Animator anim;
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        bulletref = Resources.Load<GameObject>("Bullet");
        currentHealth = maxHealth;
    }

    public void onmove(InputAction.CallbackContext context)
    {
        if (!isDead)
            horizontalmovement = context.ReadValue<Vector2>().x;
    }

    public void onjump(InputAction.CallbackContext context)
    {
        if (isDead) return;

        if (jumpremaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                jumpremaining -= 1;
                anim.SetBool("jump", true);
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpremaining -= 1;
            }
        }
    }

    void Update()
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(horizontalmovement * movespeed, rb.linearVelocity.y);
        groundcheck();

        anim.SetFloat("speed", Mathf.Abs(horizontalmovement));
        anim.SetBool("jump", !isgrounded());

        if (horizontalmovement < 0 && !facingright) flip();
        if (horizontalmovement >  0 && facingright) flip();

        if (isclimbing)
        {
            float vertical = Input.GetAxis("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * movespeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckpos.position, groundChecksize);
    }

    private void groundcheck()
    {
        if (Physics2D.OverlapBox(groundCheckpos.position, groundChecksize, 0, ground))
        {
            jumpremaining = maxjump;
        }
    }

    private bool isgrounded()
    {
        return Physics2D.OverlapBox(groundCheckpos.position, groundChecksize, 0, ground);
    }

    public void flip()
    {
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
        facingright = !facingright;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("latter"))
        {
            rb.gravityScale = 0f;
            isclimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("latter"))
        {
            isclimbing = false;
            rb.gravityScale = 10f;
        }
    }

    public void onattack(InputAction.CallbackContext context)
    {
        if (context.performed && !isDead)
        {
            anim.SetTrigger("attack");
            
            GameObject bullet = Instantiate(bulletref);
            bullet.transform.position = new Vector3(transform.position.x + (facingright ? 1f : -1f), transform.position.y, transform.position.z);
            bullet.transform.localScale = new Vector3(facingright ? 1 : -1, 1, 1); // جهت گلوله
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        hurt();

        if (currentHealth <= 0)
        {
            die();
        }
    }

    private void hurt()
    {
        anim.SetTrigger("hurt");
        // می‌تونی افکت صوتی یا فلش خوردن هم بزاری اینجا
    }

    private void die()
    {
        isDead = true;
        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // غیر فعال کردن فیزیک

        // اگر بخوای UI یا صحنه restart شه، اون رو هم اینجا اضافه کن
    }
}
