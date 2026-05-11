using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    private GameObject Player;
    private List<Vector3> PointList;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    private float ProjectileSpeed;
    private int ProjectileDmg;

    private int CurrentPoint = 1;
    private bool OnCooldown = false;
    private float CooldownTime = 1f;
    private int TouchedOnRight = 2;
   

    public void InitializePlasmaBall(GameObject Player, List<Vector3> PointList, PlayerMovement PM, HealthHeartBarV2 HHB, float ProjectileSpeed, int ProjectileDmg)
    {
        this.Player = Player;
        this.PointList = PointList;
        this.PM = PM;
        this.HHB = HHB;
        this.ProjectileSpeed = ProjectileSpeed;
        this.ProjectileDmg = ProjectileDmg;
    }

    void Update()
    {
        float step = ProjectileSpeed * Time.deltaTime;

        foreach (int point in Enumerable.Range(0, 5))
        {
            if (point + 1 == CurrentPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, PointList[point], step);
            }
            if (transform.position == new Vector3(PointList[point].x, PointList[point].y, 0))
            {
                CurrentPoint += 1;
            }
            if (CurrentPoint == 6)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && OnCooldown is false)
        {
            StartCoroutine(Cooldown());
            SideTouched();
            TakeDmg(ProjectileDmg);
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
