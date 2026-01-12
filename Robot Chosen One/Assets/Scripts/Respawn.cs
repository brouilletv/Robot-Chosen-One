using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Respawn : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform player;
    [SerializeField] Transform CurrentRespawnPoint;

    void OnEnable()
    {
        HealthHeartBarV2.respawn += HandleRespawn;
    }

    void OnDisable()
    {
        HealthHeartBarV2.respawn -= HandleRespawn;
    }

    void HandleRespawn(int respawnTime)
    {
        rb.velocity = new Vector2(0, 0);
        player.transform.position = CurrentRespawnPoint.position;
    }
}