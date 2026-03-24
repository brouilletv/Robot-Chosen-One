using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLogic : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    private HealthHeartBarV2 healthScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (healthScript == null)
            {
                healthScript = collision.transform.GetChild(1).GetChild(0).GetComponent<HealthHeartBarV2>();
            }

            if (healthScript.health > 1)
            {
                healthScript.TakeDamage(1);
            }
            else
            {
                healthScript.TakeDamage(1);
            }
        }
    }
}
