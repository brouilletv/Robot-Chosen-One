using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator fadeAnim;
    public float fadeTime = 1f;
    public Vector2 newPlayerPosition;
    private Transform player;
    private PlayerMovement playerMovement;
    private Respawn respawn;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();

        fadeAnim.Play("FadeFromBlack");

        playerMovement.PlayerStopFalse();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            playerMovement = player.GetComponent<PlayerMovement>();
            respawn = player.GetComponent<Respawn>();
            playerMovement.PlayerStopTrue();

            fadeAnim.Play("FadeToBlack");
            StartCoroutine(DelayFade(player, playerMovement, respawn));
        }
    }


    IEnumerator DelayFade(Transform player, PlayerMovement playerMovement, Respawn respawn)
    {
        yield return new WaitForSeconds(fadeTime);
        player.transform.position = newPlayerPosition;
        respawn.currentSpawnpoint.position = newPlayerPosition;
        playerMovement.lastScene = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad);
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
