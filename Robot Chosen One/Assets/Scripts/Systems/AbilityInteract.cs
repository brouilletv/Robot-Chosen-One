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
            player = collision.transform;
            playerMovement = collision.GetComponent<PlayerMovement>();

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
}
