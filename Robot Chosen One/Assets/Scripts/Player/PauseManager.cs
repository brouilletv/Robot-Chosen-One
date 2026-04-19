using System;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{   
    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;
    private GameManager GameManager;
    private PlayerMovement playerMovement;


    private void Awake()
    {
        PauseMenu.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Start()
    {
        GameManager = GameObject.FindWithTag("GameManager").transform.GetComponent<GameManager>();
    }


    public void OnPause(InputValue value)
    {
        if (!playerMovement.playerStop)
        {
            if (!isPaused)
            {
                PauseGame();
                GameManager.Save();
            }
        }
    }


    public void OnResume(InputValue value)
    {
        if (isPaused)
        {
            ResumeGame();
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;
        PauseMenu.SetActive(true);
        playerMovement.SwitchActionMapToUI();
    }


    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
        PauseMenu.SetActive(false);
        playerMovement.SwitchActionMapToPlayer();
    }


    public void QuitGame()
    {
        GameManager.Save();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
}