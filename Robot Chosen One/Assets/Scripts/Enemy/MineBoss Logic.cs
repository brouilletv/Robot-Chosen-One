using System.Collections;
using System.Collections.Generic;
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

    public bool CanDmg = false;
    private bool Active = true;
    private int Phase = 1;

    public void InitializeMineBoss(GameObject Player, PowerBoxLogic PBL1, PowerBoxLogic PBL2, PowerBoxLogic PBL3, PowerBoxLogic PBL4, bool Active)
    {
        this.Player = Player;
        this.PBL1 = PBL1;
        this.PBL2 = PBL2;
        this.PBL3 = PBL3;
        this.PBL4 = PBL4;

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();

        transform.GetComponent<SpriteRenderer>().color = Color.blue;

        if (Active is false)
        {
            TakeDamage(MaxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Active = false;
            transform.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void Update()
    {
        if (PBL1.Active is false && PBL2.Active is false && PBL3.Active is false && PBL4.Active is false)
        {
            Phase = 1;
        }
    }


}
