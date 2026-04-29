using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JunkyardBossLogic : MonoBehaviour
{
    #region Variables & Initialize
    [Header("Boss Heath & Phases")]
    [SerializeField] float BossMaxHealth = 25f;
    private float BossHealth;
    private bool BossHeathLossCooldown;
    private float BossHeathLossCooldownTime = 1f;

    [SerializeField] float[] BossPhaseTrigger = {15f, 1f};
    private int BossPhase = 1;

    [Header("Player")]
    private GameObject Player;

    [Header("Rush")]
    private int RushDir;
    [SerializeField] float RushSpeed = 1;
    private bool RushActive = false;
    [SerializeField] int RushDmg = 2;
    [SerializeField] int RushCooldown = 10;

    [Header("Shockwave")]
    [SerializeField] float ShockwaveSpeed = 1;
    private bool ShockwaveActive = false;
    [SerializeField] int ShockwaveDmg = 1;
    [SerializeField] int ShockwaveCooldown = 10;
    [SerializeField] GameObject projectilePrefab;

    [Header("General Settings")]
    private Transform MinPos;
    private Transform MaxPos;
    private Rigidbody2D RB;
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
    }
    #endregion

    #region Health & Phases
    public void TakeDamage(float amount)
    {
        if (BossHeathLossCooldown == false)
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
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        BossHeathLossCooldown = true;
        yield return new WaitForSeconds(BossHeathLossCooldownTime);
        BossHeathLossCooldown = false;
    }
    #endregion

    #region Attacks
    void Rush()
    {
        BodyDmg BDT = transform.Find("Top").GetComponent<BodyDmg>();
        BodyDmg BDB = transform.Find("Bottom").GetComponent<BodyDmg>();

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
        BDB.Active = false;
        RushActive = true;

        StartCoroutine(RushAction(BDT, BDB));
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
            projectileStraight projectile1 = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<projectileStraight>();
            projectile1.Initializeprojectile(Player.transform.position, transform.position, 2, "Right", ShockwaveDmg);

            projectileStraight projectile2 = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<projectileStraight>();
            projectile2.Initializeprojectile(Player.transform.position, transform.position, 2, "Left", ShockwaveDmg);

            yield return new WaitForSeconds(2f);
        }
        if (BossPhase == 3)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    IEnumerator RushAction(BodyDmg BDT, BodyDmg BDB)
    {
        if (BossPhase == 1)
        {
            while (transform.position.x > MinPos.position.x + 3 && RushDir == -1 || transform.position.x < MaxPos.position.x - 3 && RushDir == 1)
            {
                RB.velocity = new Vector2(RushDir * RushSpeed * 4, RB.velocity.y);
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            while (transform.position.x > MinPos.position.x + 3 && RushDir == -1 || transform.position.x < MaxPos.position.x - 3 && RushDir == 1)
            {
                RB.velocity = new Vector2(RushDir * RushSpeed * 4, RB.velocity.y);
                yield return new WaitForSeconds(1f);
            }
            RushDir = -RushDir;
            while (transform.position.x > MinPos.position.x + 3 && RushDir == -1 || transform.position.x < MaxPos.position.x - 3 && RushDir == 1)
            {
                RB.velocity = new Vector2(RushDir * RushSpeed * 4, RB.velocity.y);
                yield return new WaitForSeconds(1f);
            }
        }
        RB.velocity = new Vector2(0, RB.velocity.y);

        RushActive = false;
        BDT.Active = true;
        BDB.Active = true;
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
                HHB.Heal(-RushDmg);
            }
        }
    }
    #endregion
}
