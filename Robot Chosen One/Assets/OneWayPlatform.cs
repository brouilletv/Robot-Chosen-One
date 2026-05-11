using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] GameObject currentOneWayPlatform;
    private Transform player;
    private PlayerMovement playerMovement;
    private CapsuleCollider2D playerCollider;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCollider = player.GetComponent<CapsuleCollider2D>();

        playerMovement.jumpForce = 0f;
    }


    private void Update()
    {
        if (playerMovement.moveDirectionY < 0f)
        { 
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
    }
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentOneWayPlatform = currentOneWayPlatform = GameObject.FindWithTag("OneWayPlatform");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
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
