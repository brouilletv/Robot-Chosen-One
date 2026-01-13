using System;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer robotSprite;
    private float facingDirection;

    // Input Variables
    private float moveDirection;
    private bool jumpPressed;
    private bool jumpReleased;

    private Vector2 knockback;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    [Header("Mouvement Variables")]
    [SerializeField] float groundSpeed = 10f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float normalGravity = 6f;
    [SerializeField] float fallGravity = 12f;
    [SerializeField] float jumpGravity = 5f;

    [Header("GroundCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;
    private bool isGrounded;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    private bool _isDashing = false;
    private float viewDirection;
    private bool _isDashOn = true;

    private void Start()
    {
        rb.gravityScale = normalGravity;
    }


    private void Update()
    {
        
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isDashOn = !_isDashOn;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && _isDashOn)
        {
            StartCoroutine(Dash());
        }
        Flip();
        WallSlide();
    }


    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        ApplyVariableGravity();
        CheckGrounded();
        HandleMouvement();
        HandleJump();
        knockback = new Vector2(0, 0);
    }

    
    private void HandleMouvement()
    {
        float speed = moveDirection * groundSpeed;
        rb.velocity = new Vector2(speed, rb.velocity.y)+ knockback;
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased && isGrounded)
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


    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    private void Flip()
    {
        facingDirection = Input.GetAxisRaw("Horizontal");

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
            jumpPressed = true;
            jumpReleased = false;
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

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (sr.flipX == false)
        {
            viewDirection = 1f;
        }
        else
        {
            viewDirection = -1f;
        }
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * viewDirection, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
}