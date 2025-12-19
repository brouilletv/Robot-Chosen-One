using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] SpriteRenderer robotSprite;

    [SerializeField] float groundSpeed;
    [SerializeField] float jumpForce;

    private float xDirection;
    private float xFlipDirection;

    private void FixedUpdate()
        {
            rb.velocity = new Vector2(xDirection * groundSpeed, rb.velocity.y);
            Flip();
        }

    private void Flip()
    {
        xFlipDirection = Input.GetAxisRaw("Horizontal");

        if(xFlipDirection == 1)
        {
            robotSprite.flipX = false;
        }

        else if(xFlipDirection == -1)
        {
            robotSprite.flipX = true;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        xDirection = context.ReadValue<Vector2>().x;
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, GroundLayer);
    }
}