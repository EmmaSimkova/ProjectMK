using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D Hitbox;
    private CapsuleCollider2D Hurtbox;
    
    [Header("Player Movement Variables")]
    [SerializeField] private float playerSpeed = 5f;
    
    [Header("Jump Variables")]
    [SerializeField] private float playerJumpForce = 5f;
    [SerializeField] private float maxJumpTime = 5f;
    [SerializeField] private float jumpTimeLimitOnDoubleJump = 0.8f;
    private int doubleJumpCount = 0;
    [SerializeField] private int maxDoubleJumpCount = 1;
    
    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private bool invincibleWhileDashing = false;
    
    [Header("Player Movement Booleans")]
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool dashIsOnCooldown = false;
    private bool isDashing = false;
    private bool canDash = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //get components
        rb = GetComponent<Rigidbody2D>();
        Hurtbox = GetComponent<CapsuleCollider2D>();
        Hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //left and right movement
        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        
        //jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                isGrounded = false;
                isJumping = true;
                StartCoroutine(Jump(maxJumpTime));
            }else if (!isGrounded && doubleJumpCount < maxDoubleJumpCount)
            {
                doubleJumpCount++;
                isJumping = true;
                StartCoroutine(Jump(maxJumpTime * jumpTimeLimitOnDoubleJump));
            }
        }
        
        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashIsOnCooldown && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    //jumping
    private IEnumerator Jump(float jumpTimeMax)
    {
        float jumpTime = 0;
        while (jumpTime < jumpTimeMax)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
                jumpTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                isJumping = false;
                //reduce velocity if player lets go of jump key
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.2f);
                break;
            }
        }
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
            Hurtbox.enabled = false;
        }
        float dashTime = 0;
        while (dashTime < dashDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x * 2, rb.velocity.y);
            dashTime += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        if (invincibleWhileDashing)
        {
            Hurtbox.enabled = true;
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
            doubleJumpCount = 0;
        }
    }
}
