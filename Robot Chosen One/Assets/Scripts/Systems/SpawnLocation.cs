using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] Canvas interactText;
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    public static event Action<Transform> NewLocation;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.enabled = true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerMovement.interactPressed)
            {
                ChangeLocation(spawnLocation);
                interactText.enabled = false;
                playerMovement.interactPressed = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.enabled = false;
        }
    }


    public void ChangeLocation(Transform spawnLocation)
    {
        NewLocation?.Invoke(spawnLocation);
    }
}
