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

    public Vector2 currentSpawnpoint = new Vector2(0, 0);
    public Vector2 platformingSpawnpoint;
    private bool respawnStop = false;

    private HealthHeartBarV2 healthScript;
    private float playerHealth;

    public static event Action<bool> enemyRespawn;


    private void Awake()
    {
        currentSpawnpoint = GameObject.FindWithTag("DefaultRespawn").transform.position;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
            player.transform.position = currentSpawnpoint;
        }

        else if (!death)
        {
            player.transform.position = platformingSpawnpoint;
        }

        fadeAnim.SetTrigger("ContinueFade");
        respawnStop = false;
        playerMovement.PlayerStopFalse();
    }


    private void UpdateLocation(Transform newLocation)
    {
        healthScript.Heal(healthScript.maxHealth - healthScript.health);
        enemyRespawn?.Invoke(true);
        currentSpawnpoint = newLocation.position;
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