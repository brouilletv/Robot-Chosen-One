using System;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{   

    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;
    private PlayerMovement playerMovement;


    private void Awake()
    {
        PauseMenu.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>();
    }


    public void OnPause(InputValue value)
    {
        if (!playerMovement.playerStop)
        {
            if (!isPaused)
            {
                PauseGame();
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
