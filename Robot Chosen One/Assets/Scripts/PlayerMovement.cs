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
    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer robotSprite;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private PlayerInput playerInput;

    // Flags
    [Header("Flags")]
    public string lastScene = "Junkyard Map";
    public bool unlockedDoubleJump = false;
    public bool unlockedDash = false;
    public bool unlockedWallJump = false;
    public bool maxHealthIncreaseJunkyard = false;
    public bool maxHealthIncreaseMines = false;
    public bool maxHealthIncreaseTower = false;

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
    [SerializeField] float wallSlidingSpeed = 2f;
    public bool playerStop;

    // Input Variables
    [Header("Input Variables")]
    public float moveDirectionX;
    public float moveDirectionY;
    public int facingDirection;
    public int inputVerticalDirection;
    private float previousFacingDirection;
    private Vector2 knockback;

    // Input Booleans
    [Header("Input Booleans")]
    private bool jumpPressed;
    private bool jumpReleased;
    public bool interactPressed;
    public bool cameraPanPressed;


    // Movement Booleans
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;
    private bool canDoubleJump = true;
    private bool isWallSliding;
    private bool isWallJumping;

    // Ground Check
    [Header("GroundCheck")]
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    //Wall Jump Variables
    [Header("WallJump")]
    [SerializeField] float wallJumpMoveOverrideDuration = 0.5f;
    [SerializeField] private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private float wallJumpMoveOverride = 0f;


    private void Awake()
    {
        playerStop = false;
        rb.gravityScale = normalGravity;
        playerInput = GetComponent<PlayerInput>();
    }
    #endregion


    #region Update & FixedUpdate
    private void Update()
    {
        if (!isWallJumping) 
        {
            HandleFlip();
        }
    }


    private void FixedUpdate()
    {
        if (!playerStop)
        {
            setFacingDirection();
            setInputVerticalDirection();
            HandleGravity();
            GroundedCheck();
            HandleMovement();
            HandleWallslide();
            HandleWallJump(); // ← moved here, removed from Update()
            HandleJump();
            knockback = Vector2.zero;
        }
        else
        {
            rb.velocity = Vector2.zero; 
        }
    }
    #endregion


    #region Input Methods
    public void OnMove(InputValue value)
    {
        if (!playerStop)
        {
            moveDirectionX = value.Get<Vector2>().x;
            moveDirectionY = value.Get<Vector2>().y;
        }
    }


    public void OnJump(InputValue value)
    {
        if (!playerStop)
        {
            if (value.isPressed)
            {
                if (isGrounded)
                {
                    jumpPressed = true;
                    jumpReleased = false;
                    canDoubleJump = true;
                }
                else if (isWallSliding) // ← ADD THIS BRANCH
                {
                    jumpPressed = true;
                    jumpReleased = false;
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
    }


    public void OnDash(InputValue value)
    {
        if (!playerStop)
        {
            if (canDash && unlockedDash)
            {
                StartCoroutine(HandleDash());
            }
        }
    }


    public void OnInteract(InputValue value)
    {
        if (!playerStop)
        {
            if (value.isPressed)
            {
                interactPressed = true;
            }
            else
            {
                interactPressed = false;
            }
        }
    }

    public void OnCameraPan(InputValue value)
    {
        if (!playerStop)
        {
            if (value.isPressed)
            {
                cameraPanPressed = true;
            }
            else
            {
                cameraPanPressed = false;
            }
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
        if (!isDashing && !isWallJumping)
        {
            float targetSpeed = facingDirection * groundSpeed;
            // Smoothly transition horizontal speed instead of snapping
            float newX = Mathf.Lerp(rb.velocity.x, targetSpeed, 0.1f);
            rb.velocity = new Vector2(facingDirection * groundSpeed, rb.velocity.y) + knockback;
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
        if (wallJumpMoveOverride > 0f) // ← don't allow wall slide during jump arc
        {
            isWallSliding = false;
            return;
        }

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


    private void HandleWallJump()
    {
        if (isWallSliding)
        {
            wallJumpMoveOverride = 0f; // ← clear override when touching any wall
            isWallJumping = false;
            wallJumpingDirection = -facingDirection;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jumpPressed && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            wallJumpMoveOverride = wallJumpMoveOverrideDuration;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            jumpPressed = false;

            facingDirection = (int)wallJumpingDirection;
            previousFacingDirection = facingDirection;
        }

        if (wallJumpMoveOverride > 0f)
        {
            wallJumpMoveOverride -= Time.fixedDeltaTime;
        }

        if (isWallJumping && wallJumpMoveOverride <= 0f && (isGrounded || isWallSliding))
        {
            isWallJumping = false;
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
            knockback = new Vector2(-10, 5);

        }
        else if (direction == 1)
        {
            knockback = new Vector2(10, 5);
        }
    }
    #endregion


    #region Checks & SetVariables
    public void PlayerStopTrue()
    {
        playerStop = true;
    }


    public void PlayerStopFalse()
    {
        playerStop = false;
    }

    private void GroundedCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            // isWallJumping = false; ← remove this, WallJump() handles it now
            canDoubleJump = true;
        }
    }


    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private void setFacingDirection()
    {
        if (isWallJumping) return;

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


    private void setInputVerticalDirection()
    {
        if (cameraPanPressed)
        {
            float value = moveDirectionY;

            if (value > 0)
            {
                inputVerticalDirection = 1;
            }
            else if (value < 0)
            {
                inputVerticalDirection = -1;
            }
            else if (value == 0)
            {
                inputVerticalDirection = 0;
            }
        }
    }
    #endregion


    #region Other
    public void SwitchActionMapToUI()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Enable();
    }


    public void SwitchActionMapToPlayer()
    {
        playerInput.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();
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
        projectileArch.HitBounce -= HandleBouceDirection;
    }
    #endregion
}