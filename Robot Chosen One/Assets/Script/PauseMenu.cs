using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;


    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
               Pause(); 
            }
            else
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
