using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RegionDoorInteract : MonoBehaviour
{
    [SerializeField] DoorBehaviour doorBehaviour;
    private bool interactPressed = false;

    public static event Action<bool> InteractUpdateFromDoor;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactPressed)
            {
                doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
                interactPressed = false;
                InteractUpdateFromDoor?.Invoke(interactPressed);
            }
        }
    }


    private void UpdateInteractPressed(bool newInteractPressed)
    {
        interactPressed = newInteractPressed;
    }


    private void OnEnable()
    {
        PlayerMovement.InteractUpdateFromPlayer += UpdateInteractPressed;
    }

    private void OnDisable()
    {
        PlayerMovement.InteractUpdateFromPlayer -= UpdateInteractPressed;
    }
}
