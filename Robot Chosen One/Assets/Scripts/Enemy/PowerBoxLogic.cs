using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoxLogic : MonoBehaviour
{
    #region Variables & Initialize
    [Header("Player")]
    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    [Header("Health")]
    [SerializeField] float MaxHealth = 5;
    private float Health;
    private bool cooldown = false;
    [Header("Genral")]
    private Rigidbody2D RB;
    [Header("Electicity")]
    [SerializeField] GameObject Projectile;
    [SerializeField] float ProjectileDelay = 2f;
    private bool ProjectileDelayActive = false;
    [SerializeField] float ProjectileSpeed = 2f;
    [SerializeField] int ProjectileDmg = 1;
    private List<Vector3> PointList;
    public bool Active = true;

    public void InitializePowerBox(GameObject Player)
    {
        this.Player = Player;

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();

        PointList  = new List<Vector3> {
        new Vector3(transform.Find("Point 1").position.x, transform.Find("Point 1").position.y, 0),
        new Vector3(transform.Find("Point 2").position.x, transform.Find("Point 2").position.y, 0),
        new Vector3(transform.Find("Point 3").position.x, transform.Find("Point 3").position.y, 0),
        new Vector3(transform.Find("Point 4").position.x, transform.Find("Point 4").position.y, 0),
        new Vector3(transform.Find("Point 5").position.x, transform.Find("Point 5").position.y, 0)
        };
    }

    #endregion

    IEnumerator PlasmaBall(float delay)
    {
        ProjectileDelayActive = true;

        GameObject Clone = Instantiate(Projectile, transform.position, transform.rotation, transform);
        PlasmaBall PlasmaScript = Clone.transform.GetComponent<PlasmaBall>();
        PlasmaScript.InitializePlasmaBall(Player, PointList, PM, HHB, ProjectileSpeed, ProjectileDmg);

        yield return new WaitForSeconds(delay);
        ProjectileDelayActive = false;
    }

    void Update()
    {
        if (ProjectileDelayActive is false && Active is true)
        {
            StartCoroutine(PlasmaBall(ProjectileDelay));
        }
    }

    public void TakeDamage(float amount)
    {
        if (cooldown == false)
        {
            Health -= amount;
            if (Health <= 0)
            {
                Active = false;
            }
        }
    }
}
