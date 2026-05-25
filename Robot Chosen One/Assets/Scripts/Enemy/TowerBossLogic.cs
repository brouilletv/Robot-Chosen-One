using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossLogic : MonoBehaviour
{
    [SerializeField] int MaxHealth = 50;
    private int Health;

    private bool OnCooldown = false;

    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;
    private GameObject SlashObject;
    private RedScript Slash;

    public void InitializeBossLogic(GameObject Player, RedScript Slash)
    {
        this.Player = Player;
        this.Slash = Slash;

        SlashObject = Slash.gameObject;

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();
    }

    /*
     * screen corrupter <-- curently doing that (find a way to activate the blind group)
     * inverse mouvement - inervse camera color
     * maybe clone
     */

    private void Update()
    {
        if (OnCooldown is false)
        {
            StartCoroutine(ActionSlash());
            StartCoroutine(Cooldown(4f));
        }
    }

    IEnumerator Cooldown(float delay)
    {
        OnCooldown = true;
        yield return new WaitForSeconds(delay);
        OnCooldown = false;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Destroy(transform.parent.gameObject);
            PM.defeatedTowerBoss = true;
        }
    }

    IEnumerator ActionSlash()
    {
        Vector3 offset = new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 0);
        transform.position = Player.transform.position + offset;
        yield return new WaitForSeconds(1f);
        Vector3 slashOffset = (-1 * offset) * 0.75f;
        SlashObject.transform.localPosition = slashOffset;
        Slash.Active = true;    
        yield return new WaitForSeconds(1f);
        SlashObject.transform.localPosition = new Vector3(0, 0, 0);
        Slash.Active = false;
    }

    IEnumerator ActionScreenCorrupter()
    {
        
        yield return new WaitForSeconds(4f);
    }
}
