using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Transform respawnPosition;

    private bool inRange;

    void Update()
    {
        //inRange = Physics2D.OverlapCircle(respawnPosition.position, 0.25f, player.position);
    }
}
