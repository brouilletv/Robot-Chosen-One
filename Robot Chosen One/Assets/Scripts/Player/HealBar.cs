using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealBar : MonoBehaviour
{
    private GameObject player;
    private PlayerMelee playerMelee;
    private Slider slider;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerMelee = player.GetComponent<PlayerMelee>();

        slider = GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        SetHealBar();
    }

    public void SetHealBar()
    {
        slider.value = playerMelee.attackCount / 10f;
    }
}