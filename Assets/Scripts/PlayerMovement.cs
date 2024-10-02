using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    
    [Header("Player Movement Variables")]
    private float playerSpeed = 5f;
    private float playerJumpForce = 5f;
    
    [Header("Player Movement Booleans")]
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool dashIsOnCooldown = false;
    private bool isDashing = false;
    
    [Header("Player Movement Cooldowns and Durations")]
    private float dashCooldown = 5f;
    private float dashDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
