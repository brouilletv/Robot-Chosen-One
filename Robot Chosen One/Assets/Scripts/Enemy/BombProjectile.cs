using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    private int BombDmg;
    private float BombFuse;
    private float BombRadius;

    private int TouchedOnRight = 2;
    private bool CanDmg = false;
    private bool OnCooldown = false;
    private float CooldownTime = 1f;

    public void InitializeBomb(GameObject Player, PlayerMovement PM, HealthHeartBarV2 HHB, int BombDmg, float BombFuse, float BombRadius)
    {
        this.Player = Player;
        this.PM = PM;
        this.HHB = HHB;
        this.BombDmg = BombDmg;
        this.BombFuse = BombFuse;
        this.BombRadius = BombRadius;
        StartCoroutine(Bomb());
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && OnCooldown is false && CanDmg is true)
        {
            StartCoroutine(Cooldown());
            SideTouched();
            TakeDmg(BombDmg);
        }

        IEnumerator Cooldown()
        {
            OnCooldown = true;
            yield return new WaitForSeconds(CooldownTime);
            OnCooldown = false;
        }
    }

    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(BombFuse/2);
        CanDmg = true;
        yield return new WaitForSeconds(BombFuse);
        Destroy(gameObject);
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
