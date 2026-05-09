using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    private PathFinder PF;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        PF = transform.GetComponent<PathFinder>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        StartCoroutine(PF.Knockback());
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
