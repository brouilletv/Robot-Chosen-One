using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Respawn : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform player;
    [SerializeField] Transform defaultSpawn;

    private Transform currentspawn;

    void Start()
    {
        currentspawn = defaultSpawn;
    }

    void OnEnable()
    {
        HealthHeartBarV2.respawn += HandleRespawn;
        SpawnLocation.newLocation += UpdateLocation;
    }

    void OnDisable()
    {
        HealthHeartBarV2.respawn -= HandleRespawn;
        SpawnLocation.newLocation -= UpdateLocation;
    }

    void HandleRespawn(int respawnTime)
    {
        rb.velocity = new Vector2(0, 0);
        player.transform.position = currentspawn.position;
    }

    private void UpdateLocation(Transform newLocation)
    {
        currentspawn = newLocation;
    }
}