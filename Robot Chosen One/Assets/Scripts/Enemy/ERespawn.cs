using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ERespawn : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;

    void Start()
    {
        Create();
    }
    void OnEnable()
    {
        SpawnLocation.enemyRespawn += Respawn;
    }


    void OnDisable()
    {
        SpawnLocation.enemyRespawn -= Respawn;
    }

    void Respawn(bool r)
    {
        if (r is true && transform.childCount == 2)
        {
            Create();
        }
    }

    void Create()
    {
        GameObject Clone = Instantiate(enemy, transform.position, transform.rotation, transform);

        PathFinder EPathFinder = Clone.GetComponent<PathFinder>();
        EPathFinder.InitializePathFinder(player.transform);

        BasicAttackPatern BAP = Clone.GetComponent<BasicAttackPatern>();
        BAP.InitializeBAP(player);
    }
}
