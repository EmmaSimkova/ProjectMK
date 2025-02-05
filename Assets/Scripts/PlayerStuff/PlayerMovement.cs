using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //current list of features:
    //player movement
    //player jumping
    //player dashing
    //player input queue
    //player grounded check
    //player pickaxe rotation
    //player health
    //player invincibility while dashing
    //Audio (Emma)
    
    [SerializeField] private PickaxeHitRotate pickaxeHitRotate;
    [SerializeField] private bool isPlayerRotated;
    [SerializeField] private TouchGrass playerHealth;
    private Rigidbody2D _rb;
    public CapsuleCollider2D _hitbox; //ultra important, do not remove
    public BoxCollider2D _hurtbox;
    [SerializeField] private Transform jozoRazcast; 
    [SerializeField] private float raycastDistance = 0.1f;
    
    [Header("Player Movement Variables")]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float maxPlayerFallSpeed = 40f;
    [SerializeField] public bool canMove = true;
    
    [Header("Jump Variables")]
    [SerializeField] private float playerJumpForce = 5f;
    [SerializeField] private float maxJumpTime = 5f;
    [SerializeField] private float jumpTimeLimitOnDoubleJump = 0.8f;
    private int _doubleJumpCount;
    [SerializeField] private int maxDoubleJumpCount = 1;
    [SerializeField] private float minJumpTime = 0.5f;
    [SerializeField] public bool canWallJump;
    [SerializeField] public GameObject otherWall;
    
    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 5f;
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
    
    
    AudioManagerScript audioManagerScript;

    private void Awake()
    {
        audioManagerScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //get components
        _rb = GetComponent<Rigidbody2D>();
        _hurtbox = GetComponent<BoxCollider2D>();
        _hitbox = GetComponent<CapsuleCollider2D>();
        
        InvokeRepeating(nameof(GroundedCheck), 0.1f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        //left and right movement
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(_rb.velocity.x) >= 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = horizontal < 0;
        }

        if (canMove)
        {
            _rb.velocity = new Vector2(horizontal * playerSpeed, _rb.velocity.y);
        }
        
        if (Input.GetAxis("Horizontal") == 0 && isGrounded)
        {
            //quick lerp stop
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, Time.deltaTime * 5), _rb.velocity.y);
        }
        
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
        
        //limit the fall speed
        if (_rb.velocity.y < -maxPlayerFallSpeed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -maxPlayerFallSpeed);
        }
        
        //rotate the pickaxe
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //brute force player damagable after pickaxe hit
            StartCoroutine(ForceDamagable());
            //TODO: fix the pickaxe rotation to save the last rotation
            if(pickaxeHitRotate.isPickaxeHitting){return;}
            //resolve the pickaxe rotation based on player direction and W and S keys
            if (Input.GetKey(KeyCode.W))
            {
                pickaxeHitRotate.PickaxeHit(90);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if(isGrounded){return;}
                pickaxeHitRotate.PickaxeHit(-90);
            }
            else if(GetComponent<SpriteRenderer>().flipX)
            {
                pickaxeHitRotate.PickaxeHit(180);
            }
            else
            {
                pickaxeHitRotate.PickaxeHit(0);
            }
        }
    }
    
    public IEnumerator ForceDamagable()
    {
        yield return new WaitForSeconds(1.6f);
        if (!playerHealth.canTakeDamage)
        {
            Debug.Log("FORCED: Player can take damage");
            playerHealth.canTakeDamage = true;
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
                if (canWallJump)
                {
                    StartCoroutine(nameof(WallJump));
                    break;
                }
                //if player is grounded and not jumping, jump
                if (isGrounded && !isJumping && _doubleJumpCount == 0 && !canWallJump)
                {
                    isGrounded = false;
                    StartCoroutine(   Jump(maxJumpTime));
                    break;
                }
                //if player is not grounded and has not double jumped, double jump
                else if (!isGrounded && _doubleJumpCount < maxDoubleJumpCount && !canWallJump) 
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
    
    public IEnumerator WallJump()
    {
        isJumping = true;
        canMove = false;
        _rb.velocity = new Vector2((otherWall.transform.position.x > this.transform.position.x? -15 : 15), playerJumpForce);
        yield return new WaitForSeconds(0.4f);
        canMove = true;
        isJumping = false;
    }
    
    public void WallJumpCheck(GameObject wall)
    {
        otherWall = wall;
    }
    
    //jumping
    private IEnumerator Jump(float jumpTimeMax)
    {
        isJumping = true;
        float jumpTime = 0;
        audioManagerScript.PlaySFX(audioManagerScript.jump);

        while ((Input.GetKey(KeyCode.Space) && jumpTime < jumpTimeMax ) || jumpTime < minJumpTime)
        {
            _rb.gravityScale = 3f;
            _rb.velocity = new Vector2(_rb.velocity.x, playerJumpForce);
            jumpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        //lerp down velocity while going up once jump is done
        while (_rb.velocity.y > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x*0.8f, Mathf.Lerp(_rb.velocity.y, 0, Time.deltaTime));
            yield return null;
        }
        
        //once the player starts going down increase gravity
        _rb.gravityScale = 2.1f;
       
        isJumping = false;
    }
    
    
    //dashing
    private IEnumerator Dash()
    {
        dashIsOnCooldown = true;
        isDashing = true;
        canDash = false;
        //move player a minor amount up
        _rb.velocity = new Vector2(_rb.velocity.x, 0.003f);
        if (invincibleWhileDashing)
        {
            playerHealth.canTakeDamage = false;
            _hurtbox.enabled = false;
        }
        float dashTime = 0;
        while (dashTime < dashDuration)
        {
            _rb.velocity = new Vector2(_rb.velocity.x * dashSpeed, _rb.velocity.y*0.01f);
            dashTime += Time.deltaTime;
            yield return null;
            if (dashTime > dashDuration*0.8f)
            {
                isDashing = false;
            }
        }
        if (invincibleWhileDashing)
        {
            playerHealth.canTakeDamage = true;
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
            _rb.gravityScale = 1f;
            _doubleJumpCount = 0;
        }
        GroundedCheck();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        GroundedCheck();
        //happily do nothing
    }

    //grounded check
    private void GroundedCheck()
    {
        //cast a raycast to check if the player is grounded
        Collider2D hit = Physics2D.OverlapCircle(jozoRazcast.position, 0.1f, LayerMask.GetMask("Ground"));
        //if the raycast hits the ground, the player is grounded
        if (hit != null)
        {
            isGrounded = true;
        }
        else
        {
            StartCoroutine(GroundedCheckDelay());
        }
    }
    
    //if the ground is not detected, remove the grounded status after a short delay
    private IEnumerator GroundedCheckDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isGrounded = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(jozoRazcast.position, raycastDistance);
    }
}
