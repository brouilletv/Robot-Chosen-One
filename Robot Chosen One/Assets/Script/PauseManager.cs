using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenu;

    PauseMenu action;

    private void Awake()
    {
        action = new PauseMenu();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Start()
    {
        action.Pause.OpenPauseMenu.performed += _ => PauseToggle();
    }

    private void PauseToggle()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
        PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        PauseMenu.SetActive(false);
    }
}
