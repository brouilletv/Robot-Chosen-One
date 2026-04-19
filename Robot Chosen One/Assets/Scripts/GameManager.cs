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
        SaveSystem.SaveGame(playerMovementInstance, respawnInstance);
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
