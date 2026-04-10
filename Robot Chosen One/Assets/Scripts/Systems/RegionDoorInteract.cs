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


    private void Awake()
    {
        doorBehaviour.doorOpenUpward = true;
        doorBehaviour.doorOpenDownward = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            playerMovement = collision.GetComponent<PlayerMovement>();
            if (!doorBehaviour.doorIsClosed && !doorBehaviour.doorIsOpen)
            {
                interactText.enabled = false;
            }
            else if (doorBehaviour.doorIsClosed || doorBehaviour.doorIsOpen)
            {
                interactText.enabled = true;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!doorBehaviour.doorIsClosed && !doorBehaviour.doorIsOpen)
            {
                interactText.enabled = false;
            }
            else if (doorBehaviour.doorIsClosed || doorBehaviour.doorIsOpen)
            {
                interactText.enabled = true;

                if (playerMovement.interactPressed)
                {
                    playerMovement.interactPressed = false;
                    doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
                }
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
}
