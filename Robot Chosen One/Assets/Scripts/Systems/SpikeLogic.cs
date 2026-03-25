using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpikeLogic : MonoBehaviour
{
    [SerializeField] public int spikeDamage = 1;
    private HealthHeartBarV2 healthScript;

    public static event Action<int> Respawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (healthScript == null)
            {
                healthScript = collision.transform.GetChild(1).GetChild(0).GetComponent<HealthHeartBarV2>();
            }

            healthScript.TakeDamage(spikeDamage);
            Respawn?.Invoke(0);
        }
    }
}
