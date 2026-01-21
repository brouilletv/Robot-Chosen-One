using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicAttackPatern : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] GameObject player;
    private CapsuleCollider2D playerC;
    private Transform playerT;

    [Header("Enemy settings")]
    [SerializeField] GameObject enemy;
    private Transform enemyT;

    [SerializeField] Transform edgeRight;
    [SerializeField] Transform edgeLeft;

    [Header("Melee settings")]
    [SerializeField] Transform meleeRight;
    [SerializeField] Transform meleeLeft;

    [SerializeField] GameObject meleeHitbox;
    private BoxCollider2D meleeHitboxC;
    private Transform meleeHitboxT;

    [SerializeField] int meleeDmg;
    private bool meleeOnCooldown = false;
    [SerializeField] float meleeCooldownTime;

    public static event Action<int> Hit;

    [Header("other settings")]
    private string playerDirection = null;

    void Start()
    {
        playerT = player.GetComponent<Transform>();
        playerC = player.GetComponent<CapsuleCollider2D>();

        enemyT = enemy.GetComponent<Transform>();

        meleeHitboxC = meleeHitbox.GetComponent<BoxCollider2D>();
        meleeHitboxT = meleeHitbox.GetComponent<Transform>();
    }

    void Update()
    {
        InRangeMelee();
        CheckHitboxMelee();
    }

    void InRangeMelee()
    {
        if (playerT.position.y >= edgeLeft.position.y && playerT.position.y <= edgeRight.position.y)
        {
            if (playerT.position.x <= meleeRight.position.x && playerT.position.x >= enemyT.position.x)
            {
                playerDirection = "Right";
            }
            else if (playerT.position.x >= meleeLeft.position.x && playerT.position.x <= enemyT.position.x)
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
        if (playerDirection == "Right")
        {
            meleeHitboxT.localPosition = new Vector2(1f, 0f);
        }
        else if (playerDirection == "Left")
        {
            meleeHitboxT.localPosition = new Vector2(-1f, 0f);
        }
        else
        {
            meleeHitboxT.localPosition = new Vector2(0f, 0f);
        }

        if (meleeHitboxC.IsTouching(playerC) && meleeOnCooldown == false)
        {
            StartCoroutine(MeleeCooldown());
            ApplyDmg(meleeDmg);
            Debug.Log("works");
        }
    }

    void ApplyDmg(int dmg)
    {
        Hit?.Invoke(dmg);
    }

    IEnumerator MeleeCooldown()
    {
        meleeOnCooldown = true;
        yield return new WaitForSeconds(meleeCooldownTime);
        meleeOnCooldown = false;
    }
}
