using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInteract : MonoBehaviour
{
    [SerializeField] Canvas interactText1;
    [SerializeField] Canvas interactText2;
    [SerializeField] DoorBehaviour doorBehaviour;
    private GameObject player;
    private PlayerMovement playerMovement;
    private ElevatorState elevatorState;
    private ElevatorState previousElevatorState;
    

    private enum ElevatorState
    {
        ElevatorUp = 0,
        ElevatorDown = 1
    }


    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        doorBehaviour.doorOpenUpward = true;
        doorBehaviour.doorOpenDownward = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!doorBehaviour.doorIsClosed && !doorBehaviour.doorIsOpen)
            {
                interactText1.enabled = false;
                interactText2.enabled = false;
            }
            else if (doorBehaviour.doorIsClosed || doorBehaviour.doorIsOpen)
            {
                interactText1.enabled = true;
                interactText2.enabled = true;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!doorBehaviour.doorIsClosed && !doorBehaviour.doorIsOpen)
            {
                interactText1.enabled = false;
                interactText2.enabled = false;
            }
            else if (doorBehaviour.doorIsClosed || doorBehaviour.doorIsOpen)
            {
                interactText1.enabled = true;
                interactText2.enabled = true;

                if (playerMovement.interactPressed)
                {
                    playerMovement.interactPressed = false;
                    previousElevatorState = elevatorState;
                    playerMovement.PlayerStopTrue();
                    doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
                }
            }

            if (doorBehaviour.doorIsClosed)
            {
                elevatorState = ElevatorState.ElevatorDown;
            }
            else if (doorBehaviour.doorIsOpen)
            {
                elevatorState = ElevatorState.ElevatorUp;
            }

            if (elevatorState != previousElevatorState)
            {
                playerMovement.PlayerStopFalse();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText1.enabled = false;
            interactText2.enabled = false;
        }
    }
}
