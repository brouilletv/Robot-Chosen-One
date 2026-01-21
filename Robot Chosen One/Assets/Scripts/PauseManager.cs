using UnityEngine;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{   

    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;
    private static PlayerInput playerInput;


    private void Awake()
    {
        PauseMenu.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
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
            //ResumeGame();
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;
        PauseMenu.SetActive(true);
        playerInput.SwitchCurrentActionMap("UI");
    }


    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
        PauseMenu.SetActive(false);
        playerInput.SwitchCurrentActionMap("Player");
    }
}
