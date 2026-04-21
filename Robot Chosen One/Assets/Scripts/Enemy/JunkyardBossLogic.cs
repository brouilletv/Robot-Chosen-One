using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkyardBossLogic : MonoBehaviour
{
    #region Variables & Initialize
    [Header("Boss Heath & Phases")]
    [SerializeField] float BossMaxHealth = 30f;
    private float BossHealth;
    private bool BossHeathLossCooldown;
    private float BossHeathLossCooldownTime = 1f;

    [SerializeField] float[] BossPhaseTrigger = {15f, 0f};
    private int BossPhase = 1;

    [Header("Player")]
    private GameObject Player;

    [Header("Rush")]
    private int RushDir;
    [SerializeField] float RushSpeed = 1;
    private bool RushActive = false;
    [SerializeField] int RushDmg = 2;
    [SerializeField] int RushCooldown = 2;

    [Header("Shockwave")]

    [Header("General Settings")]
    private Transform MinPos;
    private Transform MaxPos;
    private Rigidbody2D RB;
    private bool GlobalCooldown = false;

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
    }
    #endregion

    #region Update
    void Update()
    {
        
    }
    #endregion

    #region Health & Phases
    public void TakeDamage(float amount)
    {
        if (BossHeathLossCooldown == false)
        {
            BossHealth -= amount;
            Debug.Log(BossHealth);
            if(BossHealth <= BossPhaseTrigger[1])
            {
                BossPhase = 3;
                Debug.Log(BossPhase);
                Destroy(transform.parent.gameObject);
            }
            else if (BossHealth <= BossPhaseTrigger[0])
            {
                BossPhase = 2;
                Debug.Log(BossPhase);
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
        }
        else
        {
            RushDir = 1;
        }

        BDT.Active = false;
        BDB.Active = false;
        RushActive = true;

        while (transform.position.x > MinPos.position.x + 1.5 || transform.position.x < MaxPos.position.x - 1.5)
        {
            RB.velocity = new Vector2(RushDir * RushSpeed * 10, RB.velocity.y);
        }

        RB.velocity = new Vector2(0, RB.velocity.y);

        RushActive = false;
        BDT.Active = true;
        BDB.Active = true;
    }

    void Shockwave()
    {

    }
    #endregion

    #region Colliders Things
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (RushActive is true)
            {

            }
        }
    }
    #endregion
}
