using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthHeartBarV2 : MonoBehaviour
{
    public GameObject heartPrefab;
    private GameObject player;
    private PlayerMelee playerMelee;
    public float maxHealth = 12;
    public float startingHealth = 6;
    public float health;
    private float respawnTime = 0.75f;


    private List<HealthHeart> hearts = new List<HealthHeart>();

    public static event Action<float> Respawn;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerMelee = player.GetComponent<PlayerMelee>();
        health = Mathf.Clamp(startingHealth, 0, maxHealth);
        DrawHearts();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(1);
        }

        HandleHeal();
    }


    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
        DrawHearts();
    }

    public void TakeDamage(float amount)
    {
        SetHealth(health - amount);
        if (health <= 0)
        {
            Dead(respawnTime);
            StartCoroutine(DeathHealCooldown());
        }
    }


    public void HandleHeal()
    {
        if (playerMelee.healPressed)
        {
            if (playerMelee.attackCount == 10 && health < maxHealth)
            {
                playerMelee.healPressed = false;
                Heal(1);
                Debug.Log("Healed 1 health for reaching 10 attacks");
                playerMelee.attackCount = 0;
                Debug.Log("Attack count reset to: " + playerMelee.attackCount);
            }

            else
            {
                playerMelee.healPressed = false;
                Debug.Log("Not healed, attack count is: " + playerMelee.attackCount);
            }
        }
    }

        IEnumerator DeathHealCooldown()
    {
        yield return new WaitForSeconds(respawnTime);
        Heal(maxHealth);
    }

    public void Heal(float amount)
    {
        SetHealth(health + amount);
    }


    private void DrawHearts()
    {
        ClearHearts();

        float remainder = maxHealth % 2;
        int heartsToMake = (int)(maxHealth / 2 + remainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartValue = Mathf.Clamp((int)health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartValue);
        }
    }

    private void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform, false);
        HealthHeart heart = newHeart.GetComponent<HealthHeart>();
        heart.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heart);
    }

    private void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts.Clear();
    }

    void OnEnable()
    {
        TouchDmg.Hit += HandleHealthChanged;
        BasicAttackPatern.Hit += HandleHealthChanged;
        projectileStraight.Hit += HandleHealthChanged;
        projectileArch.Hit += HandleHealthChanged;
    }

    void OnDisable()
    {
        TouchDmg.Hit -= HandleHealthChanged;
        BasicAttackPatern.Hit -= HandleHealthChanged;
        projectileStraight.Hit -= HandleHealthChanged;
        projectileArch.Hit -= HandleHealthChanged;
    }

    void HandleHealthChanged(float newHealth)
    {
        TakeDamage(newHealth);
    }

    public void Dead(float respawnTime)
    {
        Respawn?.Invoke(respawnTime);
    }

}


