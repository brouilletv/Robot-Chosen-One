using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaxHealthInteract : MonoBehaviour
{
    [SerializeField] Canvas interactText;
    [SerializeField] SpriteRenderer maxHealthIncreaseSprite;
    private Transform player;
    private PlayerMovement playerMovement;
    private HealthHeartBarV2 healthScript;
    private bool canInteract = true;

    [Header("TypeOfMaxHealthIncrease")]
    public bool junkyardMaxHealth;
    public bool minesMaxHealth;
    public bool towerMaxHealth;


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
        healthScript = player.GetChild(1).GetChild(0).GetComponent<HealthHeartBarV2>();

        if (junkyardMaxHealth && playerMovement.maxHealthIncreaseJunkyard)
        {
            interactText.enabled = false;
            maxHealthIncreaseSprite.enabled = false;
            canInteract = false;
        }
        else if (minesMaxHealth && playerMovement.maxHealthIncreaseMines)
        {
            interactText.enabled = false;
            maxHealthIncreaseSprite.enabled = false;
            canInteract = false;
        }
        else if (towerMaxHealth && playerMovement.maxHealthIncreaseTower)
        {
            interactText.enabled = false;
            maxHealthIncreaseSprite.enabled = false;
            canInteract = false;
        }
    }


    private void  IncreaseMaxHealth(PlayerMovement playerMovement, HealthHeartBarV2 healthScript)
    {
        if (junkyardMaxHealth && !playerMovement.maxHealthIncreaseJunkyard)
        {
            canInteract = false;
            playerMovement.maxHealthIncreaseJunkyard = true;
        }
        else if (minesMaxHealth && !playerMovement.maxHealthIncreaseMines)
        {
            healthScript.maxHealth += 2;
            healthScript.Heal(2);
            playerMovement.maxHealthIncreaseMines = true;
        }
        else if (towerMaxHealth && !playerMovement.maxHealthIncreaseTower)
        {
            healthScript.maxHealth += 2;
            healthScript.Heal(2);
            playerMovement.maxHealthIncreaseTower = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if ((junkyardMaxHealth && !playerMovement.maxHealthIncreaseJunkyard) || (minesMaxHealth && !playerMovement.maxHealthIncreaseMines) || (towerMaxHealth && !playerMovement.maxHealthIncreaseTower))
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
                maxHealthIncreaseSprite.enabled = false;
                IncreaseMaxHealth(playerMovement, healthScript);
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
