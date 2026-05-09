using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoorInteract : MonoBehaviour
{
    [SerializeField] GameObject obstacleDoorObject1;
    [SerializeField] GameObject obstacleDoorObject2;
    private DoorBehaviour doorBehaviour1;
    private DoorBehaviour doorBehaviour2;
    private bool canCloseDoor = true;
    public bool doorOpenDownward;
    public bool doorOpenUpward;

    private GameObject player;
    private PlayerMovement playerMovement;

    [Header("TypeOfBoss")]
    public bool JunkyardBoss = false;
    public bool MinesBoss = false;
    public bool TowerBoss = false;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        doorBehaviour1 = obstacleDoorObject1.transform.GetComponentInChildren<DoorBehaviour>();
        doorBehaviour1.doorOpenDownward = doorOpenDownward;
        doorBehaviour1.doorOpenUpward = doorOpenUpward;

        doorBehaviour2 = obstacleDoorObject2.transform.GetComponentInChildren<DoorBehaviour>();
        doorBehaviour2.doorOpenDownward = doorOpenDownward;
        doorBehaviour2.doorOpenUpward = doorOpenUpward;

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
                    doorBehaviour1.isDoorOpen = !doorBehaviour1.isDoorOpen;
                    doorBehaviour2.isDoorOpen = !doorBehaviour2.isDoorOpen;
                    canCloseDoor = false;
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if ((playerMovement.defeatedJunkyardBoss == true && JunkyardBoss) || (playerMovement.defeatedMinesBoss == true && MinesBoss) || (playerMovement.defeatedTowerBoss == true && TowerBoss))
        {
            if (doorBehaviour1.doorIsClosed && doorBehaviour2.doorIsClosed)
            {
                doorBehaviour1.isDoorOpen = !doorBehaviour1.isDoorOpen;
                doorBehaviour2.isDoorOpen = !doorBehaviour2.isDoorOpen;
                canCloseDoor = false;
            }
        }
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
