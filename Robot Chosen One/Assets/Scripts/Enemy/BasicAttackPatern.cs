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
    [SerializeField] GameObject enemyObject;
    [SerializeField] GameObject enemy;
    private Transform enemyT;

    [SerializeField] Transform edgeRight;
    [SerializeField] Transform edgeLeft;

    [Header("Melee settings")]
    [SerializeField] bool melee;

    [SerializeField] Transform meleeRight;
    [SerializeField] Transform meleeLeft;

    [SerializeField] GameObject meleeHitbox;
    private BoxCollider2D meleeHitboxC;
    private Transform meleeHitboxT;

    [SerializeField] int meleeDmg;
    private bool meleeOnCooldown = false;
    [SerializeField] float meleeCooldownTime;

    public static event Action<int> Hit;
    public static event Action<int> HitBounce;

    [Header("Range settings")]
    [SerializeField] bool range;

    [SerializeField] GameObject projectile;

    private bool rangeOnCooldown = false;
    [SerializeField] float rangeCooldownTime;

    [Header("other settings")]
    private string playerDirection = null;
    private int playerDirectionInt = 2;

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
        if(melee is true && meleeOnCooldown is false)
        {
            InRangeMelee();
            CheckHitboxMelee();
        }
        if (range is true && rangeOnCooldown is false)
        {
            InRangeRange();
            CheckHitboxRange();
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

    void InRangeRange()
    {
        if (playerT.position.x >= edgeLeft.position.x && playerT.position.x <= edgeRight.position.x && playerT.position.y >= edgeLeft.position.y && playerT.position.y <= edgeRight.position.y)
        {
            if (playerT.position.x >= enemyT.position.x)
            {
                playerDirection = "Right";
            }
            else if (playerT.position.x <= enemyT.position.x)
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
            playerDirectionInt = 1;
        }
        else if (playerDirection == "Left")
        {
            meleeHitboxT.localPosition = new Vector2(-1f, 0f);
            playerDirectionInt = 0;
        }
        else
        {
            meleeHitboxT.localPosition = new Vector2(0f, 0f);
        }

        if (meleeHitboxC.IsTouching(playerC) && meleeOnCooldown == false)
        {
            StartCoroutine(MeleeCooldown());
            ApplyDmg(meleeDmg);
            Bouce(playerDirectionInt);
        }
    }

    void CheckHitboxRange()
    {
        if (playerDirection == "Right" || playerDirection == "Left")
        {
            StartCoroutine(RangeCooldown());
            Instantiate(projectile, enemyT.position, enemyT.rotation);
        }
    }

    void ApplyDmg(int dmg)
    {
        Hit?.Invoke(dmg);
    }

    public void Bouce(int side)
    {
        HitBounce?.Invoke(side);
    }

    IEnumerator MeleeCooldown()
    {
        meleeOnCooldown = true;
        yield return new WaitForSeconds(meleeCooldownTime);
        meleeOnCooldown = false;
    }

    IEnumerator RangeCooldown()
    {
        rangeOnCooldown = true;
        yield return new WaitForSeconds(rangeCooldownTime);
        rangeOnCooldown = false;
    }
}
