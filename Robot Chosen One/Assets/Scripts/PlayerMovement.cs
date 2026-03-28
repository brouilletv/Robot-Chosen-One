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
    #region Variables & Awake
    // References
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer robotSprite;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private PlayerInput playerInput;

    // Movement Variables
    [Header("Movement Variables")]
    [SerializeField] float groundSpeed = 10f;
    [SerializeField] float jumpForce = 16f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float normalGravity = 6f;
    [SerializeField] float fallGravity = 10f;
    [SerializeField] float jumpGravity = 4f;

    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;

    // Ground Check
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    // Flags
    [Header("Flags")]
    public bool unlockedDoubleJump;
    public bool unlockedDash;

    // Input Variables
    public float moveDirectionX;
    public float moveDirectionY;
    private int facingDirection;
    private float previousFacingDirection;
    private Vector2 knockback;

    // Input Booleans
    private bool jumpPressed;
    private bool jumpReleased;

    // Movement Booleans
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;
    private bool canDoubleJump = true;

    // Wall Slide Variables
    private bool isWallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;

    //Wall Jump Variables
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    private void Awake()
    {
        rb.gravityScale = normalGravity;
        playerInput = GetComponent<PlayerInput>();
    }
    #endregion


    #region Update & FixedUpdate
    private void Update()
    {
        HandleFlip();
    }


    private void FixedUpdate()
    {
        setFacingDirection();
        HandleGravity();
        GroundedCheck();
        HandleMovement();
        HandleJump();
        HandleWallslide();
        knockback = new Vector2(0, 0);
    }
    #endregion


    #region Input Methods
    public void OnMove(InputValue value)
    {
        moveDirectionX = value.Get<Vector2>().x;
        moveDirectionY = value.Get<Vector2>().y;
    }


    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded)
            {
                jumpPressed = true;
                jumpReleased = false;
                canDoubleJump = true;

                if (unlockedDoubleJump)
                {
                    canDoubleJump = true;
                }
            }
            else if (!isGrounded && unlockedDoubleJump)
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


    public void OnDash(InputValue value)
    {
        if (canDash && unlockedDash)
        {
            StartCoroutine(HandleDash());
        }
    }
    #endregion


    #region "Handle" Methods
    private void HandleFlip()
    {
        if (facingDirection == 1)
        {
            robotSprite.flipX = false;
            wallCheck.localPosition = new Vector2(0.5f, wallCheck.localPosition.y);
        }

        else if (facingDirection == -1)
        {
            robotSprite.flipX = true;
            wallCheck.localPosition = new Vector2(-0.5f, wallCheck.localPosition.y);
        }
    }


    void HandleGravity()
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
        if (jumpPressed)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = false;
                jumpReleased = false;
            }
            else if (!isGrounded && canDoubleJump && unlockedDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = false;
                jumpReleased = false;
                canDoubleJump = false;
            }
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


    private void HandleWallslide()
    {
        if (IsWalled() && !isGrounded && moveDirectionX != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }


    private IEnumerator HandleDash()
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
    #endregion


    #region Checks & SetVariables
    private void GroundedCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    private void WallJump()
    
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingCounter;
        }
        else
        { 
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        { 
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
        }
    }

    private void setFacingDirection()
    {
        float value = moveDirectionX;

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
    #endregion


    #region Other
    public void SwitchActionMapToUI()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Enable();
        print("switched to UI");
    }


    public void SwitchActionMapToPlayer()
    {
        playerInput.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();
        print("switched to Player");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


    void OnEnable()
    {
        TouchDmg.HitBounce += HandleBouceDirection;
        BasicAttackPatern.HitBounce += HandleBouceDirection;
        projectileStraight.HitBounce += HandleBouceDirection;
        projectileArch.HitBounce += HandleBouceDirection;
    }


    void OnDisable()
    {
        TouchDmg.HitBounce -= HandleBouceDirection;
        BasicAttackPatern.HitBounce -= HandleBouceDirection;
        projectileStraight.HitBounce -= HandleBouceDirection;
        projectileArch.HitBounce += HandleBouceDirection;
    }
    #endregion
}