using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawnerM : MonoBehaviour
{
    private Transform MaxPos;
    private Transform MinPos;

    private GameObject Player;
    private LayerMask playerMask;

    public string state = "inactive";
    private int w = 0;

    void Start()
    {
        MaxPos = transform.Find("MaxPos");
        MinPos = transform.Find("MinPos");

        playerMask = LayerMask.GetMask("PlayerMask");
        Player = System.Array.Find(FindObjectsOfType<GameObject>(), o => ((1 << o.layer) & playerMask) != 0);
    }

    void Update()
    {
        if (Player.transform.position.x >= MinPos.position.x && Player.transform.position.x <= MaxPos.position.x && Player.transform.position.y >= MinPos.position.y && Player.transform.position.y <= MaxPos.position.y && state == "inactive")
        {
            state = "active";
        }
        else if (state == "active")
        {
            foreach (int wave in Enumerable.Range(2, transform.childCount - 2))
            {
                if (transform.GetChild(wave).childCount > 0 && transform.GetChild(wave - 1).childCount == 0 && wave != w)
                {
                    w = wave;
                    foreach (int enemy in Enumerable.Range(0, transform.GetChild(wave).childCount))
                    {
                        transform.GetChild(wave).GetChild(enemy).GetComponent<WaveSpawnerS>().Spawn();
                    }
                }
                else if (transform.GetChild(wave).name == "Last Wave" && transform.GetChild(wave).childCount == 0)
                {
                    state = "done";
                }
            }
        }
    }
}
