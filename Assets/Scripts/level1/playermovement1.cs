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
    public Rigidbody2D rb;
    public float horizontalmovement;
    public float movespeed = 20;
    public bool facingright = true;


    public float jumpforce = 10f;


    private Vector2 movement;


    public Transform groundCheckpos;
    public Vector2 groundChecksize = new Vector2(0.5f, 0.05f);
    public LayerMask ground;



    float horizontal;


    public float dashspeed = 50f;
    public float dashtime = 0.2f;
    public float dashcoldown = 8f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trail;

    private bool isclimbing;
    public float climbspeed = 5f;



    private Animator anim;


    public Transform attackpoint;
    public float attackrange = 1f;
    public LayerMask enemylayers;


    public int maxHealth = 4; //10 is too much
    private int currentHealth;

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

    bool inwater = false;
    private float watertime = 0;

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
        updatevives();
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
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    // public IEnumerable<WaitForSeconds> onDash(InputAction.CallbackContext context)
    // {
    //     if (context.performed && canDash)
    //     {
    //         StartCoroutine(dashcourotine());
    //     } 
    //     trail.emitting = true;
    //     float dashdirction = 1; 
    //     rb.linearVelocity = new Vector2(dashdirction * dashspeed, rb.linearVelocity.y);
    //     yield return new WaitForSeconds(dashtime);
    //     rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    //     isDashing = false;
    //     trail.emitting = false;
    //     
    //     yield return new WaitForSeconds(dashcoldown);
    //     canDash = true;
    // }
    //
    // private IEnumerator dashcourotine()
    // {
    //     canDash = false;
    //     isDashing = true;
    // }


    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalmovement * movespeed, rb.linearVelocity.y);

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

            if (watertime >= 5)
            {
                TakeDamage(1);
                watertime = 0;
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
                enemy.GetComponent<EnemyController>()?.TakeDamage(1);
                Debug.Log("we hit enemy");
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
                currentHealth = maxHealth;
                bar.Sethealth(currentHealth);
            }
        }

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
 

}