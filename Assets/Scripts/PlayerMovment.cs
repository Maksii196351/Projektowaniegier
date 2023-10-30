using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Rider.Unity.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//dodanie braku kolizji podczas dash
//dodanie życia
public class PlayerMovment : MonoBehaviour
{

        private float horizontal;
        
        private float Speed = 8f;
        private float JumpPower = 16f;

        private bool isFacingRight = true;
        private float _groundCheckSize = 0.05f;
       
    [SerializeField]        private Rigidbody2D rb;
    [SerializeField]        private Transform groundCheck;
    [SerializeField]        private LayerMask groundLayer;    
    [SerializeField]        private CapsuleCollider2D CG;
    [SerializeField]        private int hp = 3;
   [Header("dashing")]
    [SerializeField]    private float _dashingVelocity = 24f;
    [SerializeField]    private float _dashingTime = 0.1f;
	private Vector2 _dashingDir;
	private bool _isDashing;
	private bool _canDash = true;
    private Vector2 _moveInput;
  
	private TrailRenderer _trailRenderer;    
    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();        
    }
    void Update()
    {
        
        var inputX = Input.GetAxisRaw("Horizontal");
        var JumpInput  = Input.GetButtonDown("Vertical");
        rb.velocity = new Vector2(inputX * Speed, rb.velocity.y);

        if (JumpInput && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }

        if (inputX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(inputX), 1, 1);
        }
        
     
      
		var dashInput = Input.GetButtonDown("Dash");
		if (dashInput && _canDash)
		{
            CG.enabled = false;
            rb.gravityScale = 0;
			_canDash = false;
			_isDashing = true;
			_trailRenderer.emitting = true;
			
			_dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            if (_dashingDir ==Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }

			StartCoroutine(StopDashing());
			
		}
		
		if (_isDashing)
		{
			
			rb.velocity = _dashingDir * _dashingVelocity;
			return;
		}
		if (isGrounded())
		{
			_canDash = true;
		}
		
		                       

    }

    private IEnumerator StopDashing()
    {
		yield return new WaitForSeconds(_dashingTime);
		rb.gravityScale = 4f;
        CG.enabled = true;
		_trailRenderer.emitting = false;
		_isDashing = false;		
	}
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, _groundCheckSize, groundLayer);
    }    

}

   