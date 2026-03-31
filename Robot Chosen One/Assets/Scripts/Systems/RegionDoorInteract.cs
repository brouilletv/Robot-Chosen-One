using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionDoorInteract : MonoBehaviour
{
    [SerializeField] Canvas interactText;
    [SerializeField] DoorBehaviour doorBehaviour;
    private Transform player;
    private PlayerMovement playerMovement;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactText.enabled = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerMovement.interactPressed)
            {
                doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
                playerMovement.interactPressed = false;
                interactText.enabled = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        interactText.enabled = false;
    }
}
