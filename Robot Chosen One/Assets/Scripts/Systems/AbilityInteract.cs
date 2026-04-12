using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilityInteract : MonoBehaviour
{
    [SerializeField] Canvas interactText;
    [SerializeField] SpriteRenderer abilitySprite;
    private Transform player;
    private PlayerMovement playerMovement;
    private bool canInteract = true;

    [Header("TypeOfUnlock")]
    public bool DashUnlock;
    public bool DoubleJumpUnlock;
    public bool WallJumpUnlock;


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();

        if (DashUnlock && playerMovement.unlockedDash)
        {
            interactText.enabled = false;
            abilitySprite.enabled = false;
            canInteract = false;
        }
        else if (DoubleJumpUnlock && playerMovement.unlockedDoubleJump)
        {
            interactText.enabled = false;
            abilitySprite.enabled = false;
            canInteract = false;
        }
        else if (WallJumpUnlock && playerMovement.unlockedWallJump)
        {
            interactText.enabled = false;
            abilitySprite.enabled = false;
            canInteract = false;
        }
    }


    private void UnlockAbility(PlayerMovement playerMovement)
    {
        if (DashUnlock && !playerMovement.unlockedDash)
        {
            playerMovement.unlockedDash = true;
        }
        else if (DoubleJumpUnlock && !playerMovement.unlockedDoubleJump)
        {
            playerMovement.unlockedDoubleJump = true;
        }
        else if (WallJumpUnlock && !playerMovement.unlockedWallJump)
        {
            playerMovement.unlockedWallJump = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if ((DashUnlock && !playerMovement.unlockedDash) || (DoubleJumpUnlock && !playerMovement.unlockedDoubleJump) || (WallJumpUnlock && !playerMovement.unlockedWallJump))
            {
                interactText.enabled = true;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerMovement.interactPressed)
            {
                playerMovement.interactPressed = false;
                interactText.enabled = false;
                abilitySprite.enabled = false;
                UnlockAbility(playerMovement);
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


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
