using UnityEngine;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenu;
    public InputActionReference pause;

    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        pause.action.started += PauseToggle;
    }

    private void OnDisable()
    {
        pause.action.started -= PauseToggle;
    }

    private void PauseToggle(InputAction.CallbackContext obj)
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
