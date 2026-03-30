using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] bool Roming;
    [SerializeField] bool FullRoming;
    [SerializeField] bool Flying;

    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Transform Body;
    private Transform Player;
    private Transform MaxPos;
    private Transform MinPos;

    private bool Fallow = false;
    private bool GoRight = false;


    public void InitializePathFinder(Transform Player)
    {
        this.Player = Player;

        MaxPos = transform.parent?.Find("MaxPos");
        MinPos = transform.parent?.Find("MinPos");
    }
    void FixedUpdate()
    {
        float PlayerX = Player.position.x;
        float PlayerY = Player.position.y;

        float BodyX = Body.position.x;
        float BodyY = Body.position.y;

        if (PlayerX >= MinPos.position.x && PlayerX <= MaxPos.position.x && PlayerY >= MinPos.position.y && PlayerY <= MaxPos.position.y && FullRoming is false)
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
        else if (Roming is true)
        {
            if (GoRight is false && BodyX >= MinPos.transform.position.x)
            {
                RB.velocity = new Vector2(-1f, RB.velocity.y);
            }
            else if (GoRight is false)
            {
                GoRight = true;
            }
            else if (GoRight is true && BodyX <= MaxPos.transform.position.x)
            {
                RB.velocity = new Vector2(1f, RB.velocity.y);
            }
            else if (GoRight is true)
            {
                GoRight = false;
            }
        }
        else
        {
            RB.velocity = new Vector2(0f, RB.velocity.y);
        }

        if (Fallow is true && Flying is true && BodyY > PlayerY)
        {
            RB.velocity = new Vector2(RB.velocity.x, -0.5f);
        }
        else if (Fallow is true && Flying is true && BodyY < PlayerY)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0.5f);
        }
        else if (Flying is true)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0f);
        }
    }
}