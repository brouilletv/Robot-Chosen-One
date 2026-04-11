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

    public bool defeatedJunkyardBoss = false;
    public bool defeatedMinesBoss = false;
    public bool defeatedTowerBoss = false;


    // Movement Variables
    [Header("Movement Variables")]
    [SerializeField] float groundSpeed = 6f;
    [SerializeField] float jumpForce = 17f;
    [SerializeField] float wallJumpForce = 12f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float normalGravity = 6f;
    [SerializeField] float fallGravity = 10f;
    [SerializeField] float jumpGravity = 4f;
    [SerializeField] float wallSlidingVelocityDamping;
    [SerializeField] float wallSlidingVelocityDampingModifier = 1.05f;
    [SerializeField] float wallJumpingKnockback = 25f;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    private float dashCooldownTimer = 0f;
    public float DashCooldownNormalized
    {
        get
        {
            if (canDash) return 1f;
            return 1f - (dashCooldownTimer / dashingCooldown);
        }
    }
    //[SerializeField] float wallSlidingSpeed = 2f;
    public Vector2 knockback;
    public bool playerStop;


    // Input Variables
    [Header("Input Variables")]
    public float moveDirectionX;
    public float moveDirectionY;
    public int facingDirection;
    private int spriteFacingDirection;
    public int inputVerticalDirection;
    public int wallJumpKnockbackDirection;


    // Input Booleans
    [Header("Input Booleans")]
    private bool jumpPressed;
    private bool jumpReleased;
    public bool interactPressed;
    public bool cameraPanPressed;


    // Movement Booleans
    private bool isGrounded;
    public bool isWalled;
    private bool isDashing = false;
    private bool canDash = true;
    private bool canDoubleJump = true;
    //private bool isWallSliding;
    //private bool isWallJumping;


    // Ground Check
    [Header("GroundCheck")]
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;


    // Wall Check
    [Header("WallCheck")]
    [SerializeField] float wallCheckSize = 1f;
    [SerializeField] float wallCheckHeight = 0.8f;
    [SerializeField] private LayerMask wallLayer;


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
        HandleFlip();
        if (!canDash && dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer < 0f)
            {
                dashCooldownTimer = 0f;
            }
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
            WalledCheck();
            HandleMovement();
            HandleJump();
            wallSlidingVelocityDamping = 0f;
            knockback = Vector2.zero;
        }
        else
        {
            rb.velocity = Vector2.zero;
            knockback = Vector2.zero;
        }
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
                else if (isWalled && (facingDirection != 0) && unlockedWallJump)
                {
                    jumpPressed = true;
                    jumpReleased = false;
                }
                else if (!isGrounded && !isWalled && unlockedDoubleJump)
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


    // Pans the camera up or down when ctrl is pressed
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
        }

        else if (facingDirection == -1)
        {
            robotSprite.flipX = true;
        }
    }


    void HandleGravity()
    {
        if (!isDashing)
        {
            if (rb.velocity.y < -0.05f)
            {
                if (unlockedWallJump)
                {
                    if (!isWalled)
                    {
                        rb.gravityScale = fallGravity;
                    }
                    else if (isWalled)
                    {
                        rb.gravityScale = fallGravity;

                        if (facingDirection != 0)
                        {
                            wallSlidingVelocityDamping = rb.velocity.y / wallSlidingVelocityDampingModifier;
                        }
                    }
                }
                else if (!unlockedWallJump)
                {
                    rb.gravityScale = fallGravity;
                }
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
            rb.velocity = new Vector2((facingDirection * groundSpeed) + knockback.x, rb.velocity.y + knockback.y - wallSlidingVelocityDamping);
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
            else if (isWalled && unlockedWallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x + (wallJumpKnockbackDirection * wallJumpingKnockback), wallJumpForce);
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
        else
        {
            wallJumpKnockbackDirection = -spriteFacingDirection;
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


    private IEnumerator HandleDash()
    {
        if (!playerStop)
        {
            isDashing = true;
            canDash = false;
            dashCooldownTimer = dashingCooldown;
            rb.velocity = new Vector2(spriteFacingDirectio * dashingPower, 0f);
            rb.gravityScale = 0f;

            //tr.emitting = true;

            yield return new WaitForSeconds(dashingTime);
            isDashing = false;

            //tr.emitting = false;

            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
            dashCooldownTimer = 0f;
        }
    }


    void HandleBouceDirection(int direction)
    {
        if (direction == 0)
        {
            knockback = new Vector2(-50, 10);
        }
        else if (direction == 1)
        {
            knockback = new Vector2(50, 10);
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
            canDoubleJump = true;
        }
    }


    private void WalledCheck()
    {
        isWalled = Physics2D.OverlapBox(wallCheck.position, new Vector2(wallCheckSize, wallCheckHeight), 0f, wallLayer);
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
            spriteFacingDirection = facingDirection;
        }
    }


    private void setInputVerticalDirection()
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
        Gizmos.DrawWireCube(wallCheck.position, new Vector2(wallCheckSize, wallCheckHeight));
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