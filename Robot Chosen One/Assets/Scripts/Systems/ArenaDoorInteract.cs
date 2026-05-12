using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaDoorInteract : MonoBehaviour
{
    [SerializeField] GameObject obstacleDoorObject1;
    [SerializeField] GameObject obstacleDoorObject2;
    [SerializeField] GameObject waveSpawner;
    private WaveSpawnerM waveSpawnerScript;
    private DoorBehaviour doorBehaviour1;
    private DoorBehaviour doorBehaviour2;

    private GameObject player;
    private PlayerMovement playerMovement;
    [SerializeField] float avoidDoorClipDistance = 30f;
    [SerializeField] bool movePlayer;
    [SerializeField] Vector3 newPlayerPosition;

    private bool canCloseDoor = true;
    public bool doorOpenDownward;
    public bool doorOpenUpward;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        doorBehaviour1 = obstacleDoorObject1.transform.GetComponentInChildren<DoorBehaviour>();
        doorBehaviour1.doorOpenDownward = doorOpenDownward;
        doorBehaviour1.doorOpenUpward = doorOpenUpward;

        doorBehaviour2 = obstacleDoorObject2.transform.GetComponentInChildren<DoorBehaviour>();
        doorBehaviour2.doorOpenDownward = doorOpenDownward;
        doorBehaviour2.doorOpenUpward = doorOpenUpward;

        waveSpawnerScript = waveSpawner.transform.GetComponent<WaveSpawnerM>();

        doorBehaviour1.isDoorOpen = !doorBehaviour1.isDoorOpen;
        doorBehaviour2.isDoorOpen = !doorBehaviour2.isDoorOpen;

        player = GameObject.FindWithTag("Player");
        playerMovement = player.transform.GetComponent<PlayerMovement>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canCloseDoor)
            {
                if (doorBehaviour1.doorIsOpen && doorBehaviour2.doorIsOpen)
                {
                    playerMovement.PlayerStopTrue();
                    newPlayerPosition = new Vector3(player.transform.position.x + (avoidDoorClipDistance * playerMovement.spriteFacingDirection), player.transform.position.y, player.transform.position.y);
                    movePlayer = true;
                    StartCoroutine(AvoidDoorClipping());

                    doorBehaviour1.isDoorOpen = !doorBehaviour1.isDoorOpen;
                    doorBehaviour2.isDoorOpen = !doorBehaviour2.isDoorOpen;
                    canCloseDoor = false;
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if ((waveSpawnerScript.state == "done") && (doorBehaviour1.doorIsClosed && doorBehaviour2.doorIsClosed))
        {
            doorBehaviour1.isDoorOpen = !doorBehaviour1.isDoorOpen;
            doorBehaviour2.isDoorOpen = !doorBehaviour2.isDoorOpen;
        }

        if (movePlayer)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, newPlayerPosition, playerMovement.groundSpeed * Time.deltaTime);
        }
    }


    private IEnumerator AvoidDoorClipping()
    {
        yield return new WaitUntil(() => (doorBehaviour1.doorIsClosed && doorBehaviour2.doorIsClosed));
        movePlayer = false;
        playerMovement.PlayerStopFalse();
    }


    private void ResetDoors(bool none)
    {
        if (!(waveSpawnerScript.state == "done") && (doorBehaviour1.doorIsClosed && doorBehaviour2.doorIsClosed))
        {
            canCloseDoor = true;
            doorBehaviour1.isDoorOpen = !doorBehaviour1.isDoorOpen;
            doorBehaviour2.isDoorOpen = !doorBehaviour2.isDoorOpen;
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Respawn.resetDoors += ResetDoors;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Respawn.resetDoors -= ResetDoors;
    }
}
