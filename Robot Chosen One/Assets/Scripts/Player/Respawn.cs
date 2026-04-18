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

    public Vector2 currentSpawnpoint;
    public Vector2 platformingSpawnpoint;
    private bool respawnStop = false;

    private HealthHeartBarV2 healthScript;
    private float playerHealth;

    public static event Action<bool> resetElevator;
    public static event Action<bool> enemyRespawn;

    public static Respawn instance;


    private void Awake()
    {
        instance = this;
        currentSpawnpoint = GameObject.FindWithTag("DefaultRespawn").transform.position;
    }


    private void Start()
    {
        DataContainer dataContainer = LoadSystem.LoadGame();
        if (dataContainer != null)
        {
            currentSpawnpoint = dataContainer.respawnData.currentSpawnpoint;
            platformingSpawnpoint = dataContainer.respawnData.platformingSpawnpoint;
        }
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
            resetElevator?.Invoke(true);
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
        resetElevator?.Invoke(true);
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


[System.Serializable]
public class RespawnData
{
    [SerializeField] public Vector2 currentSpawnpoint;
    [SerializeField] public Vector2 platformingSpawnpoint;

    public RespawnData(Respawn respawn)
    {
        this.currentSpawnpoint = respawn.currentSpawnpoint;
        this.platformingSpawnpoint = respawn.platformingSpawnpoint;
    }
}