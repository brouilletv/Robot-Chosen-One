using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerMovement playerMovement;
    public Animator fadeAnim;

    public Transform currentSpawnpoint;
    public Transform platformingSpawnpoint;
    private bool respawnStop = false;

    private HealthHeartBarV2 healthScript;
    private float playerHealth;

    public static event Action<bool> enemyRespawn;


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentSpawnpoint != null)
        {
            currentSpawnpoint = GameObject.FindWithTag("DefaultRespawn").transform;
        }
        platformingSpawnpoint = currentSpawnpoint;
        healthScript = player.transform.GetChild(1).GetChild(0).GetComponent<HealthHeartBarV2>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }


    void HandleRespawn(float respawnTime)
    {
        if (respawnTime <= 0)
        {
            respawnTime = 0.75f;
        }

        playerHealth = healthScript.health;

        if (playerHealth <= 0 && !respawnStop)
        {
            playerMovement.PlayerStopTrue();
            respawnStop = true;
            StartCoroutine(RespawnLogic(respawnTime, true));
            enemyRespawn?.Invoke(true);
        }
        else if (playerHealth > 0 && !respawnStop)
        {
            playerMovement.PlayerStopTrue();
            respawnStop = true;
            StartCoroutine(RespawnLogic(respawnTime, false));
        }
    }


    IEnumerator RespawnLogic(float respawnTime, bool death)
    {
        fadeAnim.Play("FadeToBlack");
        yield return new WaitForSeconds(respawnTime);

        if (death)
        {
            player.transform.position = currentSpawnpoint.position;
        }

        else if (!death)
        {
            player.transform.position = platformingSpawnpoint.position;
        }

        fadeAnim.SetTrigger("ContinueFade");
        respawnStop = false;
        playerMovement.PlayerStopFalse();
    }


    private void UpdateLocation(Transform newLocation)
    {
        healthScript.Heal(healthScript.maxHealth - healthScript.health);
        enemyRespawn?.Invoke(true);
        currentSpawnpoint = newLocation;
    }


    void OnEnable()
    {
        HealthHeartBarV2.Respawn += HandleRespawn;
        SpikeLogic.Respawn += HandleRespawn;
        DeathZone.Respawn += HandleRespawn;
        SpawnLocation.NewLocation += UpdateLocation;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        HealthHeartBarV2.Respawn -= HandleRespawn;
        SpikeLogic.Respawn -= HandleRespawn;
        DeathZone.Respawn -= HandleRespawn;
        SpawnLocation.NewLocation -= UpdateLocation;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}