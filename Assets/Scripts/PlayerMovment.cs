using System.Collections;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{   
    [Header("References")]
    [SerializeField]    private LayerMask groundLayer;    
    [SerializeField]    private CapsuleCollider2D Collider; 
    [SerializeField]    private Transform groundCheck;
    [SerializeField]    private Rigidbody2D rb;
    [Header("Dash")]
    [SerializeField]    private float _dashingVelocity = 24f;
    [SerializeField]    private float _dashingTime = 0.1f;
    [Header("WallJump/slide")]
    [SerializeField]    private Vector2 _wallJumpPower = new Vector2(8f,16f);
    [SerializeField]    private LayerMask wallslidelayer;
    [SerializeField]    private Transform _wallCheck;
  
    //wallJump
    private float _wallJumpingDir; 
    private float _wallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private float _walljumpingDur = 0.4f;
    private bool _isWallJumping;
    //dash
    private Vector2 _dashingDir;
	private bool _isDashing;
	private bool _canDash = true;
    //wall slide
    private float _SlideSpeed = 2f;
  	private bool _isWallSliding;
    //movment 
    private float Speed = 8f;
    private float JumpPower = 16f;       
    private float horizontal;
    private Vector2 _moveInput;
    private Vector2 _lastInputDirection;
    //other 
    private float _groundCheckSize = 0.05f;    
    private TrailRenderer _trailRenderer;
    
    void Start()
    {
            _trailRenderer = GetComponent<TrailRenderer>();
            rb = GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        Move();
        isWallSlidding();        
        WallJump();
     
      
		var dashInput = Input.GetButtonDown("Dash");
		if (dashInput && _canDash)
		    {
            //CG.enabled = false;
            //rb.gravityScale = 0;
			_canDash = false;
			_isDashing = true;
			_trailRenderer.emitting = true;
			_dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
			    
                if (_dashingDir ==Vector2.zero)
                {
                    _dashingDir = new Vector2(_lastInputDirection.x, 0);
                }

			StartCoroutine(StopDashing());
			
		    }
		
		if (_isDashing)
		{
			
			rb.velocity = _lastInputDirection * _dashingVelocity;
			return;
		}
		if (isGrounded())
		{
			_canDash = true;
		}
		
		

    }

    private void Move()
    {
        if(!isWalled())
        {
            _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
        }
        
        rb.velocity = new Vector2(_moveInput.x * Speed, rb.velocity.y);

        if (_moveInput.y !=0 && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
        if (Input.GetButtonUp("Vertical") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (_moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(_moveInput.x), 1, 1);
        }
    }

    private IEnumerator StopDashing()
    {
		yield return new WaitForSeconds(_dashingTime);
		//rb.gravityScale = 4f;
        //CG.enabled = true;
		_trailRenderer.emitting = false;
		_isDashing = false;		
	}

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, _groundCheckSize, groundLayer);
    }    

    private void LateUpdate() //to jest metoda która wykonuje się trochę póżniej po Update()
    {
    if(_moveInput != Vector2.zero)
    {
        _lastInputDirection = _moveInput; //ustawiasz lastInputDirection na moveInput tylko wtedy kiedy nie równa się 0
    }
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(_wallCheck.position,0.2f, wallslidelayer);
    }

    private void isWallSlidding()
    {
        if (isWalled() && !isGrounded() && horizontal != 1f)
        {
            _isWallSliding = true;
            
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -_SlideSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void WallJump()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDir = -_lastInputDirection.x;
            _wallJumpingCounter = _wallJumpingTime; 
            CancelInvoke(nameof(StopWallJumping));
        }

        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Vertical") && _wallJumpingCounter > 0f)
        {
            _isWallJumping = true;
            rb.velocity = new Vector2(_wallJumpingDir * _wallJumpPower.x, _wallJumpPower.y);
            _wallJumpingCounter = 0f;
            if (_lastInputDirection.x != _wallJumpingDir) 
            {
                transform.localScale = new Vector3(Mathf.Sign(-_moveInput.x), 1, 1);
            }
            Invoke(nameof(StopWallJumping), _walljumpingDur);
        }   
    }
    private void StopWallJumping()
    {
       _isWallJumping = false;
    }

}

   