using System;
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
    public Vector2 groundChecksize = new Vector2(0.5f , 0.05f);
    public LayerMask ground;


    public int maxjump = 2;
    private int jumpremaining;
    
    
    
    
    private bool isclimbing;
    public float climbspeed = 5f;
    
    float horizontal;
    
    
    public float dashspeed = 50f;
    public float dashtime = 0.2f;
    public float dashcoldown = 8f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trail;
    
     private Animator anim;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        
        
    }

    public void onmove(InputAction.CallbackContext context)
    {
       horizontalmovement = context.ReadValue<Vector2>().x; 
    }

    public void onjump(InputAction.CallbackContext context)
    {
        if (jumpremaining > 0 )
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                jumpremaining -= 1;
                anim.SetBool("jump" , true);
                // underneath.Play();
                
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpremaining -= 1;
         
                // underneath.Play();
            }
        }
    }



    
    void Update()
    {
       rb.linearVelocity = new Vector2(horizontalmovement * movespeed, rb.linearVelocity.y);
       groundcheck();
       
       anim.SetFloat("speed" , Mathf.Abs(horizontalmovement));
       anim.SetBool("jump" , !isgrounded());
       
       
       if (horizontalmovement > 0 && facingright)
       {
           flip();
       }

       if (horizontalmovement < 0 && !facingright)
       {
           flip();
       }
       
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
        if (Physics2D.OverlapBox(groundCheckpos.position, groundChecksize, 0, ground))
        {
            return true;
        }
        return false;
    }
    
    public void flip()
    {
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
        facingright = !facingright;
    }
    //hurt and attack and die need anim

    
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


   
    
    
    
   
}

   