using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawnerS : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    private GameObject player;
    private Transform MaxPos;
    private Transform MinPos;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
        MaxPos = transform.parent.parent.Find("MaxPos");
        MinPos = transform.parent.parent.Find("MinPos");
    }
    public void Create()
    {
        if (transform.childCount == 0)
        {
            GameObject Clone = Instantiate(enemy, transform.position, transform.rotation, transform);

            PathFinder EPathFinder = Clone.GetComponent<PathFinder>();
            EPathFinder.InitializePathFinder(player.transform, MaxPos, MinPos);

            BasicAttackPatern BAP = Clone.GetComponent<BasicAttackPatern>();
            BAP.InitializeBAP(player);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
