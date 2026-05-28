using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JunkyardBossLogic : MonoBehaviour
{
    #region Variables & Initialize
    [Header("Boss Heath & Phases")]
    [SerializeField] float BossMaxHealth = 50f;
    [SerializeField] float BossHealth;

    [SerializeField] float[] BossPhaseTrigger = {25f, 0f};
    private int BossPhase = 1;

    [Header("Player")]
    private GameObject Player;

    [Header("Rush")]
    private int RushDir;
    [SerializeField] float RushSpeed = 1.5f;
    private bool RushActive = false;
    [SerializeField] int RushDmg = 3;
    [SerializeField] int RushCooldown = 8;

    [Header("Shockwave")]
    [SerializeField] float ShockwaveSpeed = 2;
    [SerializeField] int ShockwaveDmg = 2;
    [SerializeField] int ShockwaveCooldown = 6;
    [SerializeField] GameObject projectilePrefab;

    [Header("General Settings")]
    private Transform MinPos;
    private Transform MaxPos;
    private Rigidbody2D RB;
    private bool flip;
    private bool GlobalCooldown = false;
    private int Direction = 0;

    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;

    public void InitializeBossLogic(GameObject Player, Transform MinPos, Transform MaxPos)
    {
        this.Player = Player;
        this.MinPos = MinPos;
        this.MaxPos = MaxPos;

        BossHealth = BossMaxHealth;
        RB = transform.GetComponent<Rigidbody2D>();
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();

        StartCoroutine(WaitCooldown(5));
    }
    #endregion

    #region Update
    void Update()
    {
        if (GlobalCooldown is false)
        {
            int attackR = Random.Range(1, 3);
            if (attackR == 1 || BossPhase == 3)
                Shockwave();
            else if (attackR == 2)
                Rush();
        }
        if (flip is true)
        {
            transform.Find("Top").GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (flip is false)
        {
            transform.Find("Top").GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    #endregion

    #region Health & Phases
    public void TakeDamage(float amount)
    {
        BossHealth -= amount;
        if(BossHealth <= BossPhaseTrigger[1])
        {
            BossPhase = 3;
        }
        else if (BossHealth <= BossPhaseTrigger[0])
        {
            BossPhase = 2;
        }
    }

    #endregion

    #region Attacks
    void Rush()
    {
        BodyDmg BDT = transform.Find("Top").GetComponent<BodyDmg>();

        if (transform.position.x > Player.transform.position.x)
        {
            RushDir = -1;
            Direction = 0;
        }
        else
        {
            RushDir = 1;
            Direction = 1;
        }

        BDT.Active = false;
        RushActive = true;

        StartCoroutine(RushAction(BDT));
        StartCoroutine(WaitCooldown(RushCooldown));
    }

    void Shockwave()
    {
        StartCoroutine(ShockwaveAction());
        StartCoroutine(WaitCooldown(ShockwaveCooldown));
    }

    IEnumerator ShockwaveAction()
    {
        foreach (int i in Enumerable.Range(1, BossPhase))
        {
            projectileStraight projectile1 = Instantiate(projectilePrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation).GetComponent<projectileStraight>();
            projectile1.Initializeprojectile(Player.transform.position, transform.position, 2 * ShockwaveSpeed, "Right", ShockwaveDmg);

            projectileStraight projectile2 = Instantiate(projectilePrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation).GetComponent<projectileStraight>();
            projectile2.Initializeprojectile(Player.transform.position, transform.position, 2 * ShockwaveSpeed, "Left", ShockwaveDmg);

            yield return new WaitForSeconds(2f);
        }
        if (BossPhase == 3)
        {
            Destroy(transform.parent.gameObject);
            PM.defeatedJunkyardBoss = true;
        }
    }

    IEnumerator RushAction(BodyDmg BDT)
    {
        if (BossPhase == 1)
        {
            if (RushDir == 1)
            {
                flip = true;
            }
            else if (RushDir == -1)
            {
                flip = false;
            }
            while (transform.position.x > MinPos.position.x + 2 && RushDir == -1 || transform.position.x < MaxPos.position.x - 2 && RushDir == 1)
            {
                RB.velocity = new Vector2(RushDir * RushSpeed * 6, RB.velocity.y);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            if (RushDir == 1)
            {
                flip = true;
            }
            else if (RushDir == -1)
            {
                flip = false;
            }
            while (transform.position.x > MinPos.position.x + 2 && RushDir == -1 || transform.position.x < MaxPos.position.x - 2 && RushDir == 1)
            {
                RB.velocity = new Vector2(RushDir * RushSpeed * 6, RB.velocity.y);
                yield return new WaitForSeconds(0.1f);
            }
            RushDir = -RushDir;
            if (RushDir == 1)
            {
                flip = true;
            }
            else if (RushDir == -1)
            {
                flip = false;
            }
            if (Direction == 1)
            {
                Direction = 0;
            }
            else if (Direction == 0)
            {
                Direction = 1;
            }
            yield return new WaitForSeconds(1f);
            while (transform.position.x > MinPos.position.x + 2 && RushDir == -1 || transform.position.x < MaxPos.position.x - 2 && RushDir == 1)
            {
                RB.velocity = new Vector2(RushDir * RushSpeed * 6, RB.velocity.y);
                yield return new WaitForSeconds(0.1f);
            }
        }
        RB.velocity = new Vector2(0, RB.velocity.y);

        RushActive = false;
        BDT.Active = true;
    }

    IEnumerator WaitCooldown(float cooldown)
    {
        GlobalCooldown = true;
        yield return new WaitForSeconds(cooldown);
        GlobalCooldown = false;
    }

    #endregion

    #region Colliders Things
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (RushActive is true)
            {
                PM.JunkyardBossRush(Direction);
                HHB.TakeDamage(RushDmg);
            }
        }
    }
    #endregion
}
