using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerMainMenu : MonoBehaviour
{
    public Animator fadeAnim;
    public float fadeTime = 1f;
    private Transform player;
    private PlayerMovement playerMovement;
    private Respawn respawn;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.PlayerStopTrue();

        fadeAnim.Play("FadeFromBlack");
    }


    public void PlayGame()
    {
        respawn = player.GetComponent<Respawn>();
        playerMovement.PlayerStopTrue();

        fadeAnim.Play("FadeToBlack");
        StartCoroutine(DelayFade(player, playerMovement, respawn));
    }


    IEnumerator DelayFade(Transform player, PlayerMovement playerMovement, Respawn respawn)
    {
        yield return new WaitForSeconds(fadeTime);
        player.transform.position = respawn.currentSpawnpoint;
        SceneManager.LoadScene(playerMovement.lastScene);
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


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}