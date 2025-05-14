using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Vector2 groundChecksize = new Vector2(0.5f , 0.05f);
    public LayerMask ground;
    
    
    
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
        if (isgrounded())
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                anim.SetBool("jump" , true);
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
       
       anim.SetFloat("magnitude" , Mathf.Abs(horizontalmovement));
       anim.SetBool("jump" , !isgrounded());

       if (horizontalmovement > 0 && facingright)
       {
           flip();
       }

       if (horizontalmovement < 0 && facingright)
       {
           flip();
       }

       
    }

    

    private void OnDrawGizmosSelected()
    {
       Gizmos.color = Color.green;
       Gizmos.DrawWireCube(groundCheckpos.position, groundChecksize);
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
    }
    
    public void onattack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger("attack");
        }
    }

    public void flip()
    {
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
        facingright = !facingright;
    }




}
