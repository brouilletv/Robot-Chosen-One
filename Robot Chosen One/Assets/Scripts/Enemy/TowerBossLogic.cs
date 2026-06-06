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

    private int BossPhase = 1;

    public void InitializeBossLogic(GameObject Player, RedScript Slash)
    {
        this.Player = Player;
        this.Slash = Slash;

        SlashObject = Slash.gameObject;

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();
        StartCoroutine(Cooldown(4f));
    }

    private void Update()
    {
        if (OnCooldown is false)
        {
            int attackR = Random.Range(1, 4);
            if (attackR == 1 && BossPhase == 2)
            {
                StartCoroutine(ActionScreenCorrupter());
            }
            else if (attackR == 2 && BossPhase == 2)
            {
                StartCoroutine(ActionFlipMove());
            }
            else
            {
                StartCoroutine(ActionSlash());
                StartCoroutine(Cooldown(4f));
            }
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

            GameObject camGameObject = Camera.main.gameObject;

            camGameObject.transform.Find("Invert").gameObject.SetActive(false);
            camGameObject.transform.Find("BlindRight").gameObject.SetActive(false);
            camGameObject.transform.Find("BlindLeft").gameObject.SetActive(false);
        }
        else if (Health <= 25)
        {
            BossPhase = 2;
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
        GameObject camGameObject = Camera.main.gameObject;
        int blindSide = Random.Range(1, 3);

        if (blindSide == 1)
        {
            camGameObject.transform.Find("BlindRight").gameObject.SetActive(true);
        }
        else
        {
            camGameObject.transform.Find("BlindLeft").gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(16f);
        if (blindSide == 1)
        {
            camGameObject.transform.Find("BlindRight").gameObject.SetActive(false);
        }
        else
        {
            camGameObject.transform.Find("BlindLeft").gameObject.SetActive(false);
        }
    }

    IEnumerator ActionFlipMove()
    {
        GameObject camGameObject = Camera.main.gameObject;

        camGameObject.transform.Find("Invert").gameObject.SetActive(true);
        PM.mouvementInverted = true;
        yield return new WaitForSeconds(16f);
        camGameObject.transform.Find("Invert").gameObject.SetActive(false);
        PM.mouvementInverted = false;
    }
}
