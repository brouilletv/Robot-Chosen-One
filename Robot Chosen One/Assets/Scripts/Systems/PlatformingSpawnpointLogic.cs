using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingSpawnpointLogic : MonoBehaviour
{
    [SerializeField] private Transform spawnpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Respawn>().platformingSpawnpoint = spawnpoint.position;
        }
    }
}
