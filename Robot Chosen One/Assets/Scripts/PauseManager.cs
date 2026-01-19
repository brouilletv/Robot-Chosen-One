using UnityEngine;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;
    private PlayerMovement player;


    private void Start()
    {
        PauseMenu.SetActive(false);
    }


    public void OnPause(InputValue value)
    {
        if (!isPaused)
        {
            PauseGame();
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
        player.SwitchActionMap(1);
    }


    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
        PauseMenu.SetActive(false);
        player.SwitchActionMap(0);
    }
}
