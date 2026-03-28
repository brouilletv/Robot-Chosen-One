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
        Respawn.enemyRespawn += RespawnE;
    }


    void OnDisable()
    {
        Respawn.enemyRespawn -= RespawnE;
    }

    void RespawnE(bool r)
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
