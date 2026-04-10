using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class ERespawn : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int Ecount = 1;
    [SerializeField] float Ecooldown = 0;

    private GameObject player;
    private Transform MaxPos;
    private Transform MinPos;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
        MaxPos = transform.Find("MaxPos");
        MinPos = transform.Find("MinPos");
        foreach (int i in Enumerable.Range(0, Ecount - (transform.childCount - 2)))
        {
            StartCoroutine(Create(i));
        }
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
        foreach (int i in Enumerable.Range(0, Ecount - (transform.childCount - 2)))
        {
            StartCoroutine(Create(i));
        }
    }

    IEnumerator Create(int n)
    {
        yield return new WaitForSeconds(n * Ecooldown);

        GameObject Clone = Instantiate(enemy, transform.position, transform.rotation, transform);

        PathFinder EPathFinder = Clone.GetComponent<PathFinder>();
        EPathFinder.InitializePathFinder(player.transform, MaxPos, MinPos);

        BasicAttackPatern BAP = Clone.GetComponent<BasicAttackPatern>();
        BAP.InitializeBAP(player);
    }
}
