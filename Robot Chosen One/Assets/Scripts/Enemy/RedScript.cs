using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedScript : MonoBehaviour
{
    public bool Active = false;
    private bool OnCooldown = false;

    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;

    private int TouchedOnRight = 2;

    public void InitializeRed(GameObject Player)
    {
        this.Player = Player;

        this.PM = Player.GetComponent<PlayerMovement>();
        this.HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && OnCooldown is false && Active is true)
        {
            StartCoroutine(Cooldown());
            SideTouched();
            TakeDmg(2);
        }

        IEnumerator Cooldown()
        {
            OnCooldown = true;
            yield return new WaitForSeconds(2f);
            OnCooldown = false;
        }
    }

    public void TakeDmg(int dmg)
    {
        HHB.TakeDamage(dmg);
    }

    void SideTouched()
    {
        float PlayerX = Player.transform.position.x;
        float BodyX = transform.position.x;

        if (BodyX > PlayerX)
        {
            TouchedOnRight = 0;
        }
        else if (BodyX < PlayerX)
        {
            TouchedOnRight = 1;
        }

        Bounce(TouchedOnRight);
    }
    public void Bounce(int side)
    {
        PM.HandleBouceDirection(side);
    }
}
