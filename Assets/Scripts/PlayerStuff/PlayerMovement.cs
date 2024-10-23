using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    public CapsuleCollider2D _hitbox; //ultra important, do not remove
    public BoxCollider2D _hurtbox;
    
    [Header("Player Movement Variables")]
    [SerializeField] private float playerSpeed = 5f;
    
    [Header("Jump Variables")]
    [SerializeField] private float playerJumpForce = 5f;
    [SerializeField] private float maxJumpTime = 5f;
    [SerializeField] private float jumpTimeLimitOnDoubleJump = 0.8f;
    private int _doubleJumpCount;
    [SerializeField] private int maxDoubleJumpCount = 1;
    [SerializeField] private float minJumpTime = 0.5f;
    
    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private bool invincibleWhileDashing;
    
    [Header("Player Movement Booleans")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool dashIsOnCooldown;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canDash;
    
    [Header("Input Queue Variables")]
    [SerializeField] private float inputQueueDuration = 0.2f;
    [SerializeField] private bool shouldJump;
    [SerializeField] private bool shouldDash;
    
    // Start is called before the first frame update
    void Start()
    {
        //get components
        _rb = GetComponent<Rigidbody2D>();
        _hurtbox = GetComponent<BoxCollider2D>();
        _hitbox = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //left and right movement
        float horizontal = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(horizontal * playerSpeed, _rb.velocity.y);
        
        //jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldJump = true;
            StartCoroutine(InputQueueJumpDash());
        }
        
        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shouldDash = true;
            StartCoroutine(InputQueueJumpDash());
        }
    }
    
    //input queue resolver
    private IEnumerator InputQueueJumpDash()
    {
        //check if player wants to jump
        if (shouldJump)
        {
            float jumpWaitTime = 0;
            
            //for as long as the input queue duration, check if player can jump
            while (jumpWaitTime < inputQueueDuration)
            {
                //if player is grounded and not jumping, jump
                if (isGrounded && !isJumping && _doubleJumpCount == 0)
                {
                    isGrounded = false;
                    StartCoroutine(   Jump(maxJumpTime));
                    break;
                }
                //if player is not grounded and has not double jumped, double jump
                else if (!isGrounded && _doubleJumpCount < maxDoubleJumpCount) 
                {
                    _doubleJumpCount++;
                    StartCoroutine(Jump(maxJumpTime * jumpTimeLimitOnDoubleJump));
                    break;
                }
                jumpWaitTime += Time.deltaTime;
                yield return null;
            }
            shouldJump = false;
        }
        
        //check if player wants to dash
        if (shouldDash)
        {
            float dashWaitTime = 0;
            while (dashWaitTime < inputQueueDuration)
            {
                if (!dashIsOnCooldown && canDash && !isDashing)
                {
                    StartCoroutine(Dash());
                    break;
                }
                dashWaitTime += Time.deltaTime;
                yield return null;
            }
            shouldDash = false;
        }
    }
    
    //jumping
    private IEnumerator Jump(float jumpTimeMax)
    {
        isJumping = true;
        float jumpTime = 0;

        while ((Input.GetKey(KeyCode.Space) && jumpTime < jumpTimeMax ) || jumpTime < minJumpTime)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, playerJumpForce);
            jumpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.3f);
        
       
        isJumping = false;
    }
    
    
    //dashing
    private IEnumerator Dash()
    {
        dashIsOnCooldown = true;
        isDashing = true;
        canDash = false;
        if (invincibleWhileDashing)
        {
            _hurtbox.enabled = false;
        }
        float dashTime = 0;
        while (dashTime < dashDuration)
        {
            _rb.velocity = new Vector2(_rb.velocity.x * dashSpeed, _rb.velocity.y);
            dashTime += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        if (invincibleWhileDashing)
        {
            _hurtbox.enabled = true;
        }
        yield return new WaitForSeconds(dashCooldown);
        dashIsOnCooldown = false;
        canDash = true;
    }
    
    //grounded check
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _doubleJumpCount = 0;
        }
    }
}
