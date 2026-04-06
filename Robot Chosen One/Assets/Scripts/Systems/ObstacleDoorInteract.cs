using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleDoorInteract : MonoBehaviour
{
    [SerializeField] GameObject obstacleDoorObject;
    private Canvas interactText;
    private DoorBehaviour doorBehaviour;
    private ObstacleDoorColliderInteract colliderInteract;
    private Transform player;
    private PlayerMovement playerMovement;
    private SpriteRenderer leverOpen;
    private SpriteRenderer leverClosed;

    private bool canOpenDoor = true;
    public bool doorOpenDownward;
    public bool doorOpenUpward;


    private void Awake()
    {
        interactText = GetComponentInChildren<Canvas>();
        doorBehaviour = obstacleDoorObject.transform.GetComponentInChildren<DoorBehaviour>();
        colliderInteract = GetComponentInChildren<ObstacleDoorColliderInteract>();
        leverOpen = transform.Find("LeverOpen").GetComponent<SpriteRenderer>();
        leverClosed = transform.Find("LeverClosed").GetComponent<SpriteRenderer>();

        doorBehaviour.doorOpenDownward = doorOpenDownward;
        doorBehaviour.doorOpenUpward = doorOpenUpward;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canOpenDoor)
            {
                player = collision.transform;
                playerMovement = collision.GetComponent<PlayerMovement>();
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
                if (playerMovement.interactPressed && canOpenDoor)
                {
                    interactText.enabled = false;
                    leverOpen.enabled = false;
                    leverClosed.enabled = true;

                    playerMovement.interactPressed = false;
                    canOpenDoor = false;
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


    private void FixedUpdate()
    {
        if (colliderInteract.canOpenDoorFromOtherSide)
        {
            if (canOpenDoor)
            {
                canOpenDoor = false;
                doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
            }
        }
    }
}
