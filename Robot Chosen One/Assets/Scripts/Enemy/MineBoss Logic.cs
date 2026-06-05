using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineBossLogic : MonoBehaviour
{
    [SerializeField] float MaxHealth = 3;
    private float Health;

    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    private PowerBoxLogic PBL1;
    private PowerBoxLogic PBL2;
    private PowerBoxLogic PBL3;
    private PowerBoxLogic PBL4;
    private Transform MinPos;
    private Transform MaxPos;

    public bool CanDmg = false;
    private bool Active = true;
    private int Phase = 0;
    private bool quickfix = false;

    private int AttackCount = 0;
    private float BombDelay = 0.5f;
    private bool BombOnCooldown = false;
    [SerializeField] int BombDmg = 2;
    [SerializeField] float BombFuse = 2;
    [SerializeField] float BombRadius = 2;
    [SerializeField] GameObject Projectile;

    private Animator animator = null;


    public void InitializeMineBoss(GameObject Player, PowerBoxLogic PBL1, PowerBoxLogic PBL2, PowerBoxLogic PBL3, PowerBoxLogic PBL4, Transform MinPos, Transform MaxPos, bool Active)
    {
        this.Player = Player;
        this.PBL1 = PBL1;
        this.PBL2 = PBL2;
        this.PBL3 = PBL3;
        this.PBL4 = PBL4;
        this.MinPos = MinPos;
        this.MaxPos = MaxPos;

        if (transform.Find("Main Boss").gameObject.TryGetComponent<Animator>(out animator))
        {
            animator = transform.Find("Main Boss").gameObject.GetComponent<Animator>();
        }

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();

        if (animator != null)
        {
            animator.SetInteger("A_Phase", 1);
        }

        if (Active is false)
        {
            TakeDamage(MaxHealth);
            if (animator != null)
            {
                animator.SetInteger("A_Phase", 3);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        CanDmg = false;
        BombOnCooldown = false;
        if (Health == 2)
        {
            Phase = 1;
            if (animator != null)
            {
                animator.SetInteger("A_Phase", 1);
            }
        }
        else if (Health == 1)
        {
            Phase = 2;
            if (animator != null)
            {
                animator.SetInteger("A_Phase", 1);
            }
            BombDelay = BombDelay / 2;
        }
        else if (Health <= 0)
        {
            Active = false;
            PM.defeatedMinesBoss = true;
            if (animator != null)
            {
                animator.SetInteger("A_Phase", 3);
            }
        }
    }

    private void Update()
    {
        if (PBL1.Active is false && PBL2.Active is false && PBL3.Active is false && PBL4.Active is false && Active is true && quickfix is false)
        {
            quickfix = true;
            CanDmg = true;
            BombOnCooldown = true;
            if (animator != null)
            {
                animator.SetInteger("A_Phase", 2);
            }
        }
        if (Phase >= 1 && BombOnCooldown is false && Active is true)
        {
            if (AttackCount >= 3)
            {
                AttackCount = 0;
                CanDmg = true;
                BombOnCooldown = true;
                if (animator != null)
                {
                    animator.SetInteger("A_Phase", 2);
                }
            }
            else
            {
                StartCoroutine(Bomb());
            }
        }
    }

    IEnumerator Bomb()
    {
        BombOnCooldown = true;
        foreach (int i in Enumerable.Range(0, Phase*10))
        {
            yield return new WaitForSeconds(BombDelay);

            Vector3 RPos = new Vector3(Random.Range(MinPos.position.x, MaxPos.position.x), Random.Range(MinPos.position.y, MaxPos.position.y), 0);

            GameObject Clone = Instantiate(Projectile, RPos, transform.rotation, transform);
            BombProjectile BombScript = Clone.transform.GetComponent<BombProjectile>();
            BombScript.InitializeBomb(Player, PM, HHB, BombDmg, BombFuse, BombRadius);
        }

        yield return new WaitForSeconds(BombFuse);
        AttackCount += 1;

        BombOnCooldown = false;
    }


}
