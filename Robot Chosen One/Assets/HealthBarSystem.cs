using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSystem : MonoBehaviour
{
    public GameObject heartPrefab;
    public int playerHealth = 5;
    public int maxHealth = 12;
    List<HealthBarSystem> hearts = new List<HealthBarSystem>();

    public void DrawHearts()
    {
        ClearHearts();
        float maxHealthRemainder = maxHealth % 2;
        int heartsToMake = (int)(maxHealth / 2 + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthBarSystem heartComponent = newHeart.GetComponent<HealthBarSystem>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in Transform)
        {
            Destroy(t.GameObject);
        }
        hearts = new List<HealthBarSystem>();
    }
}