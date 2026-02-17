using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Transform Body;
    [SerializeField] private Transform Player;
    [SerializeField] private Transform RightEdge;
    [SerializeField] private Transform LeftEdge;

    private bool Fallow = false;

    void Start()
    {
    }

    void FixedUpdate()
    {
        float PlayerX = Player.position.x;
        float PlayerY = Player.position.y;

        float BodyX = Body.position.x;
        float BodyY = Body.position.y;

        if (PlayerX >= LeftEdge.position.x && PlayerX <= RightEdge.position.x && PlayerY >= LeftEdge.position.y && PlayerY <= RightEdge.position.y)
        {
            Fallow = true;  
        }
        else
        {
            Fallow = false;
        }
        if (Fallow is true && BodyX > PlayerX)
        {
            RB.velocity = new Vector2(-1f, RB.velocity.y);
        }
        else if (Fallow is true && BodyX < PlayerX)
        {
            RB.velocity = new Vector2(1f, RB.velocity.y);
        }
        else
        {
            RB.velocity = new Vector2(0f, RB.velocity.y);
        }
    }
}