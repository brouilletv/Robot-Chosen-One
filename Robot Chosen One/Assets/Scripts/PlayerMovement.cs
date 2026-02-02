using System;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer robotSprite;
    private float facingDirection;
    private float previousFacingDirection;
    private static PlayerInput playerInput;


    // Input Variables
    private float moveDirection;
    private bool jumpPressed;
    private bool jumpReleased;

    private Vector2 knockback;

    [Header("Movement Variables")]
    [SerializeField] float groundSpeed = 10f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float normalGravity = 6f;
    [SerializeField] float fallGravity = 12f;
    [SerializeField] float jumpGravity = 4f;

    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;


    [Header("Checks")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;
    private bool isGrounded;

    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool canDash = true;

    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    private void Awake()
    {
        rb.gravityScale = normalGravity;
        playerInput = GetComponent<PlayerInput>();
    }


    private void Update()
    {
        Flip();
    }


    private void FixedUpdate()
    {
        setFacingDirection();
        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
        knockback = new Vector2(0, 0);
    }


    // Switching action maps doesn't work for some reason and I gave up on trying to fix it. I'm leaving the code here just in case
    public void SwitchActionMapToUI()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Enable();
        print("switched to UI");
    }


    // Switching action maps doesn't work for some reason and I gave up on trying to fix it. I'm leaving the code here just in case
    public void SwitchActionMapToPlayer()
    {
        playerInput.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();
        print("switched to Player");
    }


    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rb.velocity = new Vector2(previousFacingDirection * dashingPower, 0f);
        rb.gravityScale = 0f;

        //tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;

        //tr.emitting = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    public void OnDash(InputValue value)
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void HandleMovement()
    {
        if (!isDashing)
        { 
            float speed = facingDirection * groundSpeed;
            rb.velocity = new Vector2(speed, rb.velocity.y) + knockback;
        }
    }


    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased && !isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }


    void ApplyVariableGravity()
    {
        if (!isDashing)
        {
            if (rb.velocity.y < -0.05f)
            {
                rb.gravityScale = fallGravity;
            }
            else if (rb.velocity.y > 0.05f)
            {
                rb.gravityScale = jumpGravity;
            }
            else
            {
                rb.gravityScale = normalGravity;
            }
        }
    }


    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    private void setFacingDirection()
    {
        float value = moveDirection;

        if (value > 0)
        {
            facingDirection = 1;

        }
        else if (value < 0)
        {
            facingDirection = -1;
        }
        else if (value == 0)
        {
            facingDirection = 0;
        }

        if (facingDirection != 0)
        {
            previousFacingDirection = facingDirection;
        }
    }


    private void Flip()
    {
        if (facingDirection == 1)
        {
            robotSprite.flipX = false;
        }

        else if (facingDirection == -1)
        {
            robotSprite.flipX = true;
        }
    }


    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>().x;
    }


    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded)
            {
                jumpPressed = true;
                jumpReleased = false;
            }
        }
        else
        {
            jumpReleased = true;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


    void OnEnable()
    {
        TouchDmg.HitBounce += HandleBouceDirection;
    }


    void OnDisable()
    {
        TouchDmg.HitBounce -= HandleBouceDirection;
    }


    void HandleBouceDirection(int direction)
    {
        if (direction == 0)
        {
            knockback = new Vector2(-50, 15);

        }
        else if (direction == 1)
        {
            knockback = new Vector2(50, 15);
        }
    }
}