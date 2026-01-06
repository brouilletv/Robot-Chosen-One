using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public int health;
    public int maxHealth;

    private List<HealthHeart> hearts = new List<HealthHeart>();

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        int heartsToMake = Mathf.CeilToInt(maxHealth / 2f);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartHealth = Mathf.Clamp(health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartHealth);
        }
    } 

    void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform, false);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);

        hearts.Add(heartComponent);
    }

    void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        hearts.Clear();
    }
}
