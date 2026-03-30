using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SpawnLocation : MonoBehaviour
{

    [SerializeField] CapsuleCollider2D player;
    [SerializeField] CircleCollider2D respawnCollider;
    [SerializeField] Transform spawnLocation;
    [SerializeField] DoorBehaviour doorBehaviour;
    private bool interactPressed = false;

    public static event Action<bool> InteractUpdateFromSpawn;
    public static event Action<Transform> NewLocation;

    private bool inRange;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactPressed)
            {
                ChangeLocation(spawnLocation);
                interactPressed = false;
                InteractUpdateFromSpawn?.Invoke(interactPressed);
            }
        }
    }

    public void ChangeLocation(Transform spawnLocation)
    {
        NewLocation?.Invoke(spawnLocation);
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
