using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBarV2 : MonoBehaviour
{
    public GameObject heartPrefab;
    public float maxHealth = 12;
    public float health = 12;

    private List<HealthHeart> hearts = new List<HealthHeart>();

    private void Start()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        DrawHearts();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(1);
        }
    }


    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
        DrawHearts();
    }

    public void TakeDamage(float amount)
    {
        SetHealth(health - amount);
    }

    public void Heal(float amount)
    {
        SetHealth(health + amount);
    }


    private void DrawHearts()
    {
        ClearHearts();

        float remainder = maxHealth % 2;
        int heartsToMake = (int)(maxHealth / 2 + remainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartValue = Mathf.Clamp((int)health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartValue);
        }
    }

    private void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform, false);
        HealthHeart heart = newHeart.GetComponent<HealthHeart>();
        heart.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heart);
    }

    private void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts.Clear();
    }
}