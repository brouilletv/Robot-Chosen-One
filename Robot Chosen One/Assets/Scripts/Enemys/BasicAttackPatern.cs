using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackPatern : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] GameObject player;
    [SerializeField] private Transform playerT;

    [Header("Enemy settings")]
    [SerializeField] GameObject enemy;
    [SerializeField] private Transform enemyT;

    [SerializeField] Transform edgeRight;
    [SerializeField] Transform edgeLeft;

    [SerializeField] BoxCollider2D meleeHitbox;

    [Header("Melee settings")]
    [SerializeField] Transform meleeRight;
    [SerializeField] Transform meleeLeft;
    [SerializeField] float meleeDmg;

    [Header("other settings")]
    private string playerDirection = null;

    // Start is called before the first frame update
    void Start()
    {
        Transform playerT = player.GetComponent<Transform>();

        Transform enemyT = enemy.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDirection != null)
        {
            CheckHitboxMelee();
        }
    }

    void InRangeMelee()
    {
        if (playerT.position.y >= edgeLeft.position.y && playerT.position.y <= edgeRight.position.y)
        {
            if (playerT.position.x <= meleeRight.position.x && playerT.position.x >= enemyT.position.x)
            {
                playerDirection = "Right";
            }
            else if (playerT.position.x > meleeLeft.position.x && playerT.position.x <= enemyT.position.x)
            {
                playerDirection = "Left";
            }
            else
            {
                playerDirection = null;
            }
        }
        else
        {
            playerDirection = null;
        }
    }

    void CheckHitboxMelee()
    {
        if(playerDirection == "Right")
        {

        }
    }
}
