using System.Threading;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class playermovement1 : MonoBehaviour
{
    public playermovement2 p2;
    public GameObject boxUI;
    public Rigidbody2D rb;
    public Vector3 startpos;
    public Transform orstartpos;
    public float horizontalmovement;
    public float movespeed = 20;
    public bool facingright = true;


    public float jumpforce = 10f;


    private Vector2 movement;


    public Transform groundCheckpos;
    public Vector2 groundChecksize = new Vector2(0.5f, 0.05f);
    public LayerMask ground;
    public bool falling = false;
    public float fally;



    float horizontal;


    public float dashspeed = 200f;
    public float dashtime = 0.2f;
    public float dashcoldown = 10f;
    public bool isDashing;
    public bool canDash = true;
    TrailRenderer trail;
    public GameObject dashicon;

    private bool isclimbing;
    public float climbspeed = 5f;



    private Animator anim;


    public Transform attackpoint;
    public float attackrange = 1f;
    public LayerMask enemylayers;


    public int maxHealth = 3; //10 is too much
    public int currentHealth;

    public int maxlives = 3;

    public int currentlives;
    public Image[] lifeimages;
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1, 1, 1, 0.3f);

    public healthbar bar;

    public int coins = 0;


    public TextMeshProUGUI cointext;

    public bool haskey = false;
    public bool atship = false;
    public GameObject keyicon;

    public bool haskey2;
    public GameObject Keyicon2;

    bool inwater = false;
    private float watertime = 0;
    public GameObject watericon;

    bool cangethurt = false;


    public AudioSource audioSource;

    public AudioClip attackSound;

    public bool atfinaldoor = false;
    public AudioClip deathSound;
    public AudioClip jumpSound;



    IEnumerator gettinghurtagain(float duration)
    {
        cangethurt = true;
        yield return new WaitForSeconds(duration);
        cangethurt = false;
    }

    public void addcoin(int value)
    {
        coins += value;
        cointext.text = coins.ToString("000");
    }

    public void addhealth(int value)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += value;
            bar.Sethealth(currentHealth);
        }
    }

    public void addlives(int value)
    {
        if (currentlives < maxlives)
        {
            currentlives += value;
            updatevives();
        }
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
        jumpforce += value;
        yield return new WaitForSeconds(10);
        jumpforce -= value;
    }

    private void Start()
    {
        startpos = transform.position;
        orstartpos = transform.parent;
        trail = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        p2 = GetComponent<playermovement2>();
        currentHealth = maxHealth;
        currentlives = maxlives;
        bar.Setmaxhealth(maxHealth);
        bar.Sethealth(currentHealth);
        updatevives();

    }

    public void onmove(InputAction.CallbackContext context)
    {
        horizontalmovement = context.ReadValue<Vector2>().x;
    }

    public void onjump(InputAction.CallbackContext context)
    {
        if (isgrounded())
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                anim.SetBool("jump", true);
                if (audioSource != null && jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }

            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }


    public void ondash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine("dashroutine");
        }
    }


    public IEnumerator dashroutine()
{
    Physics2D.IgnoreLayerCollision(6, 8, true);
    canDash = false;
    isDashing = true;
    trail.emitting = true;

    float grav = rb.gravityScale;
    rb.gravityScale = 0f;

    float dashdirection = facingright ? 1f : -1f;
    rb.linearVelocity = new Vector2(dashdirection * dashspeed, 0f);

    float hit = 0f;

    while (hit < dashtime)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, enemylayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemypatrolling patrolling = enemy.GetComponent<Enemypatrolling>();
            if (patrolling != null)
            {
                patrolling.TakeDamage(1);
            }
            EnemyShooting shooting = enemy.GetComponent<EnemyShooting>();
            if (shooting != null)
            {
                shooting.TakeDamage(1);
            }
        }
        hit += Time.deltaTime;
        yield return null;
    }

    rb.linearVelocity = Vector2.zero;
    trail.emitting = false;
    rb.gravityScale = grav;
    isDashing = false;
    Physics2D.IgnoreLayerCollision(6, 8, false);

    yield return new WaitForSeconds(dashcoldown);
    canDash = true;
}


    void Update()
    {
        if (!isgrounded())
        {
            if (!falling)
            {
                fally = transform.position.y;
                falling = true;
            }
        }
        else
        {
            if (falling)
            {
                float falldis = fally - transform.position.y;
                if (falldis > 80f)
                {
                    TakeDamage(1);
                    Debug.Log("demage: " + falldis);
                }
                falling = false;
            }
        }

        if (!isDashing)
            rb.linearVelocity = new Vector2(horizontalmovement * movespeed, rb.linearVelocity.y);
        if (canDash)
        {
            dashicon.SetActive(true);
        }
        if (!canDash)
        {
            dashicon.SetActive(false);
        }
        anim.SetFloat("magnitude", Mathf.Abs(horizontalmovement));
        anim.SetBool("jump", !isgrounded());

        if (horizontalmovement > 0 && !facingright)
        {
            flip();
        }

        if (horizontalmovement < 0 && facingright)
        {
            flip();
        }

        if (isclimbing)
        {
            float vertical = Input.GetAxis("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * movespeed);
        }
        if (inwater)
        {
            watertime += Time.deltaTime;

            if (watertime >= 5f)
            {
                TakeDamage(currentHealth);
                watertime = 0f;
            }
        }
    }





    private bool isgrounded()
    {
        if (Physics2D.OverlapBox(groundCheckpos.position, groundChecksize, 0, ground))
        {
            return true;
        }
        return false;
    }



    public void hurt()
    {
        anim.SetTrigger("hurt");
    }

    public void die()
    {
        anim.SetTrigger("die");
        this.enabled = false;

        // توقف حرکت
        rb.linearVelocity = Vector2.zero;

        // اختیاری: حذف کلاهبرداری یا فیزیک اضافی
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void onattack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack Input Received");

        if (context.performed)
        {
            Debug.Log("Attack Input Performed");
            anim.SetTrigger("attack");

            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayers);

            foreach (Collider2D enemy in hitenemies)
            {
                // بررسی اینکه آیا این دشمن اسکریپت patrolling داره
                Enemypatrolling patrolling = enemy.GetComponent<Enemypatrolling>();
                if (patrolling != null)
                {
                    patrolling.TakeDamage(1);
                    Debug.Log("Hit patrolling enemy");
                }

                // بررسی اینکه آیا این دشمن اسکریپت shooting داره
                EnemyShooting shooting = enemy.GetComponent<EnemyShooting>();
                if (shooting != null)
                {
                    shooting.TakeDamage(1);
                    Debug.Log("Hit shooting enemy");
                }
            }

            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }


    }
    public void OnDrawGizmosSelected()
    {

        if (attackpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }

    public void flip()
    {
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
        facingright = !facingright;
    }
    public bool jj = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("latter"))
        {
            rb.gravityScale = 0f;
            isclimbing = true;
        }
        if (collision.CompareTag("water"))
        {
            watericon.SetActive(true);
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

        if (collision.CompareTag("obstecle"))
        {
            TakeDamage(1);
        }

        if (collision.CompareTag("key2"))
        {
            haskey2 = true;
            Debug.Log("got iy");
            Keyicon2.SetActive(true);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("finaldoor"))
        {
            atfinaldoor = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("movingblock"))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("movingblock"))
        {
            transform.parent = null;
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
            watericon.SetActive(false);
            inwater = false;
            watertime = 0;
        }

    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        bar.Sethealth(currentHealth);
        hurt();

        if (currentHealth <= 0)
        {
            currentlives -= 1;
            updatevives();
            if (currentlives <= 0)
            {
                die();
            }
            else
            {
                  if (audioSource != null && deathSound != null)
                        {
                            audioSource.PlayOneShot(deathSound);
                        }

                anim.SetTrigger("die");
                currentHealth = maxHealth;
                bar.Sethealth(currentHealth);


                Transform originalParent = transform.parent;
                transform.SetParent(null); // موقت جدا کردن از parent
                transform.position = startpos;
                transform.SetParent(originalParent);

            }
        }
        if (cangethurt) return;
        



    }

    private void updatevives()
    {
        for (int i = 0; i < lifeimages.Length; i++)
        {
            if (i < currentlives)
            {
                lifeimages[i].color = activeColor;
            }
            else
            {
                lifeimages[i].color = inactiveColor;
            }
        }
    }
    
    public Transform playerRoot; 
    
}
