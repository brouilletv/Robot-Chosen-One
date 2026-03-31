using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerPauseMenu : MonoBehaviour
{
    public Animator fadeAnim;
    public float fadeTime = 1f;
    private string mainMenu = "Main Menu";
    private Transform player;
    private PlayerMovement playerMovement;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeAnim.Play("FadeFromBlack");
    }


    public void MainMenu()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.PlayerStopTrue();

        fadeAnim.Play("FadeToBlack");
        StartCoroutine(DelayFade());
    }


    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(mainMenu);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
