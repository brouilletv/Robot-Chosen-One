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
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    public static event Action<Transform> NewLocation;

    private bool inRange;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerMovement.interactPressed)
            {
                ChangeLocation(spawnLocation);
                playerMovement.interactPressed = false;
            }
        }
    }

    public void ChangeLocation(Transform spawnLocation)
    {
        NewLocation?.Invoke(spawnLocation);
    }
}
