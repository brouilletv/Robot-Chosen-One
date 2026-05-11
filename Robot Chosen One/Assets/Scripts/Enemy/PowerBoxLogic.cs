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
    private LineRenderer lineRenderer;

    public void InitializePowerBox(GameObject Player, bool Active)
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

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 6;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, PointList[0]);
        lineRenderer.SetPosition(2, PointList[1]);
        lineRenderer.SetPosition(3, PointList[2]);
        lineRenderer.SetPosition(4, PointList[3]);
        lineRenderer.SetPosition(5, PointList[4]);

        if (Active is false)
        {
            TakeDamage(MaxHealth);
        }
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
        Health -= amount;
        if (Health <= 0)
        {
            Active = false;
            transform.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
