using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ERespawn : MonoBehaviour
{
    [SerializeField] GameObject enemy;

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
        PathFinder EPathFinder = Instantiate(enemy, transform.position, transform.rotation, transform).GetComponent<PathFinder>();
        EPathFinder.InitializePathFinder();
    }
}
