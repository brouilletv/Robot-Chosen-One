using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerM : MonoBehaviour
{
    private Transform MaxPos;
    private Transform MinPos;

    private GameObject Player;
    private LayerMask playerMask;

    void Start()
    {
        MaxPos = transform?.Find("MaxPos");
        MinPos = transform?.Find("MinPos");

        playerMask = LayerMask.GetMask("PlayerMask");
        Player = System.Array.Find(FindObjectsOfType<GameObject>(), o => ((1 << o.layer) & playerMask) != 0);
    }

    void Update()
    {
        //add that it start when the player enter the zone
    }
}
