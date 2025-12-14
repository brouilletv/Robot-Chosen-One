using UnityEngine;

public class OldPlayerMovement : MonoBehaviour
{
    private Rigidbody2D RigidBody;
    public SpriteRenderer RobotF;
    public Transform RobotT;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    private float HorizontalInput;
    private bool isGrounded;

    public float Speed = 5f;
    public float JumpForce = 10f;

    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        if(HorizontalInput == 1 )
        {
            RobotF.flipX = false;
            RobotT.localPosition = new Vector2(-0.03f, 0f);
        }
        else if(HorizontalInput == -1)
        {
            RobotF.flipX = true;
            RobotT.localPosition = new Vector2(0.03f, 0f);
        }
        RigidBody.velocity = new Vector2(HorizontalInput * Speed, RigidBody.velocity.y);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            RigidBody.velocity = new Vector2(RigidBody.velocity.x, JumpForce);
        }

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.25f, GroundLayer);

    }
}
