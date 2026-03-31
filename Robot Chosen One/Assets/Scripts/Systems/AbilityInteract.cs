using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInteract : MonoBehaviour
{
    [SerializeField] Canvas interactText;
    [SerializeField] SpriteRenderer abilitySprite;
    private Transform player;
    private PlayerMovement playerMovement;

    [Header("TypeOfUnlock")]
    public bool DashUnlock;
    public bool DoubleJumpUnlock;
    public bool WallJumpUnlock;


    private void UnlockAbility(PlayerMovement playerMovement)
    {
        if (DashUnlock)
        {
            playerMovement.unlockedDash = true;
        }
        else if (DoubleJumpUnlock)
        {
            playerMovement.unlockedDoubleJump = true;
        }
        else if (WallJumpUnlock)
        {
            playerMovement.unlockedWallJump = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null)
        {
            player = collision.transform;
            playerMovement = collision.GetComponent<PlayerMovement>();
        }

        if ((DashUnlock && !playerMovement.unlockedDash) || (DoubleJumpUnlock && !playerMovement.unlockedDoubleJump) || (WallJumpUnlock && !playerMovement.unlockedWallJump))
        {
            interactText.enabled = true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player = collision.transform;
                playerMovement = collision.GetComponent<PlayerMovement>();
            }

            if (playerMovement.interactPressed)
            {
                UnlockAbility(playerMovement);
                playerMovement.interactPressed = false;
                interactText.enabled = false;
                abilitySprite.enabled = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        interactText.enabled = false;
    }
}
