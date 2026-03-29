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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            fadeAnim.Play("FadeToBlack");
            StartCoroutine(DelayFade(player));
        }
    }
    IEnumerator DelayFade(Transform player)
    {
        yield return new WaitForSeconds(fadeTime);
        player.transform.position = newPlayerPosition;
        SceneManager.LoadScene(sceneToLoad);
    }
}
