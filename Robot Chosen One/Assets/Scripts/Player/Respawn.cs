using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Transform defaultSpawnpoint;
    public Transform currentSpawnpoint;
    public Transform platformingSpawnpoint;

    private HealthHeartBarV2 healthScript;
    private float playerHealth;

    public static event Action<bool> enemyRespawn;

    void Awake()
    {
        currentSpawnpoint = defaultSpawnpoint;
        platformingSpawnpoint = currentSpawnpoint;
        healthScript = player.transform.GetChild(1).GetChild(0).GetComponent<HealthHeartBarV2>();
    }

    void OnEnable()
    {
        HealthHeartBarV2.Respawn += HandleRespawn;
        SpikeLogic.Respawn += HandleRespawn;
        DeathZone.Respawn += HandleRespawn;
        SpawnLocation.NewLocation += UpdateLocation;
    }

    void OnDisable()
    {
        HealthHeartBarV2.Respawn -= HandleRespawn;
        SpikeLogic.Respawn -= HandleRespawn;
        DeathZone.Respawn -= HandleRespawn;
        SpawnLocation.NewLocation -= UpdateLocation;
    }

    void HandleRespawn(int respawnTime)
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        playerHealth = healthScript.health;

        if (playerHealth <= 0)
        {
            player.transform.position = currentSpawnpoint.position;
            enemyRespawn?.Invoke(true);
        }
        else if (playerHealth > 0)
        {
            player.transform.position = platformingSpawnpoint.position;
        }
    }

    private void UpdateLocation(Transform newLocation)
    {
        healthScript.Heal(healthScript.maxHealth - healthScript.health);
        enemyRespawn?.Invoke(true);
        currentSpawnpoint = newLocation;
    }
}