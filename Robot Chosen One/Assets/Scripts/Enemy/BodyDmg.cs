using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDmg : MonoBehaviour
{
    [SerializeField] int Dmg = 1;
    [SerializeField] float CooldownTime = 1f;
    private bool OnCooldown = false;
    private int TouchedOnRight;
    public bool Active = true;

    private GameObject Player;

    public void InitializeBodyDmg(GameObject Player)
    {
        this.Player = Player;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && OnCooldown is false && Active is true)
        {
            StartCoroutine(Cooldown());
            SideTouched();
            TakeDmg(Dmg);
        }

        IEnumerator Cooldown()
        {
            OnCooldown = true;
            yield return new WaitForSeconds(CooldownTime);
            OnCooldown = false;
        }
    }

    public void TakeDmg(int dmg)
    {
        Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>().Heal(-dmg);
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
        Player.GetComponent<PlayerMovement>().HandleBouceDirection(side);
    }
}
