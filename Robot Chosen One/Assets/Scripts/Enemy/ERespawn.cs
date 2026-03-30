using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ERespawn : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    private GameObject player;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
        Create();
    }

    void OnEnable()
    {
        Respawn.enemyRespawn += RespawnE;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        Respawn.enemyRespawn -= RespawnE;
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
