using UnityEngine;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;

    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
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
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
        PauseMenu.SetActive(false);
    }
}
