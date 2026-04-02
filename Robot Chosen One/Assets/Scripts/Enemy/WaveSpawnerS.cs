using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawnerS : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int Ecount = 1;
    [SerializeField] int Ecooldown = 0;

    private GameObject player;
    private Transform MaxPos;
    private Transform MinPos;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
        MaxPos = transform.parent.parent.Find("MaxPos");
        MinPos = transform.parent.parent.Find("MinPos");
    }
    public void Spawn()
    {
        foreach (int i in Enumerable.Range(0, Ecount - transform.childCount))
        {
            StartCoroutine(Create(i));
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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
