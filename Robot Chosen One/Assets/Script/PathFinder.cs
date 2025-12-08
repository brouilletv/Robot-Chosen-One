using UnityEngine;

public class script : MonoBehaviour
{
    public Rigidbody2D BodyR;
    public Transform BodyT;
    public Transform Player;
    public Transform RightEdge;
    public Transform LeftEdge;

    void Start()
    {
    }

    void Update()
    {
        float PlayerX = Player.position.x;
        float PlayerY = Player.position.y;

        float BodyX = BodyT.position.x;
        float BodyY = BodyT.position.y;

        if (PlayerX >= LeftEdge.position.x && BodyX > PlayerX)
        {
            BodyR.velocity = new Vector2(-1f, BodyR.velocity.y);
        }
        else if (PlayerX <= RightEdge.position.x && BodyX < PlayerX)
        {
            BodyR.velocity = new Vector2(1f, BodyR.velocity.y);
        }
        else
        {
            BodyR.velocity = new Vector2(0f, BodyR.velocity.y);
        }
    }
}