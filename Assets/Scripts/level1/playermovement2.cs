using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Transform FirePoint;

    private Animator anim;
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    public healthbar bar;

    public int maxlives = 3;
    public int currentlives;

    public Image[] lifeimages;
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1, 1, 1, 0.3f);
    
    public int coins = 0;
    
    
    public TextMeshProUGUI cointext;

    private bool inwater;
    private float watertime;

    public bool haskey = false;
    public bool atship = false;
    public GameObject keyicon;


    public void addcoin(int value)
    {
        coins += value;
        cointext.text = coins.ToString("000");
    }

    public void addhealth(int value)
    {
        currentHealth += value;
        bar.Sethealth(currentHealth);
    }

    public void addlives(int value)
    {
        currentlives += value;
        updatelives();
    }

    public void addspeed(float value)
    {
        speed(value);
    }

    IEnumerator speed(float value)
    {
        movespeed += value;
        yield return new WaitForSeconds(10);
        movespeed -= value;
    }

    public void addpower(float value)
    {
      power(value); 
    }

    IEnumerator power(float value)
    {
        movespeed += value;
        yield return new WaitForSeconds(10);
        movespeed -= value;
    }
    


    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        
        bulletref = Resources.Load<GameObject>("Bullet");
        currentHealth = maxHealth;
        bar.Setmaxhealth(maxHealth);
        bar.Sethealth(currentHealth);
        currentlives = maxlives;
        updatelives();
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
        if (collision.CompareTag("water"))
        {
            inwater = true;
            watertime = 0;
        }
        if (collision.CompareTag("key"))
            {
                     haskey = true;
                     keyicon.SetActive(true);
                     Destroy(collision.gameObject);
            }

        if (collision.CompareTag("ship"))
        {
            atship = true;
            sencemanager.Instance.ready();  
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("latter"))
        {
            isclimbing = false;
            rb.gravityScale = 10f;
        }

        if (collision.CompareTag("water"))
        {
            inwater = false;
            watertime = 0;
        }

        
    }

    public void onattack(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        anim.SetTrigger("attack");

        // Instantiate bullet at fire point
        GameObject bullet = Instantiate(bulletref, FirePoint.position, Quaternion.identity);
        
        // Set direction based on player scale
        float direction = transform.localScale.x >= 0 ? 1f : -1f;

        // Set direction value in bullet script
        bullet.GetComponent<bullet>().direction = direction;

        // Flip bullet sprite if needed
        Vector3 bulletScale = bullet.transform.localScale;
        bulletScale.x = Mathf.Abs(bulletScale.x) * direction;
        bullet.transform.localScale = bulletScale;

        // Prevent collision with player
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}


    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        bar.Sethealth(currentHealth);
        hurt();

        if (currentHealth <= 0)
        {
            currentlives--;
            updatelives();
            if (currentlives <= 0)
            {
                die();
            }
            else
            {
                currentHealth = maxHealth;
                bar.Sethealth(currentHealth);
            }
        }
    }
    private void updatelives()
{
    for (int i = 0; i < lifeimages.Length; i++)
    {
        if (i < currentlives)
        {
            lifeimages[i].color = activeColor; // جون داره، رنگ روشن
        }
        else
        {
            lifeimages[i].color = inactiveColor; // جون از دست رفته، رنگ کمرنگ
        }
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
