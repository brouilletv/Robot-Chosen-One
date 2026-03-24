using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Transform Body;
    private Transform Player;
    private Transform MaxPos;
    private Transform MinPos;

    private bool Fallow = false;
    public void InitializePathFinder(Transform Player)
    {
        this.Player = Player;

        MaxPos = transform.parent?.Find("MaxPos");
        MinPos = transform.parent?.Find("MinPos");

    }
    void FixedUpdate()
    {
        Debug.Log(Player);

        float PlayerX = Player.position.x;
        float PlayerY = Player.position.y;

        float BodyX = Body.position.x;
        float BodyY = Body.position.y;

        if (PlayerX >= MinPos.position.x && PlayerX <= MaxPos.position.x && PlayerY >= MinPos.position.y && PlayerY <= MaxPos.position.y)
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