using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    private GameObject Player;
    private List<Vector2> PointList;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    private float ProjectileSpeed;

    private int CurrentPoint = 1;
   

    public void InitializePlasmaBall(GameObject Player, List<Vector2> PointList, PlayerMovement PM, HealthHeartBarV2 HHB, float ProjectileSpeed)
    {
        this.Player = Player;
        this.PointList = PointList;
        this.PM = PM;
        this.HHB = HHB;
        this.ProjectileSpeed = ProjectileSpeed;
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

    void Damaging()
    {

    }

    IEnumerable Cooldown()
    {
        yield return new WaitForSeconds(1);
    }
}
