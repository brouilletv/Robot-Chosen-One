using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoxLogic : MonoBehaviour
{
    #region Variables & Initialize
    [Header("Heath")]
    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    [Header("Player")]
    [SerializeField] float MaxHealth = 5;
    private float Health;
    [Header("Genral")]
    private Rigidbody2D RB;
    [Header("Electicity")]
    [SerializeField] GameObject Projectile;
    [SerializeField] float ProjectileDelay = 2f;
    private bool ProjectileDelayActive = false;
    [SerializeField] float ProjectileSpeed = 2f;
    private List<Vector2> PointList;

    /*
    instancitate the ball
    make the ball go tought the point before destroying
    make health and change themself at 0
     */
    public void InitializePowerBox(GameObject Player)
    {
        this.Player = Player;

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();

        PointList  = new List<Vector2> {
        new Vector2(transform.Find("Point 1").position.x, transform.Find("Point 1").position.y),
        new Vector2(transform.Find("Point 2").position.x, transform.Find("Point 2").position.y),
        new Vector2(transform.Find("Point 3").position.x, transform.Find("Point 3").position.y),
        new Vector2(transform.Find("Point 4").position.x, transform.Find("Point 4").position.y),
        new Vector2(transform.Find("Point 5").position.x, transform.Find("Point 5").position.y)
        };
    }

    #endregion

    IEnumerator PlasmaBall(float delay)
    {
        ProjectileDelayActive = true;

        GameObject Clone = Instantiate(Projectile, transform.position, transform.rotation, transform);
        PlasmaBall PlasmaScript = Clone.transform.GetComponent<PlasmaBall>();
        PlasmaScript.InitializePlasmaBall(Player, PointList, PM, HHB, ProjectileSpeed);

        yield return new WaitForSeconds(delay);
        ProjectileDelayActive = false;
    }

    void Update()
    {
        if (ProjectileDelayActive is false)
        {
            StartCoroutine(PlasmaBall(ProjectileDelay));
        }
    }
}
