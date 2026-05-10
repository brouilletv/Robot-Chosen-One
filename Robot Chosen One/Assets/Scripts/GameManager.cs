using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Persitent Objects")]
    public GameObject[] persistentObjects;
    private GameObject Player;


    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();
            return;
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }

        Player = GameObject.FindWithTag("Player");
    }


    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }


    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }

        Destroy(gameObject);
    }


    public void QuitGame()
    {
        Save();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }


    public void Save()
    {
        PlayerMovement playerMovementInstance = Player.GetComponent<PlayerMovement>().instance;
        Respawn respawnInstance = Player.GetComponent<Respawn>().instance;
        HealthHeartBarV2 healthHeartBarV2 = Player.GetComponentInChildren<HealthHeartBarV2>().instance;
        SaveSystem.SaveGame(playerMovementInstance, respawnInstance, healthHeartBarV2);
    }


    public void NewGame()
    {
        PlayerMovement playerMovementInstance = Player.GetComponent<PlayerMovement>().instance;
        Respawn respawnInstance = Player.GetComponent<Respawn>().instance;
        HealthHeartBarV2 healthHeartBarV2Instance = Player.GetComponentInChildren<HealthHeartBarV2>().instance;

        ResetSave(ref playerMovementInstance, ref respawnInstance, ref healthHeartBarV2Instance);

        SaveSystem.SaveGame(playerMovementInstance, respawnInstance, healthHeartBarV2Instance);
    }


    private void ResetSave(ref PlayerMovement playerMovementInstance, ref Respawn respawnInstance, ref HealthHeartBarV2 healthHeartBarV2Instance)
    {
        // PlayerMovement Reset
        playerMovementInstance.lastScene = "Junkyard Map";

        playerMovementInstance.unlockedDoubleJump = false;
        playerMovementInstance.unlockedDash = false;
        playerMovementInstance.unlockedWallJump = false;

        playerMovementInstance.maxHealthIncreaseJunkyard = false;
        playerMovementInstance.maxHealthIncreaseMines = false;
        playerMovementInstance.maxHealthIncreaseTower = false;

        playerMovementInstance.defeatedJunkyardBoss = false;
        playerMovementInstance.defeatedMinesBoss = false;
        playerMovementInstance.defeatedTowerBoss = false;


        // Respawn Reset
        respawnInstance.currentSpawnpoint = Vector2.zero;
        respawnInstance.platformingSpawnpoint = Vector2.zero;


        // HealthHeartBarV2 Reset
        healthHeartBarV2Instance.maxHealth = 12f;
        healthHeartBarV2Instance.health = Mathf.Clamp(12f, 0, 12f);
    }


    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }


    private void OnApplicationQuit()
    {
        Save();
    }
}
