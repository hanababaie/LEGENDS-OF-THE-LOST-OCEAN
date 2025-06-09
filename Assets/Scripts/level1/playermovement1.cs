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
        yield return new WaitForSeconds(duration); // waits for duration and during it we can get hurt
        cangethurt = false;
    }

    public void addcoin(int value)
    {
        coins += value;
        cointext.text = coins.ToString("000"); // it changes the text
    }

    public void addhealth(int value)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += value;
            bar.Sethealth(currentHealth); // for updating the health bar
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
        yield return new WaitForSeconds(10); // for 10  seconds our speed is larger
        movespeed -= value;  //back to original speed
    }

    public void addpower(float value)
    {
        power(value);
    }

    IEnumerator power(float value)
    {
        jumpforce += value;
        yield return new WaitForSeconds(10); // waits for 10 sec 
        jumpforce -= value;
    }

    private void Start()
    {
        startpos = transform.position;
        orstartpos = transform.parent;
        trail = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        p2 = GetComponent<playermovement2>(); // for having the script of the playermovement2
        currentHealth = maxHealth;
        currentlives = maxlives;
        bar.Setmaxhealth(maxHealth); // seting the health bar
        bar.Sethealth(currentHealth);
        updatevives();

    }

    public void onmove(InputAction.CallbackContext context)
    {
        horizontalmovement = context.ReadValue<Vector2>().x; // for moving horizontally
        // reads the value from the keyboard and then as long as its x is important and then update horizontal movement
    }

    public void onjump(InputAction.CallbackContext context) // for jumping
    {
        if (isgrounded()) // checks if we are at ground
        {
            if (context.performed) // if we hit the key
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce); // we give tis to rigidbody and it causes jump, x doesn't change
                anim.SetBool("jump", true);
                if (audioSource != null && jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }

            }
            else if (context.canceled) // if leave the ley it jump less this time(half)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }


    public void ondash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash) // if key is pressed and we can dash
        {
            StartCoroutine("dashroutine"); // start the coroutine
        }
    }


    public IEnumerator dashroutine()
{
    Physics2D.IgnoreLayerCollision(6, 8, true); // it is set so enemies and player doesn't collide
                                                // player is at layer 6 and enemies at 8
                                                // true means we want to ignore the collide
    
    canDash = false;
    isDashing = true;
    trail.emitting = true;

    float grav = rb.gravityScale; // we store the gravity to use later
    rb.gravityScale = 0f; // set gravity to 0 so when it doesn't fall

    float dashdirection = facingright ? 1f : -1f; //set the direction based on the direction of the player

    rb.linearVelocity = new Vector2(dashdirection * dashspeed, 0f); // apply the dash force

    float hit = 0f; // timer for checking how much time we still have

        while (hit < dashtime) //while we have time
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, enemylayers);
            // check for the enemies near the player

            foreach (Collider2D enemy in hitEnemies)
            // set a for each for the enemies
            {
                Enemypatrolling patrolling = enemy.GetComponent<Enemypatrolling>();
                if (patrolling != null)
                {
                    patrolling.TakeDamage(1);
                    // if it does have the script of patrolling take damage
                }
                EnemyShooting shooting = enemy.GetComponent<EnemyShooting>();
                if (shooting != null)
                {
                    shooting.TakeDamage(1);
                    // if it has the shooting script takes damage
                }
            }
            hit += Time.deltaTime;
            //updating the timer
            yield return null;
        // wait for the next frame
    }

    rb.linearVelocity = Vector2.zero; // resetting the velocity to stop the dash
    trail.emitting = false; // turn of the trail
    rb.gravityScale = grav; // reset the gravity to its original
    isDashing = false;
    Physics2D.IgnoreLayerCollision(6, 8, false); // we turn of ignoring the colliders

    yield return new WaitForSeconds(dashcoldown); // waits for the coldown so after that we can dash
    canDash = true;
}


    void Update()
    {
        if (!isgrounded()) //if we are not on the ground
        {
            if (!falling) 
            {
                fally = transform.position.y; // store the y in the beginning
                falling = true;
            }
        }
        else
        {
            if (falling)
            {
                float falldis = fally - transform.position.y; // check the distance we fel
                if (falldis > 100f)  // if it was greater that 100  we get hurt
                {
                    TakeDamage(1);
                    Debug.Log("demage: " + falldis);
                }
                falling = false;
            }
        }

        if (!isDashing) // when we don't dash we upadte the movement
            rb.linearVelocity = new Vector2(horizontalmovement * movespeed, rb.linearVelocity.y);
        if (canDash)
        {
            dashicon.SetActive(true); // apear the dash icon
        }
        if (!canDash)
        {
            dashicon.SetActive(false); // disapear
        }

        anim.SetFloat("magnitude", Mathf.Abs(horizontalmovement)); //idle
        anim.SetBool("jump", !isgrounded()); //jump

        if (horizontalmovement > 0 && !facingright) // check fo the dirction and flip the player
        {
            flip();
        }

        if (horizontalmovement < 0 && facingright)
        {
            flip();
        }

        if (isclimbing) // for climbing the latter
        {
            float vertical = Input.GetAxis("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * movespeed);
        }
        
        if (inwater)
        {
            watertime += Time.deltaTime; // timer

            if (watertime >= 5f) // more than 5 sce in water -> take damage
            {
                TakeDamage(currentHealth);
                watertime = 0f;
            }
        }
    }





    private bool isgrounded() 
    {
        if (Physics2D.OverlapBox(groundCheckpos.position, groundChecksize, 0, ground))
        // check if any block with layer groud has collide with the box
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
            // take the enemies near the player in attack range 

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
                audioSource.PlayOneShot(attackSound); // plays the attack sound
            }
        }


    }
    public void OnDrawGizmosSelected() // used to set the attack range
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
        // get the size of the player
        theScale.x *= -1; // rotate it in x 
        gameObject.transform.localScale = theScale; //aplly the scale
        facingright = !facingright; // change the position
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("latter"))
        {
            rb.gravityScale = 0f; // when on ladder we set the graviy to 0
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
            transform.parent = collision.transform; // so that the player move with the block
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
        bar.Sethealth(currentHealth); //updating the healthbar
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
                bar.Sethealth(currentHealth); //reset the health bar


                Transform originalParent = transform.parent; //store the parent of the player
                transform.SetParent(null); // set free the player from any parent
                transform.position = startpos; // change the positon to the begining
                transform.SetParent(originalParent); // set the parent

            }
        }

        if (cangethurt) return; // for that time to get hurt
        



    }

    private void updatevives()
    {
        for (int i = 0; i < lifeimages.Length; i++)
        {
            if (i < currentlives)
            {
                lifeimages[i].color = activeColor; // turn of the icon
            }
            else
            {
                lifeimages[i].color = inactiveColor; //turn of the icon
            }
        }
    }
    
    public Transform playerRoot; 
    
}
