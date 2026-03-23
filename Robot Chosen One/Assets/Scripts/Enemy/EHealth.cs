using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    private float currentHealth;
    private float cooldownTime = 1f;
    private bool cooldown = false;

    void Start()
    {
        currentHealth = maxHealth; 
    }

    void Update()
    {

    }

    public void TakeDamage(float amount)
    {
        if (cooldown == false)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }
}
