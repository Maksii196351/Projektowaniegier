using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//dodanie braku kolizji podczas dash
//dodanie życia
public class PlayerMovment : MonoBehaviour
{

        private float horizontal;
        
        private float Speed = 260f;
        private float JumpPower = 16f;
        private float DashingPower = 32f;
        private float DashingTime = 0.1f;
        private float DashingCD = 5f;
        private bool CanDash = true;
        private bool isDashing;
        private bool isFacingRight = true;
       
    [SerializeField]        private Rigidbody2D rb;
    [SerializeField]        private Transform groundCheck;
    [SerializeField]        private LayerMask groundLayer;
    [SerializeField]        private TrailRenderer tr;
    [SerializeField]        private PolygonCollider2D tg;
    [SerializeField]        private int hp = 3;
   
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);            
        }
        if (Input.GetButtonDown("Dash") && CanDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {       
        rb.velocity = new Vector2(horizontal * Speed * Time.deltaTime, rb.velocity.y);
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void Flip()
    {
        if (isFacingRight && horizontal <0f || !isFacingRight && horizontal > 0f)
        {
         isFacingRight = !isFacingRight;
         Vector3 localScale = transform.localScale;
         localScale.x *= -1f;
         transform.localScale = localScale;
        }
    }
    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        float orginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * DashingPower, Input.GetAxisRaw("Vertical") * DashingPower);
        tr.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        tr.emitting = false;
        rb.gravityScale = orginalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashingCD);
        CanDash = true;
        
    }
    private void shoot()
    {

    }
}    

 
