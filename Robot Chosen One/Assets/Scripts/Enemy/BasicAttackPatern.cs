using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicAttackPatern : MonoBehaviour
{
    private GameObject player;
    private CapsuleCollider2D playerC;
    private Transform playerT;

    [Header("Enemy settings")]
    [SerializeField] GameObject enemy;
    private Transform enemyT;

    [Header("Melee settings")]
    [SerializeField] bool melee;

    private Transform MMax;
    private Transform MMin;

    private GameObject MBox;
    private BoxCollider2D meleeHitboxC;
    private Transform meleeHitboxT;

    [SerializeField] int meleeDmg;
    private bool meleeOnCooldown = false;
    [SerializeField] float meleeCooldownTime;

    public static event Action<int> Hit;
    public static event Action<int> HitBounce;

    [Header("Range settings")]
    [SerializeField] bool range;
    [SerializeField] bool rangeArch;

    private Transform RMax;
    private Transform RMin;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;

    private bool rangeOnCooldown = false;
    [SerializeField] float rangeCooldownTime;

    [SerializeField] int rangeDmg;

    private Vector2 startPos;
    private Vector2 targetPos;

    [Header("other settings")]
    private string playerDirection = null;
    private int playerDirectionInt = 2;

    public void InitializeBAP(GameObject player)
    {
        MMax = transform?.Find("MMax");
        MMin = transform?.Find("MMin");

        RMax = transform?.Find("RMax");
        RMin = transform?.Find("RMin");

        MBox = transform?.Find("MBox").gameObject;

        this.player = player;

        playerT = player.GetComponent<Transform>();
        playerC = player.GetComponent<CapsuleCollider2D>();

        enemyT = enemy.GetComponent<Transform>();

        meleeHitboxC = MBox.GetComponent<BoxCollider2D>();
        meleeHitboxT = MBox.GetComponent<Transform>();
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
        if (playerT.position.x <= MMax.position.x && playerT.position.x >= enemyT.position.x)
        {
            playerDirection = "Right";
        }
        else if (playerT.position.x >= MMin.position.x && playerT.position.x <= enemyT.position.x)
        {
            playerDirection = "Left";
        }
        else
        {
            playerDirection = null;
        }
    }

    void InRangeRange()
    {
        if (playerT.position.x >= RMin.position.x && playerT.position.x <= RMax.position.x && playerT.position.y >= RMin.position.y && playerT.position.y <= RMax.position.y)
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

            targetPos = playerT.position;
            startPos = enemyT.position;

            if (rangeArch is true)
            {
                projectileArch projectile = Instantiate(projectilePrefab, enemyT.position, enemyT.rotation).GetComponent<projectileArch>();
                projectile.Initializeprojectile(targetPos, startPos, projectileSpeed, playerDirection, rangeDmg);
            }
            else
            {
                projectileStraight projectile = Instantiate(projectilePrefab, enemyT.position, enemyT.rotation).GetComponent<projectileStraight>();
                projectile.Initializeprojectile(targetPos, startPos, projectileSpeed, playerDirection, rangeDmg);
            }
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
